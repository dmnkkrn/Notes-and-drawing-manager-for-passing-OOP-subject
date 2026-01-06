namespace Menedżer_notatek_i_rysunków
{
    partial class FormDrawing
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            clearButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(10, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(778, 348);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // clearButton
            // 
            clearButton.Location = new Point(564, 363);
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(224, 39);
            clearButton.TabIndex = 1;
            clearButton.Text = "Clear";
            clearButton.UseVisualStyleBackColor = true;
            clearButton.Click += clearButton_Click;
            // 
            // FormDrawing
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(clearButton);
            Controls.Add(pictureBox1);
            Name = "FormDrawing";
            Text = "Drawing";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Button clearButton;
    }
}