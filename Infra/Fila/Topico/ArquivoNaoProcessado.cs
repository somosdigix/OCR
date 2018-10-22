using System;
using ExtractOcrApi.Infra.DTO;
using ExtractOcrApi.Infra.Fila.Configuracao;
using ExtractOcrApi.Infra.OCR;
using Newtonsoft.Json;

namespace ExtractOcrApi.Infra.Fila.Topico
{
  public class ArquivoNaoProcessado
  {
    private ConfiguracaoDaFila _configuracaoDaFila;
    private Ocr _ocr;
    private ArquivoProcessado _arquivoProcessado;
    private ArquivoComErro _arquivoComErro;

    public ArquivoNaoProcessado(ConfiguracaoDaFila configuracaoDaFila, Ocr ocr,
      ArquivoProcessado arquivoProcessado, ArquivoComErro arquivoComErro)
    {
      _configuracaoDaFila = configuracaoDaFila;
      _arquivoProcessado = arquivoProcessado;
      _arquivoComErro = arquivoComErro;
      _ocr = ocr;
    }

    public void Consumir()
    {
      using (var consumidor = _configuracaoDaFila.ObterConsumidor())
      {
        consumidor.OnMessage += (_, mensagem)
          =>
        {
          var arquivoDto = JsonConvert.DeserializeObject<ArquivoDto>(mensagem.Value);
          var resultado = _ocr.ExtrairTextoDaImagem(arquivoDto.Url, "jpg").Result;
          var naoHaErro = string.IsNullOrEmpty(resultado.Erro);
          if (naoHaErro)
          {
            arquivoDto.Texto = resultado.Texto;
            _arquivoProcessado.Produzir(arquivoDto);
            Console.WriteLine("Extraiu texto do arquivo");
          }
          else
          {
            _arquivoComErro.Produzir(mensagem.Value);
          }
        };

        consumidor.Subscribe(Ambiente.TopicoDoArquivoNaoProcessado);

        while (true)
        {
          consumidor.Poll(TimeSpan.FromMilliseconds(100));
        }
      }
    }
  }
}