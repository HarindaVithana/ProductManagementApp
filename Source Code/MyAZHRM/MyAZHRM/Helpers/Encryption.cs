using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Description;

namespace MyAZHRM.Helpers
{
    public class Encryption
    {
        byte[] key;
        byte[] aesKey;
        byte[] aesIV;
        static readonly char[] padding = { '=' };

        public Encryption()
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            key = UE.GetBytes("testtest==");
            aesKey = SHA256Managed.Create().ComputeHash(key);
            aesIV = MD5.Create().ComputeHash(key);
        }
        public string EncryptedKey(string origanikey)
        {
            string returnValue = "";
            byte[] encryptedText;

            using (Aes aes = Aes.Create())
            {
                encryptedText = EncryptStringToBytes(origanikey, aesKey, aesIV);
            }

            return returnValue = System.Convert.ToBase64String(encryptedText).TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }

        public string DecryptedKey(string encryptedkey)
        {

            string decryptedText;
            //byte[] encryptedText = Convert.FromBase64String(Base64UrlEncoder.Decode(encryptedkey));
            //byte[] encryptedText = ;

            string incoming = encryptedkey.Replace('_', '/').Replace('-', '+');
            switch (encryptedkey.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }

            byte[] encryptedText = Convert.FromBase64String(incoming);

            using (Aes aes = Aes.Create())
            {
                decryptedText = DecryptStringFromBytes(encryptedText, aesKey, aesIV);
            }
            return decryptedText;
        }

        private static string DecryptStringFromBytes(byte[] cipherKey, byte[] aesKey, byte[] iV)
        {
            string decrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = iV;

                // Create a CryptoStream object to perform the encryption.
                using (MemoryStream memoryStream = new MemoryStream(cipherKey))
                {
                    // Create a CryptoStream object to perform the decryption.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        // Decrypt the ciphertext.
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            decrypted = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }

        private static byte[] EncryptStringToBytes(string original, byte[] aesKey, byte[] iV)
        {
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = iV;

                // Create a new MemoryStream object to contain the encrypted bytes.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Create a CryptoStream object to perform the encryption.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Encrypt the plaintext.
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(original);
                        }

                        encrypted = memoryStream.ToArray();
                    }
                }
            }

            return encrypted;
        }
    }

}