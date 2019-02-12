using Ocr.Infra.Fila.Configuracao;
using Ocr.Infra.Monitoramento;

namespace Ocr.Infra.Fila.Topico
{
  public class ArquivoComErro
  {
    private readonly ConfiguracaoDaFila _configuracaoDaFila;
    private readonly LogDeErro _logDeErro;

    public ArquivoComErro(ConfiguracaoDaFila configuracaoDaFila, LogDeErro logDeErro)
    {
      _configuracaoDaFila = configuracaoDaFila;
      _logDeErro = logDeErro;
    }

    public void Produzir(string mensagem)
    {
      using (var produtor = _configuracaoDaFila.ObterProdutor())
      {
        produtor.ProduceAsync(Ambiente.TopicoDoArquivoComErro, null, mensagem);
        _logDeErro.Logar(mensagem);
      }
    }
  }
}