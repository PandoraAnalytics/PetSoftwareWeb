using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BABusiness
{
    public class BASecurity
    {
        public static string Encrypt(string xiInputString, string xiKey)
        {
            if (string.IsNullOrEmpty(xiInputString)) xiInputString = string.Empty;

            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(xiInputString);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(xiKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        xiInputString = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch { xiInputString = string.Empty; }
            if (xiInputString == null) xiInputString = string.Empty;
            return xiInputString;
        }

        public static string Decrypt(string xiInputString, string xiKey)
        {
            if (string.IsNullOrEmpty(xiInputString)) xiInputString = string.Empty;

            try
            {
                xiInputString = xiInputString.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(xiInputString);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(xiKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        xiInputString = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch { xiInputString = string.Empty; }
            if (xiInputString == null) xiInputString = string.Empty;
            return xiInputString;
        }

        public static string HashPassword(string xiInputString)
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(xiInputString, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(32);
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyHash(string xiHashText1, string xiHashText2)
        {
            byte[] hashBytes = Convert.FromBase64String(xiHashText2);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(xiHashText1, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(32);
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }
    }
}
