using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace Ocr.Infra.Monitoramento
{
  public class LogDeErro
  {
    private IHostingEnvironment _environment;

    public LogDeErro(IHostingEnvironment environment)
    {
      _environment = environment;
    }
    public string ObterErrosDaDataAtual()
    {
      var erros = new StringBuilder();
      var caminhoDoArquivo = ObterCaminhoDoArquivo();
      if (File.Exists(caminhoDoArquivo))
      {
        var textos = File.ReadAllLines(caminhoDoArquivo);
        foreach (string texto in textos)
        {
          erros.AppendLine(texto);
        }
      }
      return erros.ToString();
    }

    public void Logar(string mensagem)
    {
      var caminhoDoArquivo = ObterCaminhoDoArquivo();
      using (StreamWriter sw = new StreamWriter(caminhoDoArquivo, true))
      {
        sw.WriteLine(mensagem);
      }
    }

    private string ObterCaminhoDoArquivo()
    {
      var dataAtual = DateTime.Now.ToShortDateString().Replace("/", "_");
      return $@"{_environment.ContentRootPath}\logs\log_{dataAtual}.txt";
    }
  }
}