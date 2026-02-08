using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Menedżer_notatek_i_rysunków.Services
{
    public class DrawingService //drawing service and audio service would inherit from the same class, maybe in future
    {
        private readonly string _drawingsDir;

        public DrawingService(string drawingsDir)
        {
            _drawingsDir = drawingsDir;
        }

        public void EnsureDirectoryExists()
        {
            if (string.IsNullOrWhiteSpace(_drawingsDir))
                return;
            Directory.CreateDirectory(_drawingsDir);
        }

        public string GetDrawingPathForNote(System.Guid noteId)
        {
            return Path.Combine(_drawingsDir, $"{noteId}.png");
        }

        public Bitmap? LoadBitmapCopy(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return null;

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var temp = new Bitmap(fs);
            return new Bitmap(temp);
        }

        public Bitmap CreateBlankBitmap(int width = 800, int height = 600)
        {
            var bmp = new Bitmap(width, height);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            return bmp;
        }

        public void SaveBitmap(string path, Bitmap bitmap)
        {
            if (bitmap is null || string.IsNullOrWhiteSpace(path))
                return;

            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            using var copy = new Bitmap(bitmap);
            copy.Save(path, ImageFormat.Png);
        }

        private void DeleteDrawing(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            
            var fullPath = Path.GetFullPath(path);

           
            if (!File.Exists(fullPath))
                return;

            File.Delete(fullPath);
        }

        public void DeleteDrawingForNote(System.Guid noteId)
        {
            var path = GetDrawingPathForNote(noteId);
            DeleteDrawing(path);
        }

    }
}
