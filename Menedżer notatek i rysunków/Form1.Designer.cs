namespace Menedżer_notatek_i_rysunków
{
    partial class Form1
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
            notesListBox = new ListBox();
            deleteButton = new Button();
            addButton = new Button();
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            exportImportToolStripMenuItem = new ToolStripMenuItem();
            exportAsZipToolStripMenuItem = new ToolStripMenuItem();
            exportAsZipEncryptedToolStripMenuItem = new ToolStripMenuItem();
            importZipToolStripMenuItem = new ToolStripMenuItem();
            noteTextBoxRich = new RichTextBox();
            editButton = new Button();
            drawButton = new Button();
            pictureBoxPreview = new PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).BeginInit();
            SuspendLayout();
            // 
            // notesListBox
            // 
            notesListBox.FormattingEnabled = true;
            notesListBox.Location = new Point(574, 14);
            notesListBox.Name = "notesListBox";
            notesListBox.Size = new Size(214, 324);
            notesListBox.TabIndex = 0;
            notesListBox.SelectedIndexChanged += notesListBox_SelectedIndexChanged;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(396, 409);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(172, 29);
            deleteButton.TabIndex = 1;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // addButton
            // 
            addButton.Location = new Point(40, 409);
            addButton.Name = "addButton";
            addButton.Size = new Size(172, 29);
            addButton.TabIndex = 2;
            addButton.Text = "Save As";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, exportImportToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(76, 24);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // exportImportToolStripMenuItem
            // 
            exportImportToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exportAsZipToolStripMenuItem, exportAsZipEncryptedToolStripMenuItem, importZipToolStripMenuItem });
            exportImportToolStripMenuItem.Name = "exportImportToolStripMenuItem";
            exportImportToolStripMenuItem.Size = new Size(125, 24);
            exportImportToolStripMenuItem.Text = "Export / Import";
            // 
            // exportAsZipToolStripMenuItem
            // 
            exportAsZipToolStripMenuItem.Name = "exportAsZipToolStripMenuItem";
            exportAsZipToolStripMenuItem.Size = new Size(259, 26);
            exportAsZipToolStripMenuItem.Text = "Export as Zip";
            exportAsZipToolStripMenuItem.Click += exportAsZipToolStripMenuItem_Click;
            // 
            // exportAsZipEncryptedToolStripMenuItem
            // 
            exportAsZipEncryptedToolStripMenuItem.Name = "exportAsZipEncryptedToolStripMenuItem";
            exportAsZipEncryptedToolStripMenuItem.Size = new Size(259, 26);
            exportAsZipEncryptedToolStripMenuItem.Text = "Export as Zip (Encrypted)";
            exportAsZipEncryptedToolStripMenuItem.Click += exportAsZipEncryptedToolStripMenuItem_Click;
            // 
            // importZipToolStripMenuItem
            // 
            importZipToolStripMenuItem.Name = "importZipToolStripMenuItem";
            importZipToolStripMenuItem.Size = new Size(259, 26);
            importZipToolStripMenuItem.Text = "Import Zip";
            importZipToolStripMenuItem.Click += importZipToolStripMenuItem_Click;
            // 
            // noteTextBoxRich
            // 
            noteTextBoxRich.Location = new Point(12, 31);
            noteTextBoxRich.Name = "noteTextBoxRich";
            noteTextBoxRich.Size = new Size(556, 372);
            noteTextBoxRich.TabIndex = 4;
            noteTextBoxRich.Text = "";
            // 
            // editButton
            // 
            editButton.Location = new Point(218, 409);
            editButton.Name = "editButton";
            editButton.Size = new Size(172, 29);
            editButton.TabIndex = 5;
            editButton.Text = "Save";
            editButton.UseVisualStyleBackColor = true;
            editButton.Click += editButton_Click;
            // 
            // drawButton
            // 
            drawButton.Location = new Point(40, 443);
            drawButton.Name = "drawButton";
            drawButton.Size = new Size(172, 29);
            drawButton.TabIndex = 6;
            drawButton.Text = "Draw";
            drawButton.UseVisualStyleBackColor = true;
            drawButton.Click += drawButton_Click;
            // 
            // pictureBoxPreview
            // 
            pictureBoxPreview.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxPreview.Location = new Point(575, 344);
            pictureBoxPreview.Name = "pictureBoxPreview";
            pictureBoxPreview.Size = new Size(213, 130);
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPreview.TabIndex = 7;
            pictureBoxPreview.TabStop = false;
            pictureBoxPreview.Click += pictureBoxPreview_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 484);
            Controls.Add(pictureBoxPreview);
            Controls.Add(drawButton);
            Controls.Add(editButton);
            Controls.Add(noteTextBoxRich);
            Controls.Add(addButton);
            Controls.Add(deleteButton);
            Controls.Add(notesListBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Save";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox notesListBox;
        private Button deleteButton;
        private Button addButton;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private RichTextBox noteTextBoxRich;
        private Button editButton;
        private ToolStripMenuItem exportImportToolStripMenuItem;
        private ToolStripMenuItem exportAsZipToolStripMenuItem;
        private ToolStripMenuItem importZipToolStripMenuItem;
        private ToolStripMenuItem exportAsZipEncryptedToolStripMenuItem;
        private Button drawButton;
        private PictureBox pictureBoxPreview;
    }
}