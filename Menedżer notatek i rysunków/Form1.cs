using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Persistence;
using Menedżer_notatek_i_rysunków.Persistence.Security;
using Menedżer_notatek_i_rysunków.Repositories;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Menedżer_notatek_i_rysunków
{
    public partial class Form1 : Form
    {
        private NoteRepository<Note> _repository;
        private NoteFileService _fileService;
        private ZipExportService _zipService;
        private EncryptionService _encryptionService;

        private AutosaveService<Note> _autosaveService;

        string jsonPath = "notes.json";
        string restorePath = "notes_restore.json";
        string zipPath = "notes_export.zip";
        string encPath = "notes_export.zip.enc";
        string workBackupPath = "work_backup.json";
        string drawingsDir = "drawings";
        public Form1(NoteRepository<Note> repository, NoteFileService fileService,
            ZipExportService zipService, EncryptionService encryptionService)
        {
            InitializeComponent();
            _repository = repository;
            _fileService = fileService;
            _zipService = zipService;
            _encryptionService = encryptionService;


            this.FormClosing += Form1_FormClosing;

            notesListBox.DisplayMember = "Title";
            RefreshNotesList();

            _autosaveService = new AutosaveService<Note>(
            jsonPath,
            restorePath,
            workBackupPath,
            () =>
            {
                SaveWorkingNoteToBackup();
                return _repository.GetAll();
            },
            (path, data) => _fileService.Save(path, data),
            1000
            );

            _autosaveService.Start();

            restore();
        }
        private void RefreshNotesList()
        {
            notesListBox.Items.Clear();

            foreach (var note in _repository.GetAll())
            {
                notesListBox.Items.Add(note);
            }
        }
        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            onClose(e);
        }


        private void addButton_Click(object sender, EventArgs e)
        {
            askSaveAs();
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (notesListBox.SelectedItem is Note selectedNote)
            {
                var result = MessageBox.Show(
                    "Are you sure?",
                    "Confirmation",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    _repository.Remove(selectedNote);
                    RefreshNotesList();
                }
            }
        }

        private void notesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (notesListBox.SelectedItem is Note selectedNote)
            {
                noteTextBoxRich.Text = selectedNote.TextContent;
                ShowDrawingPreview(selectedNote);
            }
            else
            {
                noteTextBoxRich.Clear();
                pictureBoxPreview.Image = null;
            }
        }

        private void editButton_Click(object sender, EventArgs e) //Save button
        {
            applyEdit();
        }

        private void exportAsZipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _fileService.Save(jsonPath, _repository.GetAll());
                _zipService.ExportJsonToZip(jsonPath, zipPath);

                MessageBox.Show("Export completed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message);
            }
        }

        private void importZipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "ZIP files (*.zip;*.enc)|*.zip;*.enc"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string selectedPath = dialog.FileName;

            string workDir = Path.Combine(
                Path.GetTempPath(),
                "NaDM_Work_" + Guid.NewGuid()
            );

            string extractDir = Path.Combine(workDir, "extract");

            Directory.CreateDirectory(workDir);
            Directory.CreateDirectory(extractDir);

            string zipToImport;

            try
            {
                if (_encryptionService.IsEncrypted(selectedPath))
                {
                    string password = Interaction.InputBox(
                        "Password:",
                        "Encrypted import"
                    );

                    if (string.IsNullOrWhiteSpace(password))
                        return;

                    zipToImport = Path.Combine(
                        workDir,
                        Path.GetFileName(selectedPath).Replace(".enc", "")
                    );

                    _encryptionService.DecryptFile(
                        selectedPath,
                        zipToImport,
                        password
                    );
                }
                else
                {
                    zipToImport = selectedPath;
                }

                _zipService.ImportZip(zipToImport, extractDir);

                string importedJsonPath = Path.Combine(extractDir, "notes.json");
                if (!File.Exists(importedJsonPath))
                {
                    MessageBox.Show("notes.json not found.");
                    return;
                }

                var notes = _fileService.Load(importedJsonPath);

                _repository.Clear();
                foreach (var note in notes)
                {
                    _repository.Add(note);
                }

                RefreshNotesList();
                noteTextBoxRich.Clear();

                MessageBox.Show("Import completed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import failed: " + ex.Message);
            }
        }

        private void exportAsZipEncryptedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string password = Interaction.InputBox(
                "Password:",
                "Encrypt ZIP"
            );

            if (string.IsNullOrWhiteSpace(password))
                return;

            try
            {
                _fileService.Save(jsonPath, _repository.GetAll());
                _zipService.ExportJsonToZip(jsonPath, zipPath);

                _encryptionService.EncryptFile(
                    zipPath,
                    encPath,
                    password
                );

                MessageBox.Show("Encrypted export completed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message);
            }
            File.Delete(zipPath);
        }

        private void applyEdit()
        {
            if (notesListBox.SelectedItem is not Note selectedNote)
            {
                askSaveAs();
                return;
            }

            selectedNote.UpdateText(noteTextBoxRich.Text);
        }

        private void askSaveAs()
        {
            string title = Interaction.InputBox(
                "Title:",
                "Title"
            ).Trim();

            saveAs(title);
        }
        private void saveAs(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return;

            string content = noteTextBoxRich.Text.Trim();

            var note = new Note(title, content);

            _repository.Add(note);
            RefreshNotesList();

            notesListBox.SelectedItem = note;
            noteTextBoxRich.Clear();
        }

        private void restore()
        {
            if (File.Exists(restorePath) && !_autosaveService.HasUnsavedChanges())
                return;

            if (File.Exists(restorePath))
            {
                var result = MessageBox.Show(
                    "An autosaved session was found. Restore it?",
                    "Restore session",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    var restored = _fileService.Load(restorePath);
                    _repository.Clear();
                    foreach (var note in restored)
                        _repository.Add(note);

                    RefreshNotesList();
                }
            }

            if (File.Exists(workBackupPath))
            {
                var result = MessageBox.Show(
                    "A working note was found. Restore it?",
                    "Restore working note",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    var work = _fileService.Load(workBackupPath).FirstOrDefault();
                    if (work != null)
                        noteTextBoxRich.Text = work.TextContent;
                }
            }

        }
        private void SaveWorkingNoteToBackup()
        {
            string text = noteTextBoxRich.Text;

            if (string.IsNullOrWhiteSpace(text))
                return;

            if (notesListBox.SelectedItem is Note selectedNote)
            {
                if (text == selectedNote.TextContent)
                    return;
            }

            Note tempNote = null;

            if (File.Exists(workBackupPath))
            {
                var existing = _fileService.Load(workBackupPath);
                tempNote = existing.FirstOrDefault();
            }

            if (tempNote == null)
            {
                tempNote = new Note("[WORKING_NOTE]", text);
            }
            else
            {
                tempNote.UpdateText(text);
            }

            _fileService.Save(workBackupPath, new List<Note> { tempNote });
        }




        private void onClose(FormClosingEventArgs e)
        {
            askAboutWork(e);
            _fileService.Save(jsonPath, _repository.GetAll());

            if (File.Exists(restorePath))
                File.Delete(restorePath);

            _autosaveService.Dispose();
        }

        private void askAboutWork(FormClosingEventArgs e)
        {
            if (File.Exists(workBackupPath))
            {
                var result = MessageBox.Show(
                    "Do you want to save the working note?",
                    "Unsaved work",
                    MessageBoxButtons.YesNoCancel
                );
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    if (notesListBox.SelectedItem is not null)
                    {
                        applyEdit();
                    }
                    else
                    {
                        askSaveAs();
                    }
                }
                File.Delete(workBackupPath);
            }
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            if (notesListBox.SelectedItem is not Note selectedNote)
            {
                MessageBox.Show("Select a note first.");
                return;
            }

            if (!Directory.Exists(drawingsDir))
                Directory.CreateDirectory(drawingsDir);

            if (selectedNote.Drawing == null)
            {
                string path = Path.Combine(
                    drawingsDir,
                    $"{selectedNote.Id}.png"
                );

                selectedNote.AttachDrawing(new Drawing(path));
            }

            using var form = new FormDrawing(selectedNote.Drawing.ImagePath);
            form.ShowDialog();
            ShowDrawingPreview(selectedNote);
        }

        private void pictureBoxPreview_Click(object sender, EventArgs e)
        {

        }

        private void ShowDrawingPreview(Note note)
        {
            if (note.Drawing == null || string.IsNullOrWhiteSpace(note.Drawing.ImagePath))
            {
                pictureBoxPreview.Image = null;
                return;
            }

            if (!File.Exists(note.Drawing.ImagePath))
            {
                pictureBoxPreview.Image = null;
                return;
            }

            using (var fs = new FileStream(note.Drawing.ImagePath, FileMode.Open, FileAccess.Read))
            using (var temp = new Bitmap(fs))
            {
                pictureBoxPreview.Image?.Dispose();
                pictureBoxPreview.Image = new Bitmap(temp);
            }
        }

        private void buttonSortAsc_Click(object sender, EventArgs e)
        {
            _repository.SortAscending();
            RefreshNotesList();
        }

        private void buttonSortDesc_Click(object sender, EventArgs e)
        {
            _repository.SortDescending();
            RefreshNotesList();
        }
    }
}




