namespace Menedżer_notatek_i_rysunków
{
    partial class AudioForm
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
            audioName = new Label();
            playButton = new Button();
            stopButton = new Button();
            deleteButton = new Button();
            SuspendLayout();
            // 
            // audioName
            // 
            audioName.AutoSize = true;
            audioName.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            audioName.Location = new Point(198, 9);
            audioName.Name = "audioName";
            audioName.Size = new Size(83, 32);
            audioName.TabIndex = 0;
            audioName.Text = "label1";
            // 
            // playButton
            // 
            playButton.Location = new Point(38, 95);
            playButton.Name = "playButton";
            playButton.Size = new Size(129, 23);
            playButton.TabIndex = 1;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = true;
            // 
            // stopButton
            // 
            stopButton.Location = new Point(173, 95);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(129, 23);
            stopButton.TabIndex = 2;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(308, 95);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(129, 23);
            deleteButton.TabIndex = 3;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            // 
            // AudioForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(489, 130);
            Controls.Add(deleteButton);
            Controls.Add(stopButton);
            Controls.Add(playButton);
            Controls.Add(audioName);
            Name = "AudioForm";
            Text = "Audio";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label audioName;
        private Button playButton;
        private Button stopButton;
        private Button deleteButton;
    }
}