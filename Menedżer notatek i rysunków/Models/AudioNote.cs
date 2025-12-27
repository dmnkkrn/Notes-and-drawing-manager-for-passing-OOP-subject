using System;
using System.Collections.Generic;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Models
{
    public class AudioNote
    {
        public string FilePath { get; private set; }

        public AudioNote(string filePath)
        {
            FilePath = filePath;
        }
    }
}
