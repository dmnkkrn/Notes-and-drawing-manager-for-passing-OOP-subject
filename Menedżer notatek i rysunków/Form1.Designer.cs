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
            buttonSortAsc = new Button();
            buttonSortDesc = new Button();
            audioEmbed = new Button();
            audioProperties = new Button();
            exportImportToolStripMenuItem1 = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).BeginInit();
            SuspendLayout();
            // 
            // notesListBox
            // 
            notesListBox.FormattingEnabled = true;
            notesListBox.Location = new Point(503, 50);
            notesListBox.Margin = new Padding(3, 2, 3, 2);
            notesListBox.Name = "notesListBox";
            notesListBox.Size = new Size(188, 184);
            notesListBox.TabIndex = 0;
            notesListBox.SelectedIndexChanged += notesListBox_SelectedIndexChanged;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(329, 309);
            deleteButton.Margin = new Padding(3, 2, 3, 2);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(150, 22);
            deleteButton.TabIndex = 1;
            deleteButton.Text = "Delete";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // addButton
            // 
            addButton.Location = new Point(18, 309);
            addButton.Margin = new Padding(3, 2, 3, 2);
            addButton.Name = "addButton";
            addButton.Size = new Size(150, 22);
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
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(700, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // exportImportToolStripMenuItem
            // 
            exportImportToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exportAsZipToolStripMenuItem, exportAsZipEncryptedToolStripMenuItem, importZipToolStripMenuItem, exportImportToolStripMenuItem1 });
            exportImportToolStripMenuItem.Name = "exportImportToolStripMenuItem";
            exportImportToolStripMenuItem.Size = new Size(100, 20);
            exportImportToolStripMenuItem.Text = "Export / Import";
            // 
            // exportAsZipToolStripMenuItem
            // 
            exportAsZipToolStripMenuItem.Name = "exportAsZipToolStripMenuItem";
            exportAsZipToolStripMenuItem.Size = new Size(206, 22);
            exportAsZipToolStripMenuItem.Text = "Export as Zip";
            exportAsZipToolStripMenuItem.Click += exportAsZipToolStripMenuItem_Click;
            // 
            // exportAsZipEncryptedToolStripMenuItem
            // 
            exportAsZipEncryptedToolStripMenuItem.Name = "exportAsZipEncryptedToolStripMenuItem";
            exportAsZipEncryptedToolStripMenuItem.Size = new Size(206, 22);
            exportAsZipEncryptedToolStripMenuItem.Text = "Export as Zip (Encrypted)";
            exportAsZipEncryptedToolStripMenuItem.Click += exportAsZipEncryptedToolStripMenuItem_Click;
            // 
            // importZipToolStripMenuItem
            // 
            importZipToolStripMenuItem.Name = "importZipToolStripMenuItem";
            importZipToolStripMenuItem.Size = new Size(206, 22);
            importZipToolStripMenuItem.Text = "Import Zip";
            importZipToolStripMenuItem.Click += importZipToolStripMenuItem_Click;
            // 
            // noteTextBoxRich
            // 
            noteTextBoxRich.Location = new Point(10, 23);
            noteTextBoxRich.Margin = new Padding(3, 2, 3, 2);
            noteTextBoxRich.Name = "noteTextBoxRich";
            noteTextBoxRich.Size = new Size(487, 280);
            noteTextBoxRich.TabIndex = 4;
            noteTextBoxRich.Text = "";
            // 
            // editButton
            // 
            editButton.Location = new Point(174, 309);
            editButton.Margin = new Padding(3, 2, 3, 2);
            editButton.Name = "editButton";
            editButton.Size = new Size(150, 22);
            editButton.TabIndex = 5;
            editButton.Text = "Save";
            editButton.UseVisualStyleBackColor = true;
            editButton.Click += editButton_Click;
            // 
            // drawButton
            // 
            drawButton.Location = new Point(18, 334);
            drawButton.Margin = new Padding(3, 2, 3, 2);
            drawButton.Name = "drawButton";
            drawButton.Size = new Size(150, 22);
            drawButton.TabIndex = 6;
            drawButton.Text = "Draw";
            drawButton.UseVisualStyleBackColor = true;
            drawButton.Click += drawButton_Click;
            // 
            // pictureBoxPreview
            // 
            pictureBoxPreview.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxPreview.Location = new Point(503, 258);
            pictureBoxPreview.Margin = new Padding(3, 2, 3, 2);
            pictureBoxPreview.Name = "pictureBoxPreview";
            pictureBoxPreview.Size = new Size(187, 98);
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPreview.TabIndex = 7;
            pictureBoxPreview.TabStop = false;
            pictureBoxPreview.Click += pictureBoxPreview_Click;
            // 
            // buttonSortAsc
            // 
            buttonSortAsc.Location = new Point(503, 23);
            buttonSortAsc.Margin = new Padding(3, 2, 3, 2);
            buttonSortAsc.Name = "buttonSortAsc";
            buttonSortAsc.Size = new Size(45, 22);
            buttonSortAsc.TabIndex = 8;
            buttonSortAsc.Text = "Asc";
            buttonSortAsc.UseVisualStyleBackColor = true;
            buttonSortAsc.Click += buttonSortAsc_Click;
            // 
            // buttonSortDesc
            // 
            buttonSortDesc.Location = new Point(553, 23);
            buttonSortDesc.Margin = new Padding(3, 2, 3, 2);
            buttonSortDesc.Name = "buttonSortDesc";
            buttonSortDesc.Size = new Size(45, 22);
            buttonSortDesc.TabIndex = 9;
            buttonSortDesc.Text = "Desc";
            buttonSortDesc.UseVisualStyleBackColor = true;
            buttonSortDesc.Click += buttonSortDesc_Click;
            // 
            // audioEmbed
            // 
            audioEmbed.Location = new Point(174, 333);
            audioEmbed.Name = "audioEmbed";
            audioEmbed.Size = new Size(150, 23);
            audioEmbed.TabIndex = 10;
            audioEmbed.Text = "Embed Audio";
            audioEmbed.UseVisualStyleBackColor = true;
            audioEmbed.Click += audioEmbed_Click;
            // 
            // audioProperties
            // 
            audioProperties.Location = new Point(329, 333);
            audioProperties.Name = "audioProperties";
            audioProperties.Size = new Size(150, 23);
            audioProperties.TabIndex = 11;
            audioProperties.Text = "Audio";
            audioProperties.UseVisualStyleBackColor = true;
            audioProperties.Click += audioProperties_Click;
            // 
            // exportImportToolStripMenuItem1
            // 
            exportImportToolStripMenuItem1.Name = "exportImportToolStripMenuItem1";
            exportImportToolStripMenuItem1.Size = new Size(206, 22);
            exportImportToolStripMenuItem1.Text = "Export / Import";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 363);
            Controls.Add(audioProperties);
            Controls.Add(audioEmbed);
            Controls.Add(buttonSortDesc);
            Controls.Add(buttonSortAsc);
            Controls.Add(pictureBoxPreview);
            Controls.Add(drawButton);
            Controls.Add(editButton);
            Controls.Add(noteTextBoxRich);
            Controls.Add(addButton);
            Controls.Add(deleteButton);
            Controls.Add(notesListBox);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Save";
            Load += Form1_Load;
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
        private Button buttonSortAsc;
        private Button buttonSortDesc;
        private Button audioEmbed;
        private Button audioProperties;
        private ToolStripMenuItem exportImportToolStripMenuItem1;
    }
}