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
            () => _repository.GetAll(),
            (path, data) => _fileService.Save(path, data),15000);

            _autosaveService.Start();

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
        }
        private void RefreshNotesList()
        {
            notesListBox.Items.Clear();

            foreach (var note in _repository.GetAll())
            {
                notesListBox.Items.Add(note);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _fileService.Save(jsonPath, _repository.GetAll());
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string title = Interaction.InputBox(
                "Title:",
                "Title"
            ).Trim();

            if (string.IsNullOrWhiteSpace(title))
                return;

            string content = noteTextBoxRich.Text.Trim();

            var note = new Note(title, content);

            _repository.Add(note);
            RefreshNotesList();

            notesListBox.SelectedItem = note;
            noteTextBoxRich.Clear();
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
            }
            else
            {
                noteTextBoxRich.Clear();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (notesListBox.SelectedItem is not Note selectedNote)
            {
                MessageBox.Show("Select a note first.");
                return;
            }

            selectedNote.UpdateText(noteTextBoxRich.Text);
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
    }
}
