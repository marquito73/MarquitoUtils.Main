using System.Security.Cryptography;
using System.Text;

namespace MarquitoUtils.Main.Common.Tools.Encryption
{
    public class Encrypter
    {
        /// <summary>
        /// Hash a password
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>The password hashed</returns>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert password to byte array
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Convert bytes to string
                StringBuilder builder = new StringBuilder();
                // Convert to hexadecimal
                bytes.ToList().ForEach(x => builder.Append(x.ToString("x2")));

                return builder.ToString();
            }
        }
    }
}
