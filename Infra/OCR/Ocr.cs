using System;
using System.Linq;
using System.Threading.Tasks;
using ExtractOcrApi.Infra.DTO;
using Microsoft.AspNetCore.Hosting;

namespace ExtractOcrApi.Infra.OCR
{
  public class Ocr
  {
    private readonly IHostingEnvironment _environment;
    private readonly Arquivo _arquivo;
    private readonly ExtracaoDeTexto _extracaoDeTexto;

    public Ocr(IHostingEnvironment environment, Arquivo arquivo, ExtracaoDeTexto extracaoDeTexto)
    {
      _environment = environment;
      _arquivo = arquivo;
      _extracaoDeTexto = extracaoDeTexto;
    }

    public async Task<ExtracaoDeTextoDto> ExtrairTextoDaImagem(string url, string extensao)
    {
      bool ehValidoParametros = ValidarParametros(url, extensao);
      if (!ehValidoParametros)
        return new ExtracaoDeTextoDto { Erro = "Parametros invalido" };

      var resultado = await _arquivo.Obter(url, extensao, _environment.ContentRootPath);
      if (!resultado.Sucesso) return new ExtracaoDeTextoDto { Erro = resultado.Erro };

      var textoExtraido = await _extracaoDeTexto.Extrair(extensao, resultado.CaminhoDoArquivo);
      await _arquivo.Excluir(resultado.CaminhoDoArquivo);

      return new ExtracaoDeTextoDto { Texto = textoExtraido };
    }

    private static bool ValidarParametros(string url, string extensao)
    {
      return !string.IsNullOrEmpty(url)
        && (extensao.Contains("pdf")
        || extensao.Contains("docx")
        || extensao.Contains("jpeg")
        || extensao.Contains("jpg"));
    }
  }
}