using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Persistence.Security
{
    public class EncryptionService
    {
        private const int KeySize = 256;
        private const int Iterations = 100_000;

        public void EncryptFile(string inputPath, string outputPath, string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            using var aes = Aes.Create();
            aes.KeySize = KeySize;

            using var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            aes.Key = key.GetBytes(KeySize / 8);
            aes.GenerateIV();

            using var inputFile = new FileStream(inputPath, FileMode.Open);
            using var outputFile = new FileStream(outputPath, FileMode.Create);

            outputFile.Write(salt, 0, salt.Length);
            outputFile.Write(aes.IV, 0, aes.IV.Length);

            using var cryptoStream = new CryptoStream(
                outputFile,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write
            );

            inputFile.CopyTo(cryptoStream);
        }

        public void DecryptFile(string inputPath, string outputPath, string password)
        {
            using var inputFile = new FileStream(inputPath, FileMode.Open);

            byte[] salt = new byte[16];
            byte[] iv = new byte[16];

            inputFile.Read(salt, 0, salt.Length);
            inputFile.Read(iv, 0, iv.Length);

            using var aes = Aes.Create();
            aes.KeySize = KeySize;

            using var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            aes.Key = key.GetBytes(KeySize / 8);
            aes.IV = iv;

            using var cryptoStream = new CryptoStream(
                inputFile,
                aes.CreateDecryptor(),
                CryptoStreamMode.Read
            );

            using var outputFile = new FileStream(outputPath, FileMode.Create);
            cryptoStream.CopyTo(outputFile);
        }
    }
}
