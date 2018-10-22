using ExtractOcrApi.Infra.DTO;
using ExtractOcrApi.Infra.Fila.Configuracao;
using Newtonsoft.Json;

namespace ExtractOcrApi.Infra.Fila.Topico
{
  public class ArquivoProcessado
  {
    private ConfiguracaoDaFila _configuracaoDaFila;

    public ArquivoProcessado(ConfiguracaoDaFila configuracaoDaFila)
    {
      _configuracaoDaFila = configuracaoDaFila;
    }

    public void Produzir(ArquivoDto arquivoDto)
    {
      using (var produtor = _configuracaoDaFila.ObterProdutor())
      {
        var arquivoJson = JsonConvert.SerializeObject(arquivoDto);
        produtor.ProduceAsync(Ambiente.TopicoDoArquivoProcessado, null, arquivoJson);
      }
    }
  }
}