namespace Ocr.Infra.Fila.Configuracao
{
  public static class Ambiente
  {
    private static string topicoDoArquivoNaoProcessado;
    private static string topicoDoArquivoProcessado;
    private static string hostDaFila;
    private static string topicoDoArquivoComErro;

    public static string TopicoDoArquivoNaoProcessado
    {
      get => string.IsNullOrEmpty(topicoDoArquivoNaoProcessado)
        ? "ArquivosNaoProcessados"
        : topicoDoArquivoNaoProcessado;
      set
      {
        topicoDoArquivoNaoProcessado = value;
      }
    }

    public static string TopicoDoArquivoProcessado
    {
      get => string.IsNullOrEmpty(topicoDoArquivoProcessado)
        ? "ArquivosProcessados"
        : topicoDoArquivoProcessado;
      set
      {
        topicoDoArquivoProcessado = value;
      }
    }

    public static string TopicoDoArquivoComErro
    {
      get => string.IsNullOrEmpty(topicoDoArquivoComErro)
        ? "ArquivosComErro"
        : topicoDoArquivoComErro;
      set
      {
        topicoDoArquivoComErro = value;
      }
    }

    public static string HostDaFila
    {
      get => string.IsNullOrEmpty(hostDaFila)
          ? "localhost:9092"
          : hostDaFila;
      set
      {
        hostDaFila = value;
      }
    }
  }
}