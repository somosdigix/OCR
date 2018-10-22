using System.Threading.Tasks;
using Ocr.Infra.Fila.Configuracao;

namespace Ocr.Infra.Fila.Topico
{
  public class ArquivoComErro
  {
    private ConfiguracaoDaFila _configuracaoDaFila;

    public ArquivoComErro(ConfiguracaoDaFila configuracaoDaFila)
    {
      _configuracaoDaFila = configuracaoDaFila;
    }

    public void Produzir(string mensagem)
    {
      using (var produtor = _configuracaoDaFila.ObterProdutor())
      {
        produtor.ProduceAsync(Ambiente.TopicoDoArquivoComErro, null, mensagem);
      }
    }
  }
}