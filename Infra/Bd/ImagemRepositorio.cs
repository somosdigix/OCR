using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using OCR.DTO;

namespace OCR.Infra.Bd
{
    public class ImagemRepositorio
    {
        public Imagem BuscarImagemNaoProcessada(){
            
            var connectionString = "Data Source=VESUVIO;Initial Catalog=horus_dev;User ID=horus;Password=hrs;";
            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var nomeDoArquivo = GravarArquivo(connection);
                connection.Close();
                return nomeDoArquivo;
            }
        }

        private Imagem GravarArquivo(SqlConnection connection)
        {
            Imagem imagem = null;
            
            using (var command = connection.CreateCommand())
            {
                var statusDeNaoProcessado = 0;

                command.CommandText =
                    $@"SELECT top 1 imagem_cd, imagem, nm_imagem FROM Imagens 
                        WHERE situacaodoocr = {statusDeNaoProcessado}
                        ORDER BY imagem_cd ASC";

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.Write("Leu");
                        
                        var codigoDaImagem = reader.GetInt32(0);
                        var stream = reader.GetStream(1);
                        var nomeDoArquivo = Guid.NewGuid().ToString();
                        var fileStream = File.Create($"{nomeDoArquivo}.pdf");
                        stream.CopyTo(fileStream);
                        fileStream.Close();

                        imagem = new Imagem{ Id = codigoDaImagem, Endereco = $"{Environment.CurrentDirectory}/{nomeDoArquivo}"};
                    }
                }
            }

            AtualizarParaProcessando(connection, imagem.Id);
            return imagem;
        }

        public void AtualizarParaProcessado(Imagem imagem, string resultadoDoOcr)
        {
            var connectionString = "Data Source=VESUVIO;Initial Catalog=horus_dev;User ID=horus;Password=hrs;";
            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    var statusDeProcessadoDaImagem = 2;
                    const int statusDoOcrProcessadoDoDocumento = 3; 

                    command.CommandText =
                        $@"BEGIN
                        UPDATE Imagens SET resultadoocr = '{resultadoDoOcr.Replace("'", string.Empty)}', situacaodoocr = {statusDeProcessadoDaImagem} WHERE imagem_cd = {imagem.Id};
                        DECLARE @doc INT = (SELECT TOP 1 doc_cd FROM imagens WHERE imagem_cd = {imagem.Id})

                            IF ((SELECT COUNT(*) FROM imagens WHERE doc_cd = @doc AND situacaodoocr = 0) = 0)
                            BEGIN
                                UPDATE doc SET doc_ok = {statusDoOcrProcessadoDoDocumento} WHERE doc_cd = @doc;
                            END
                        END";

                    command.ExecuteScalar();
                }
                connection.Close();
            }
        }

        private static void AtualizarParaProcessando(SqlConnection connection, int codigoDaImagem)
        {
            using (var command = connection.CreateCommand())
            {
                var statusDeProcessando = 1;

                command.CommandText =
                    $@"UPDATE Imagens set situacaodoocr = {statusDeProcessando}
                        WHERE imagem_cd = {codigoDaImagem}";

                command.ExecuteScalar();
            }
        }

        public async Task RemoverArquivo(Imagem imagem)
        {
            await Task.Run(() => System.IO.File.Delete($"{imagem.Endereco}.pdf"));
            await Task.Run(() => System.IO.File.Delete($"{imagem.Endereco}.png"));
        }
    }
}