using System;
using System.IO;
using System.Security.Cryptography;

namespace SteganographyLibrary {
    public static class Cryptography {
        public static byte[] Encrypt(byte[] data, string password, int keySize = 256) {
            using var aes = Aes.Create();
            aes.KeySize = keySize;
            aes.GenerateIV();
            aes.Key = new PasswordDeriveBytes(password, Array.Empty<byte>()).GetBytes(aes.KeySize / 8);

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var memoryStreamEncrypt = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStreamEncrypt, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            var encryptedData = memoryStreamEncrypt.ToArray();
            var cipherData = new byte[encryptedData.Length + aes.IV.Length];
            Array.Copy(aes.IV, 0, cipherData, 0, aes.IV.Length);
            Array.Copy(encryptedData, 0, cipherData, aes.IV.Length, encryptedData.Length);

            return cipherData;
        }

        public static byte[] Decrypt(byte[] data, string password, int keySize = 256) {
            using var aes = Aes.Create();
            aes.KeySize = keySize;
            aes.Key = new PasswordDeriveBytes(password, Array.Empty<byte>()).GetBytes(aes.KeySize / 8);

            var iv = new byte[aes.BlockSize / 8];
            var cipherData = new byte[data.Length - iv.Length];

            Array.Copy(data, 0, iv, 0, iv.Length);
            Array.Copy(data, iv.Length, cipherData, 0, cipherData.Length);

            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var memoryStreamEncrypt = new MemoryStream(cipherData);
            using var cryptoStream = new CryptoStream(memoryStreamEncrypt, decryptor, CryptoStreamMode.Read);

            var plainCipherStream = new MemoryStream();
            cryptoStream.CopyTo(plainCipherStream);

            return plainCipherStream.ToArray();
        }

        public static string GenerateSecurePassword(int length) {
            var secureRandom = new RNGCryptoServiceProvider();
            var secureBytes = new byte[length];
            secureRandom.GetNonZeroBytes(secureBytes);

            return Convert.ToBase64String(secureBytes);
        }
    }
}
