using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Persistence;
using Menedżer_notatek_i_rysunków.Persistence.Security;
using Menedżer_notatek_i_rysunków.Repositories;
using Menedżer_notatek_i_rysunków.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Menedżer_notatek_i_rysunków
{
    public partial class Form1 : Form
    {
        private NoteRepository<Note> _repository;
        private INoteFileService _fileService;
        private IZipExportService _zipService;
        private IEncryptionService _encryptionService;
        private DrawingService _drawingService;
        private IAudioService _audioService;

        private AutosaveService<Note> _autosaveService;

        string _jsonPath = FileStrings.jsonPath;
        string _restorePath = FileStrings.restorePath;
        string _zipPath = FileStrings.zipPath;
        string _encPath = FileStrings.encPath;
        string _workBackupPath = FileStrings.workBackupPath;
        string _drawingsDir = FileStrings.drawingsDir;
        string _audioDir = FileStrings.audioDir;

        public Form1(NoteRepository<Note> repository, INoteFileService fileService,
            IZipExportService zipService, IEncryptionService encryptionService,
            DrawingService drawingService, IAudioService audioService)
        {
            InitializeComponent();

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _zipService = zipService ?? throw new ArgumentNullException(nameof(zipService));
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            _drawingService = drawingService ?? throw new ArgumentNullException(nameof(drawingService));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));


            this.FormClosing += Form1_FormClosing;

            notesListBox.DisplayMember = "ListDisplay";
            RefreshNotesList();

            _autosaveService = new AutosaveService<Note>(
                _jsonPath,
                _restorePath,
                _workBackupPath,
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
                    _drawingService.DeleteDrawingForNote(selectedNote.Id);
                    RefreshNotesList();
                    pictureBoxPreview.Image = null;
                }
            }
        }

        private void notesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (notesListBox.SelectedItem is Note selectedNote)
            {
                noteTextBoxRich.Text = selectedNote.TextContent;
                ShowDrawingPreview(selectedNote);

                if (_audioService.HasEmbeddedAudio(notesListBox.SelectedItem as Note))
                {
                    isAudioLabel.Text = "♪";
                }
                else
                {
                    isAudioLabel.Text = "";
                }
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
                _drawingService.EnsureDirectoryExists();
                _audioService.EnsureDirectoryExists();

                _zipService.ExportNotesToZip(_repository.GetAll(), _fileService, _jsonPath, _zipPath);
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

            try
            {
                string? password = null;
                if (_encryptionService.IsEncrypted(selectedPath))
                {
                    password = Interaction.InputBox(
                        "Password:",
                        "Encrypted import"
                    );

                    if (string.IsNullOrWhiteSpace(password))
                        return;
                }

                var mergedNotes = _zipService.ImportAndMergeNotesFromZip(selectedPath, _repository.GetAll(), _fileService, _encryptionService, password);

                _repository.Clear();
                foreach (var note in mergedNotes)
                    _repository.Add(note);

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
                _drawingService.EnsureDirectoryExists();
                _audioService.EnsureDirectoryExists();

                _zipService.ExportNotesToEncryptedZip(_repository.GetAll(), _fileService, _jsonPath, _zipPath, _encPath, _encryptionService, password);
                MessageBox.Show("Encrypted export completed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message);
            }
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
                "New note"
            );

            if (string.IsNullOrWhiteSpace(title)) return;

            var note = new Note(title, "");
            _repository.Add(note);
            RefreshNotesList();
        }

        private void restore()
        {
            if (File.Exists(_restorePath) && !_autosaveService.HasUnsavedChanges())
                return;

            if (File.Exists(_restorePath))
            {
                var result = MessageBox.Show(
                    "An autosaved session was found. Restore it?",
                    "Restore session",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    var restored = _fileService.Load(_restorePath);
                    _repository.Clear();
                    foreach (var note in restored)
                        _repository.Add(note);

                    RefreshNotesList();
                }
            }

            if (File.Exists(_workBackupPath))
            {
                var result = MessageBox.Show(
                    "A working note was found. Restore it?",
                    "Restore working note",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    var work = _fileService.Load(_workBackupPath).FirstOrDefault();
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

            if (File.Exists(_workBackupPath))
            {
                var existing = _fileService.Load(_workBackupPath);
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

            _fileService.Save(_workBackupPath, new List<Note> { tempNote });
        }

        private void onClose(FormClosingEventArgs e)
        {
            askAboutWork(e);
            _fileService.Save(_jsonPath, _repository.GetAll());

            if (File.Exists(_restorePath))
                File.Delete(_restorePath);

            _autosaveService.Dispose();
        }

        private void askAboutWork(FormClosingEventArgs e)
        {
            if (File.Exists(_workBackupPath))
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
                File.Delete(_workBackupPath);
            }
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            if (notesListBox.SelectedItem is not Note selectedNote)
            {
                MessageBox.Show("Select a note first.");
                return;
            }

            _drawingService.EnsureDirectoryExists();

            if (selectedNote.Drawing == null)
            {
                string path = _drawingService.GetDrawingPathForNote(selectedNote.Id);
                selectedNote.AttachDrawing(new Drawing(path));
            }

            using var form = new FormDrawing(selectedNote.Drawing.ImagePath, _drawingService);
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

            var bmp = _drawingService.LoadBitmapCopy(note.Drawing.ImagePath);
            if (bmp == null)
            {
                pictureBoxPreview.Image = null;
                return;
            }

            pictureBoxPreview.Image?.Dispose();
            pictureBoxPreview.Image = bmp;
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void audioEmbed_Click(object sender, EventArgs e)
        {
            if (notesListBox.SelectedItem is not Note selectedNote)
            {
                MessageBox.Show("Select a note first.");
                return;
            }

            using var dialog = new OpenFileDialog
            {
                Filter = "WAV files (*.wav)|*.wav",
                Title = "Select WAV file to attach"
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var selectedFile = dialog.FileName;

            try
            {
                _audioService.EnsureDirectoryExists();
                _audioService.AttachAudio(selectedNote, selectedFile);

                MessageBox.Show("Audio attached.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to attach audio: " + ex.Message);
            }
        }

        private void audioProperties_Click(object sender, EventArgs e)
        {
            if (notesListBox.SelectedItem is not Note selectedNote)
            {
                MessageBox.Show("Select a note first.");
                return;
            }

            if (!_audioService.HasEmbeddedAudio(selectedNote))
            {
                MessageBox.Show("No audio attached.");
                return;
            }

            using var form = new AudioForm(selectedNote, _audioService);
            form.ShowDialog();
        }
    }
}


