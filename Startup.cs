using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Ocr.Infra.Fila.Configuracao;
using Ocr.Infra.Fila.Topico;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocr.Infra.ExtracaoDeOcr;
using Ocr.Infra.Monitoramento;
using OCR.Infra.Fila.Topico;

namespace Ocr
{
    public class Startup
    {
        public IConfiguration Configuration;
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            var serviceProvider = services
                .AddSingleton<IHostingEnvironment>(_environment)
                .AddSingleton<Arquivo>(new Arquivo())
                .AddSingleton<ExtracaoDeTexto>()
                .AddSingleton<ConfiguracaoDaFila>()
                .AddSingleton<ArquivoNaoProcessado>()
                .AddSingleton<ArquivoProcessado>()
                .AddSingleton<ArquivoComErro>()
                .AddSingleton<ExecucaoDoWorker>()
                .AddSingleton<ApplicationInsights>()
                .BuildServiceProvider();

            serviceProvider.GetService<ExecucaoDoWorker>().LogDaConexao();
            var arquivoNaoProcessado = serviceProvider.GetService<ArquivoNaoProcessado>();
            Task.Run(() => arquivoNaoProcessado.Consumir());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InicializarApplicationInsights();

            var applicationInsights = (ApplicationInsights) app.ApplicationServices.GetService(typeof(ApplicationInsights));
            applicationInsights.NovoEvento("OCR: Iniciado");

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

        private static void InicializarApplicationInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = "720b68b3-e987-404f-bb69-0a5ba5844814";
        }
    }
}
