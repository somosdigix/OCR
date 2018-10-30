using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ocr.DTO;
using Ocr.Infra.ExtracaoDeOcr;

namespace Ocr.Controllers
{
  [Route("/")]
  [ApiController]
  public class OcrController : ControllerBase
  {
    private readonly ExtracaoDeTexto _ocr;

    public OcrController(ExtracaoDeTexto ocr)
    {
      _ocr = ocr;
    }

    [HttpGet, Route("")]
    public async Task<ExtracaoDeTextoDto> Ocr(string url, string tipo)
    {
      return await _ocr.ExtrairTextoDaImagem(url, tipo);
    }
  }
}
