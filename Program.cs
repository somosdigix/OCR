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
