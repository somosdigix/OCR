using System;
using Ocr.Infra.Fila;
using Ocr.Infra.Fila.Configuracao;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Ocr
{
  public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            Ambiente.TopicoDoArquivoNaoProcessado = Environment.GetEnvironmentVariable("FILA_DE_DOCUMENTOS_NAO_PROCESSADOS");
            Ambiente.TopicoDoArquivoProcessado = Environment.GetEnvironmentVariable("FILA_DE_DOCUMENTOS_PROCESSADOS");
            Ambiente.TopicoDoArquivoComErro = Environment.GetEnvironmentVariable("FILA_DE_ERRO_NO_PROCESSAMENTO_DOS_DOCUMENTOS");
            Ambiente.HostDaFila = Environment.GetEnvironmentVariable("localhost:9092");
            Console.WriteLine("Aplicação de OCR está sendo executada");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls("http://*:3000/")
                .UseStartup<Startup>();
    }
}
