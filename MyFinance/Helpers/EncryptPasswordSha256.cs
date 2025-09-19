using System.Security.Cryptography;
using System.Text;

namespace MyFinance.Helpers
{
    public class EncryptPasswordSha256
    {
        /// <summary>
        /// Método que encripta una cadena de texto utilizando el algoritmo SHA256.
        /// </summary>
        /// <param name="password">Cadena de texto que se quiere encriptar.</param>
        /// <returns>Cadena de texto encriptada.</returns>
        public static string Encrypt(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                var builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}