using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ocr.DTO;

namespace Ocr.Infra.ExtracaoDeOcr
{
  public class Arquivo
  {
    public async Task<ProcessoDoArquivoDto> Obter(string url, string extensao, string raizDoCaminho)
    {
      using (var cliente = new HttpClient())
      {
        cliente.DefaultRequestHeaders.Add("token", "87e118a8-6206-4628-b6e9-e62fb5346c9d");
        using (var resultado = await cliente.GetAsync(url))
        {
          if (!resultado.IsSuccessStatusCode)
            return new ProcessoDoArquivoDto { Sucesso = false, Erro = "Erro ao obter o arquivo" };
          
          var caminhoDoArquivo = GerarCaminhoDoArquivo(extensao, raizDoCaminho);
          bool salvou = await Salvar(resultado, caminhoDoArquivo);
          if (!salvou)
            return new ProcessoDoArquivoDto { Sucesso = false, Erro = "Erro ao salvar o arquivo" };

          return new ProcessoDoArquivoDto { Sucesso = true, CaminhoDoArquivo = caminhoDoArquivo };
        }
      }
    }

    private static string GerarCaminhoDoArquivo(string extensao, string raizDoCaminho)
    {
      var nomeDoArquivo = $"{Guid.NewGuid().ToString()}.{extensao}";
      var caminhoDoArquivo = Path.Combine(raizDoCaminho, $"{nomeDoArquivo}");
      return caminhoDoArquivo;
    }

    private static async Task<bool> Salvar(HttpResponseMessage resultado, string caminhoDoArquivo)
    {
      try
      {
        var bytes = await resultado.Content.ReadAsByteArrayAsync();
        await System.IO.File.WriteAllBytesAsync(caminhoDoArquivo, bytes);
        return true;
      }
      catch { return false; }
    }

    public async Task Excluir(string path)
    {
      await Task.Run(() => System.IO.File.Delete(path));
    }
  }
}