using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Persistence.Security
{
    public class EncryptionService : IEncryptionService
    {
        private const string Header = "ENCNaDM01";
        private const int KeySize = 256;
        private const int Iterations = 100_000;

        public void EncryptFile(string inputPath, string outputPath, string password)
        {
            byte[] headerBytes = Encoding.ASCII.GetBytes(Header);
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            using var aes = Aes.Create();
            aes.KeySize = KeySize;

            using var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            aes.Key = key.GetBytes(KeySize / 8);
            aes.GenerateIV();

            using var inputFile = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
            using var outputFile = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

            outputFile.Write(headerBytes, 0, headerBytes.Length);
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
            using var inputFile = new FileStream(inputPath, FileMode.Open, FileAccess.Read);

            byte[] headerBytes = new byte[Header.Length];
            inputFile.Read(headerBytes, 0, headerBytes.Length);

            if (Encoding.ASCII.GetString(headerBytes) != Header)
                throw new InvalidOperationException("Invalid encryption header.");

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

            using var outputFile = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            cryptoStream.CopyTo(outputFile);
        }

        public bool IsEncrypted(string path)
        {
            if (!File.Exists(path))
                return false;

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (fs.Length < Header.Length)
                return false;

            byte[] headerBytes = new byte[Header.Length];
            fs.Read(headerBytes, 0, headerBytes.Length);

            return Encoding.ASCII.GetString(headerBytes) == Header;
        }
    }
}
