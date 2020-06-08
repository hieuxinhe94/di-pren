using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;


namespace DIClassLib.Security
{

    /// <summary>
    /// This class hopefully enables encryption/decryption between ASP.NET and PHP.
    /// </summary>
    public class EncryptDecrypt
    {

        //Shared 256 bit Key and IV here - 32 chr shared ascii string (32 * 8 = 256 bit)
        string _key = "d7ac4fe8850b49dc889b9a70c9f6a971";  //"lkirwf897+22#bbtrm8814z5qq=498j5";
        string _iv  = "729499d80857409ab83e72869465425e";  //"741952hheeyy66#cs!9hjv887mxx7@8y";

        

        public EncryptDecrypt()
        {
            //string sTextVal = "Here is my data to encrypt";
            //string eText = Encrypt(sTextVal);         
            //string dText = Decrypt(eText);
        }
            
            
        //public string DecryptRJ256(string prm_key, string prm_iv, string prm_text_to_decrypt)          
        public string Decrypt(string stringToDecrypt)          
        {
            string sEncryptedString = stringToDecrypt;          
            RijndaelManaged myRijndael = new RijndaelManaged();         
            myRijndael.Padding = PaddingMode.Zeros;
            myRijndael.Mode = CipherMode.CBC;         
            myRijndael.KeySize = 256;         
            myRijndael.BlockSize = 256;          
            byte[] key;         
            byte[] IV;          
            key = System.Text.Encoding.ASCII.GetBytes(_key);         
            IV = System.Text.Encoding.ASCII.GetBytes(_iv);          
            ICryptoTransform decryptor = myRijndael.CreateDecryptor(key, IV);          
            byte[] sEncrypted = Convert.FromBase64String(sEncryptedString);          
            byte[] fromEncrypt = new byte[sEncrypted.Length];
            MemoryStream msDecrypt = new MemoryStream(sEncrypted);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

            //return (System.Text.Encoding.ASCII.GetString(fromEncrypt));
            return TrimNulls(fromEncrypt);
        }

            
        //public string EncryptRJ256(string prm_key, string prm_iv, string prm_text_to_encrypt)          
        public string Encrypt(string stringToEncrypt)          
        {
            string sToEncrypt = stringToEncrypt;          
            RijndaelManaged myRijndael = new RijndaelManaged();         
            myRijndael.Padding = PaddingMode.Zeros;         
            myRijndael.Mode = CipherMode.CBC;         
            myRijndael.KeySize = 256;         
            myRijndael.BlockSize = 256;          
            byte[] encrypted;         
            byte[] toEncrypt;         
            byte[] key;         
            byte[] IV;          
            key = System.Text.Encoding.ASCII.GetBytes(_key);         
            IV = System.Text.Encoding.ASCII.GetBytes(_iv);          
            ICryptoTransform encryptor = myRijndael.CreateEncryptor(key, IV);          
            MemoryStream msEncrypt = new MemoryStream();         
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);          
            toEncrypt = System.Text.Encoding.ASCII.GetBytes(sToEncrypt);          
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);         
            csEncrypt.FlushFinalBlock();          
            encrypted = msEncrypt.ToArray();          
    
            return (Convert.ToBase64String(encrypted));
        }


        private string TrimNulls(byte[] data)
        {
            int rOffset = data.Length - 1;
            for (int i = data.Length - 1; i >= 0; i--)
            {
                rOffset = i;
                if (data[i] != (byte)0) 
                    break;
            }

            return System.Text.Encoding.ASCII.GetString(data, 0, rOffset + 1);
        }

    }
}
