using Menedżer_notatek_i_rysunków.Services;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Menedżer_notatek_i_rysunków
{
    public partial class FormDrawing : Form
    {

        private Bitmap _bitmap;
        private Graphics _graphics;
        private bool _isDrawing;
        private Point _lastPoint;
        private readonly string _imagePath;
        private readonly DrawingService _drawingService;

        public FormDrawing(string imagePath, DrawingService drawingService)
        {
            InitializeComponent();

            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;

            _imagePath = imagePath;
            _drawingService = drawingService;

            var loaded = _drawingService.LoadBitmapCopy(_imagePath);
            if (loaded != null)
            {
                _bitmap = loaded;
            }
            else
            {
                _bitmap = _drawingService.CreateBlankBitmap(800, 600);
            }

            _graphics = Graphics.FromImage(_bitmap);
            pictureBox1.Image = _bitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _isDrawing = true;
            _lastPoint = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing)
                return;

            _graphics.DrawLine(Pens.Black, _lastPoint, e.Location);
            _lastPoint = e.Location;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            pictureBox1.Image = null;

            _graphics.Dispose();
            _graphics = null;

            
            _drawingService.SaveBitmap(_imagePath, _bitmap);
            _bitmap.Dispose();

            base.OnFormClosing(e);
            
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            pictureBox1.Invalidate();
        }
    }
}
