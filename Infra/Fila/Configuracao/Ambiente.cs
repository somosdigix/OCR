namespace Ocr.Infra.Fila.Configuracao
{
  public static class Ambiente
  {
    private static string _topicoDoArquivoNaoProcessado;
    private static string _topicoDoArquivoProcessado;
    private static string _hostDaFila;
    private static string _topicoDoArquivoComErro;

    public static string TopicoDoArquivoNaoProcessado
    {
      get => string.IsNullOrEmpty(_topicoDoArquivoNaoProcessado)
        ? "ArquivosNaoProcessados"
        : _topicoDoArquivoNaoProcessado;
      set
      {
        _topicoDoArquivoNaoProcessado = value;
      }
    }

    public static string TopicoDoArquivoProcessado
    {
      get => string.IsNullOrEmpty(_topicoDoArquivoProcessado)
        ? "ArquivosProcessados"
        : _topicoDoArquivoProcessado;
      set
      {
        _topicoDoArquivoProcessado = value;
      }
    }

    public static string TopicoDoArquivoComErro
    {
      get => string.IsNullOrEmpty(_topicoDoArquivoComErro)
        ? "ArquivosComErro"
        : _topicoDoArquivoComErro;
      set
      {
        _topicoDoArquivoComErro = value;
      }
    }

    public static string HostDaFila
    {
      get => string.IsNullOrEmpty(_hostDaFila)
          ? "localhost:9092"
          : _hostDaFila;
      set
      {
        _hostDaFila = value;
      }
    }
  }
}