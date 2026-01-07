using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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

        public FormDrawing(string imagePath)
        {
            InitializeComponent();

            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;

            _imagePath = imagePath;

            if (File.Exists(_imagePath))
            {
                using (var fs = new FileStream(_imagePath, FileMode.Open, FileAccess.Read))
                using (var temp = new Bitmap(fs))
                {
                    _bitmap = new Bitmap(temp);
                }
            }
            else
            {
                _bitmap = new Bitmap(800, 600);
                using var g = Graphics.FromImage(_bitmap);
                g.Clear(Color.White);
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

            var dir = Path.GetDirectoryName(_imagePath);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            _bitmap.Save(_imagePath, System.Drawing.Imaging.ImageFormat.Png);
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
