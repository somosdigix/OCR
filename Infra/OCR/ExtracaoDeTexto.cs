using System;
using System.Threading.Tasks;

namespace ExtractOcrApi.Infra.OCR
{
    public class ExtracaoDeTexto
    {
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