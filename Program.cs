using System;
using Ocr.Infra.Fila.Configuracao;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Ocr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InserirVariaveisDeAmbiente();
            CreateWebHostBuilder(args).Build().Run();
        }

        private static void InserirVariaveisDeAmbiente()
        {
            Ambiente.ApplicationInsightsId = Environment.GetEnvironmentVariable("APPLICATION_INSIGHTS_ID");
            Console.WriteLine($"APPLICATION_INSIGHTS_ID: {Ambiente.ApplicationInsightsId}");

            Ambiente.TopicoDoArquivoNaoProcessado = Environment.GetEnvironmentVariable("FILA_DE_DOCUMENTOS_NAO_PROCESSADOS");
            Console.WriteLine($"FILA_DE_DOCUMENTOS_NAO_PROCESSADOS: {Ambiente.TopicoDoArquivoNaoProcessado}");

            Ambiente.TopicoDoArquivoProcessado = Environment.GetEnvironmentVariable("FILA_DE_DOCUMENTOS_PROCESSADOS");
            Console.WriteLine($"FILA_DE_DOCUMENTOS_PROCESSADOS: {Ambiente.TopicoDoArquivoProcessado}");

            Ambiente.TopicoDoArquivoComErro = Environment.GetEnvironmentVariable("FILA_DE_ERRO_NO_PROCESSAMENTO_DOS_DOCUMENTOS");
            Console.WriteLine($"FILA_DE_ERRO_NO_PROCESSAMENTO_DOS_DOCUMENTOS: {Ambiente.TopicoDoArquivoComErro}");

            Ambiente.HostDaFila = Environment.GetEnvironmentVariable("HOST_KAFKA");
            Console.WriteLine($"HOST_KAFKA: {Ambiente.HostDaFila}");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                    .UseKestrel()
                    .UseUrls("http://*:3000/")
                    .UseStartup<Startup>();
    }
}
