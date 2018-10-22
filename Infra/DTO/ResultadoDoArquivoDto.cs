namespace ExtractOcrApi.Infra.DTO
{
  public class ProcessoDoArquivoDto
  {
    public string CaminhoDoArquivo { get; set; }
    public bool Sucesso { get; set; }
    public string Erro { get; set; }
  }
}