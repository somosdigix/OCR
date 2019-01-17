using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace Ocr.Infra.Fila.Configuracao
{
    public class ConfiguracaoDaFila
    {
        private readonly Dictionary<string, object> _configuracaoDaFila;
        public ConfiguracaoDaFila()
        {
            _configuracaoDaFila = new Dictionary<string, object> {
                { "group.id", "horus-group" },
                { "bootstrap.servers", Ambiente.HostDaFila },
                { "auto.commit.interval.ms", 5000 },
                { "auto.offset.reset", "earliest" }
            };
        }

        public Consumer<Null, string> ObterConsumidor() =>
          new Consumer<Null, string>(_configuracaoDaFila, null, new StringDeserializer(Encoding.UTF8));

        public Producer<Null, string> ObterProdutor() =>
          new Producer<Null, string>(_configuracaoDaFila, null, new StringSerializer(Encoding.UTF8));
    }
}