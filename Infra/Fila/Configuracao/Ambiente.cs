namespace Ocr.Infra.Fila.Configuracao
{
    public static class Ambiente
    {
        private static string _topicoDoArquivoNaoProcessado;
        private static string _hostDaFila;
        private static string _topicoDoArquivoComErro;

        public static string TopicoDoArquivoNaoProcessado
        {
            get => string.IsNullOrEmpty(_topicoDoArquivoNaoProcessado)
              ? "ArquivosNaoProcessados_Dev"
              : _topicoDoArquivoNaoProcessado;
            set => _topicoDoArquivoNaoProcessado = value;
        }

        public static string TopicoDoArquivoProcessado
        {
            get => string.IsNullOrEmpty(_topicoDoArquivoNaoProcessado)
              ? "ArquivosProcessados_Dev"
              : _topicoDoArquivoNaoProcessado;
            set => _topicoDoArquivoNaoProcessado = value;
        }

        public static string TopicoDoArquivoComErro
        {
            get => string.IsNullOrEmpty(_topicoDoArquivoComErro)
              ? "ArquivosComErro_Dev"
              : _topicoDoArquivoComErro;
            set => _topicoDoArquivoComErro = value;
        }

        public static string HostDaFila
        {
            get => string.IsNullOrEmpty(_hostDaFila)
                ? "localhost:9092"
                : _hostDaFila;
            set => _hostDaFila = value;
        }
    }
}