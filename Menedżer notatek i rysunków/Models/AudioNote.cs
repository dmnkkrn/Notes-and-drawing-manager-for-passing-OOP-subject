using System;
using System.IO;

namespace Menedżer_notatek_i_rysunków.Models
{
    public class AudioNote
    {

        public string FilePath { get; set; }

        public AudioNote()
        {
            FilePath = string.Empty;
        }

        public AudioNote(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public bool IsEmpty => string.IsNullOrWhiteSpace(FilePath);

        public string GetFullPath()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                return string.Empty;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(baseDir, FilePath));
        }
    }
}
