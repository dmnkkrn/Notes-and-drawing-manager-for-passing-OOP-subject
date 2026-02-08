namespace Menedżer_notatek_i_rysunków.Models
{
    public class Drawing
    {
        public string ImagePath { get; set; }

        public Drawing(string imagePath)
        {
            ImagePath = imagePath;
        }
    }
}
