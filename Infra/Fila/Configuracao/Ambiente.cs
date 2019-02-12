namespace Ocr.Infra.Fila.Configuracao
{
  public static class Ambiente
  {
    private static string _topicoDoArquivoNaoProcessado;
    private static string _topicoDoArquivoProcessado;
    private static string _hostDaFila;
    private static string _topicoDoArquivoComErro;
    private static string _applicationInsights;

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
        ? "ArquivosProcessados_Dev"
        : _topicoDoArquivoProcessado;
      set => _topicoDoArquivoProcessado = value;
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
      set => _hostDaFila = value;
    }

    public static string ApplicationInsightsId
    {
      get => string.IsNullOrEmpty(_applicationInsights)
          ? "720b68b3-e987-404f-bb69-0a5ba5844814"
          : _applicationInsights;
      set => _applicationInsights = value;
    }
  }
}
