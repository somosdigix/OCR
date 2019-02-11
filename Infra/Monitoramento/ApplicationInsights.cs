using Microsoft.ApplicationInsights;

namespace Ocr.Infra.Monitoramento
{
    public class ApplicationInsights
    {
        private readonly TelemetryClient _telemetria;

        public ApplicationInsights()
        {
            _telemetria = new TelemetryClient();
        }

        public void NovoEvento(string nomeDoEvento)
        {
            _telemetria.TrackEvent(nomeDoEvento);
        }
    }
}
