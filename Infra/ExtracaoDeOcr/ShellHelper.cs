using System.Diagnostics;
using System.Threading.Tasks;

namespace Ocr.Infra.ExtracaoDeOcr
{
  public static class ShellHelper
  {
    public async static Task<string> Bash(this string cmd)
    {
      var processo = ObterProcesso(cmd);
      processo.Start();
      var resultado = await processo.StandardOutput.ReadToEndAsync();
      processo.WaitForExit();
      return resultado;
    }

    private static Process ObterProcesso(string cmd)
    {
      var argumentos = cmd.Replace("\"", "\\\"");
      
      return new Process()
      {
        StartInfo = new ProcessStartInfo("cmd.exe")
        {
          //FileName = "/bin/bash",
          Arguments = $"-c \"{argumentos}\"",
          RedirectStandardOutput = true,
          UseShellExecute = false,
          CreateNoWindow = true,
        }
      };
    }
  }
}