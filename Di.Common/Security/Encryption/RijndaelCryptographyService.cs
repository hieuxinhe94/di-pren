using System.IO;
using System.Security.Cryptography;
using System.Text;
using Di.Common.Utils.ByteArray;

namespace Di.Common.Security.Encryption
{
    public class RijndaelCryptographyService : ICryptographyService
    {
        public string EncryptString(string message, string key, string iv)
        {
            var keyArray = Encoding.UTF8.GetBytes(key);
            var ivArray = Encoding.UTF8.GetBytes(iv);

            var rj = new RijndaelManaged();
            rj.Key = keyArray;
            rj.IV = ivArray;
            rj.Mode = CipherMode.CBC;

            var ms = new MemoryStream();

            using (var cs = new CryptoStream(ms, rj.CreateEncryptor(keyArray, ivArray), CryptoStreamMode.Write))
            {
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(message);
                    sw.Close();
                }
                cs.Close();
            }
            var encoded = ms.ToArray();
            string encrypted = ByteArrayUtils.ConvertToHex(encoded);
            
            ms.Close();

            rj.Clear();

            return encrypted;
        }

        public string DecryptString(string message, string key, string iv)
        {
            var keyArray = Encoding.UTF8.GetBytes(key);
            var ivArray = Encoding.UTF8.GetBytes(iv);

            string plaintext;

            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = keyArray;
                rijAlg.IV = ivArray;

                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (var msDecrypt = new MemoryStream(ByteArrayUtils.ConvertToByteArray(message)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
    }
}