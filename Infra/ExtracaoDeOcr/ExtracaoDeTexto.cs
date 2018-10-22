using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Ocr.DTO;

namespace Ocr.Infra.ExtracaoDeOcr
{
  public class ExtracaoDeTexto
  {
    private readonly IHostingEnvironment _environment;
    private readonly Arquivo _arquivo;

    public ExtracaoDeTexto(IHostingEnvironment environment, Arquivo arquivo)
    {
      _environment = environment;
      _arquivo = arquivo;
    }

    public async Task<ExtracaoDeTextoDto> ExtrairTextoDaImagem(string url, string extensao)
    {
      bool ehValidoParametros = ValidarParametros(url, extensao);
      if (!ehValidoParametros)
        return new ExtracaoDeTextoDto { Erro = "Parametros invalido" };

      var resultado = await _arquivo.Obter(url, extensao, _environment.ContentRootPath);
      if (!resultado.Sucesso) return new ExtracaoDeTextoDto { Erro = resultado.Erro };

      var textoExtraido = await Extrair(extensao, resultado.CaminhoDoArquivo);
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

    public async Task<string> Extrair(string extensao, string caminhoDoArquivo)
    {
      if (extensao == "jpeg" || extensao == "jpg" || extensao == "png")
        return await $"tesseract {caminhoDoArquivo} stdout".Bash();
      if (extensao == "pdf")
        return await $"pdf2txt.py {caminhoDoArquivo}".Bash();
      if (extensao == "docx")
        return await $"docx2txt {caminhoDoArquivo}".Bash();

      return string.Empty;
    }
  }
}