using System;
using System.Collections.Generic;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Persistence.Security
{
    public interface IEncryptionService
    {
        void EncryptFile(string inputFilePath, string outputFilePath, string password);
        void DecryptFile(string inputFilePath, string outputFilePath, string password);
        bool IsEncrypted(string filePath);
    }
}
