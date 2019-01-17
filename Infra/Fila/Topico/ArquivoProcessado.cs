using Ocr.Infra.Fila.Configuracao;
using Newtonsoft.Json;
using Ocr.DTO;

namespace Ocr.Infra.Fila.Topico
{
    public class ArquivoProcessado
    {
        private readonly ConfiguracaoDaFila _configuracaoDaFila;

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