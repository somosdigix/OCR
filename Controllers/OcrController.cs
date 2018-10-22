using System.Threading.Tasks;
using ExtractOcrApi.Infra.DTO;
using ExtractOcrApi.Infra.OCR;
using Microsoft.AspNetCore.Mvc;

namespace ExtractOcrApi.Controllers
{
  [Route("/")]
  [ApiController]
  public class OcrController : ControllerBase
  {
    private readonly Ocr _ocr;

    public OcrController(Ocr ocr)
    {
      _ocr = ocr;
    }

    [HttpGet, Route("")]
    public string Info()
    {
      return
          @"GET /ocr?url=url_do_documento&tipo=(pdf, docx, png, jpeg, jpg)";
    }

    [HttpGet, Route("ocr")]
    public async Task<ExtracaoDeTextoDto> Ocr(string url, string tipo)
    {
      return await _ocr.ExtrairTextoDaImagem(url, tipo);
    }
  }
}
