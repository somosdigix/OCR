using System;
using Ocr.Infra.Fila.Configuracao;

namespace OCR.Infra.Fila.Topico
{
  public class ExecucaoDoWorker
  {
    private ConfiguracaoDaFila _configuracaoDaFila;
    public ExecucaoDoWorker(ConfiguracaoDaFila configuracaoDaFila)
    {
      _configuracaoDaFila = configuracaoDaFila;
    }

    public void LogDaConexao(int tempoDeTentativa = 5000)
    {
      bool estaOnline = false;

      using (var produtor = _configuracaoDaFila.ObterProdutor())
      {
        var task = produtor.ProduceAsync("ExecucaoDosWorkes", null, $"Work do OCR iniciado as {DateTime.Now}");
        estaOnline = task.Wait(tempoDeTentativa);
      }

      if (estaOnline)
      {
        Console.WriteLine($"Sucesso ao conectar com kafka em {Ambiente.HostDaFila}");
        return;
      }
      Console.WriteLine($"Falha ao conectar com kafka {Ambiente.HostDaFila}");
      LogDaConexao(10000);
    }
  }
}