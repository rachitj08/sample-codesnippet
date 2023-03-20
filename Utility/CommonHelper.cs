using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public class CommonHelper: ICommonHelper
    {
        public Dictionary<string, string> GetTenantTheme(string cssValue)
        {
            var rtnValue = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(cssValue)) return null;
            foreach (var theme in cssValue.Split("|", StringSplitOptions.RemoveEmptyEntries))
            {
                var themeKeyValue = theme.Split(":");
                if (themeKeyValue.Length > 1
                    && !string.IsNullOrWhiteSpace(themeKeyValue[0])
                    && !string.IsNullOrWhiteSpace(themeKeyValue[1]))
                {
                    rtnValue.Add(themeKeyValue[0], themeKeyValue[1]);
                }
            }
            return rtnValue;
        }

        public string EncryptString(string input, string key)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(input);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    input = Convert.ToBase64String(ms.ToArray());
                }
            }
            return input;
        }

        public string DecryptString(string input, string key)
        {
            try
            {
                input = input.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(input);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        input = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return input;
            }
            catch
            {

                return null;
            }
        }

        public string DecryptStringForPayment(string encryptedValue,string key)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes(key);
                var iv = Encoding.UTF8.GetBytes(key);
                //DECRYPT FROM CRIPTOJS
                var encrypted = Convert.FromBase64String(encryptedValue);
                var decryptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                return decryptedFromJavascript;
            }
            catch
            {
                return string.Empty;
            }
        }
        private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("");
            }
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = key;
                rijAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
