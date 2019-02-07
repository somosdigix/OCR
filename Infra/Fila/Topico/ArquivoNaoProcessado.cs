using System;
using Ocr.Infra.Fila.Configuracao;
using Newtonsoft.Json;
using Ocr.Infra.ExtracaoDeOcr;
using Ocr.DTO;
using Ocr.Infra.Monitoramento;

namespace Ocr.Infra.Fila.Topico
{
    public class ArquivoNaoProcessado
    {
        private readonly ConfiguracaoDaFila _configuracaoDaFila;
        private readonly ExtracaoDeTexto _ocr;
        private readonly ArquivoProcessado _arquivoProcessado;
        private readonly ArquivoComErro _arquivoComErro;
        private readonly ApplicationInsights _applicationInsights;

        public ArquivoNaoProcessado(
            ConfiguracaoDaFila configuracaoDaFila, ExtracaoDeTexto ocr,
            ArquivoProcessado arquivoProcessado, ArquivoComErro arquivoComErro, ApplicationInsights applicationInsights)
        {
            _configuracaoDaFila = configuracaoDaFila;
            _arquivoProcessado = arquivoProcessado;
            _arquivoComErro = arquivoComErro;
            _applicationInsights = applicationInsights;
            _ocr = ocr;
        }

        public void Consumir()
        {
            Console.WriteLine("Iniciando comunicação com Kafka");

            using (var consumidor = _configuracaoDaFila.ObterConsumidor())
            {
                Console.WriteLine("Comunicação com Kafka feita com sucesso");

                consumidor.OnMessage += (_, mensagem)
                  =>
                {
                    Console.WriteLine($"Nova mensagem {DateTime.Now}");

                    try
                    {
                        ProcessarNovaMensagem(mensagem);
                    }
                    catch
                    {
                        Console.WriteLine("Falha na mensagem");

                        _applicationInsights.NovoEvento("OCR:Erro no consumidor");
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

        private void ProcessarNovaMensagem(Confluent.Kafka.Message<Confluent.Kafka.Null, string> mensagem)
        {
            _applicationInsights.NovoEvento("OCR:Nova imagem");
            var arquivoDto = JsonConvert.DeserializeObject<ArquivoDto>(mensagem.Value);
            var resultado = _ocr.ExtrairTextoDaImagem(arquivoDto.Url, "jpg").Result;
            var naoHaErro = string.IsNullOrEmpty(resultado.Erro);
            if (naoHaErro)
            {
                Console.WriteLine("Extraiu texto do arquivo");
                arquivoDto.Texto = resultado.Texto;
                _arquivoProcessado.Produzir(arquivoDto);
                _applicationInsights.NovoEvento("OCR:Imagem processada");
            }
            else
            {
                Console.WriteLine($"{resultado.Erro} - {arquivoDto.Url}");
                _arquivoComErro.Produzir(mensagem.Value);
            }
        }
    }
}