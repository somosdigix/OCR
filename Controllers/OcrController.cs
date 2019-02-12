using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ocr.DTO;
using Ocr.Infra.ExtracaoDeOcr;
using Ocr.Infra.Monitoramento;

namespace Ocr.Controllers
{
  [Route("/")]
  [ApiController]
  public class OcrController : ControllerBase
  {
    private readonly ExtracaoDeTexto _ocr;
    private readonly LogDeErro _logDeErro;

    public OcrController(ExtracaoDeTexto ocr, LogDeErro logDeErro)
    {
      _ocr = ocr;
      _logDeErro = logDeErro;
    }

    [HttpGet, Route("")]
    public ActionResult Obter() {
      return Ok("OCR está funcionando");
    }

    [HttpGet, Route("/erros")]
    public ActionResult ObterErros()
    {
      var erros = _logDeErro.ObterErrosDaDataAtual();
      return Ok(erros);
    }

    [HttpGet, Route("processar")]
    public async Task<ExtracaoDeTextoDto> Ocr(string url, string tipo)
    {
      if (!string.IsNullOrEmpty(url))
        return await _ocr.ExtrairTextoDaImagem(url, tipo);
      return null;
    }
  }
}
