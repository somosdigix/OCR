using System;
using Ocr.Infra.Fila;
using Ocr.Infra.Fila.Configuracao;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using OCR.Infra.Bd;
using Ocr.Infra.ExtracaoDeOcr;

namespace Ocr
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var repositorio = new ImagemRepositorio();
      var imagem = repositorio.BuscarImagemNaoProcessada();
      Console.WriteLine(imagem.Id);
      
      $"convert -density 300 -trim {imagem.Endereco}.pdf -quality 100 {imagem.Endereco}.png".Bash().Wait();
      var ocrDaImagem = $"tesseract {imagem.Endereco}.png stdout".Bash().Result;

      Console.WriteLine(ocrDaImagem);

      repositorio.RemoverArquivo(imagem).Wait();
      repositorio.AtualizarParaProcessado(imagem, ocrDaImagem);

      //CreateWebHostBuilder(args).Build().Run();
      //InserirVariaveisDeAmbiente();
    }

    private static void InserirVariaveisDeAmbiente()
    {
      Ambiente.TopicoDoArquivoNaoProcessado = Environment.GetEnvironmentVariable("FILA_DE_DOCUMENTOS_NAO_PROCESSADOS");
      Ambiente.TopicoDoArquivoProcessado = Environment.GetEnvironmentVariable("FILA_DE_DOCUMENTOS_PROCESSADOS");
      Ambiente.TopicoDoArquivoComErro = Environment.GetEnvironmentVariable("FILA_DE_ERRO_NO_PROCESSAMENTO_DOS_DOCUMENTOS");
      Ambiente.HostDaFila = Environment.GetEnvironmentVariable("HOST_KAFKA");
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls("http://*:3000/")
                .UseStartup<Startup>();
  }
}
