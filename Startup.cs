using System.Threading.Tasks;
using ExtractOcrApi.Infra.Fila.Configuracao;
using ExtractOcrApi.Infra.Fila.Topico;
using ExtractOcrApi.Infra.OCR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExtractOcrApi
{
  public class Startup
  {
    public IConfiguration _configuration;
    private IHostingEnvironment _environment;

    public Startup(IConfiguration configuration, IHostingEnvironment environment)
    {
      _configuration = configuration;
      _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      var serviceProvider = services
        .AddSingleton<IHostingEnvironment>(_environment)
        .AddSingleton<Arquivo>(new Arquivo())
        .AddSingleton<ExtracaoDeTexto>(new ExtracaoDeTexto())
        .AddSingleton<Ocr>()
        .AddSingleton<ConfiguracaoDaFila>()
        .AddSingleton<ArquivoNaoProcessado>()
        .AddSingleton<ArquivoProcessado>()
        .AddSingleton<ArquivoComErro>()
        .BuildServiceProvider();

      var arquivoNaoProcessado = serviceProvider.GetService<ArquivoNaoProcessado>();
      Task.Run(() => arquivoNaoProcessado.Consumir());
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}
