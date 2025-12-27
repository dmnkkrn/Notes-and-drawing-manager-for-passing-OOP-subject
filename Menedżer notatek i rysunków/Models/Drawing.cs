using System;
using System.Collections.Generic;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Models
{
    public class Drawing
    {
        public byte[] ImageData { get; private set; }

        public Drawing(byte[] imageData)
        {
            ImageData = imageData;
        }
    }

}
