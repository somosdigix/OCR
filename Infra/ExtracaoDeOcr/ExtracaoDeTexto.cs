using System;
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
            ValidarParametros(url, extensao);

            var resultado = await _arquivo.Obter(url, extensao, _environment.ContentRootPath);
            if (!resultado.Sucesso) return new ExtracaoDeTextoDto { Erro = resultado.Erro };

            var textoExtraido = await Extrair(extensao, resultado.CaminhoDoArquivo);
            await _arquivo.Excluir(resultado.CaminhoDoArquivo);

            return new ExtracaoDeTextoDto { Texto = textoExtraido };
        }

        private void ValidarParametros(string url, string extensao)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(extensao))
            {
                throw new Exception("GET ?url=url_do_documento&tipo=(pdf, docx, png, jpeg, jpg)");
            }
            else if (!(extensao.Contains("jpg")
                || extensao.Contains("jpeg")
                || extensao.Contains("png")
                || extensao.Contains("pdf")
                || extensao.Contains("docx")))
            {
                throw new Exception($"A extensão {extensao} é invalido");
            }
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