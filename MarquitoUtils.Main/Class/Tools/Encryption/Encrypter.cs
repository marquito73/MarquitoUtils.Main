using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Tools.Encryption
{
    public class Encrypter
    {
        public static string EncryptString(string strToEncrypt)
        {
            /*StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(strToEncrypt));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();*/

            byte[] result = Utils.GetAsBytes(strToEncrypt);

            AesCng aes = new AesCng();
            aes.EncryptEcb(result, PaddingMode.None);

            return "";



            //byte[] encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);
        }

        public static string DecryptString(string strToDecrypt)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(strToDecrypt));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
