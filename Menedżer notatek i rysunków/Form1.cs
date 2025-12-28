using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Persistence;
using Menedżer_notatek_i_rysunków.Repositories;
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
        public Form1(NoteRepository<Note> repository, NoteFileService fileService)
        {
            InitializeComponent();
            _repository = repository;
            _fileService = fileService;
            notesListBox.DisplayMember = "Title";
            RefreshNotesList();
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
            _fileService.Save("notes.json", _repository.GetAll());
        }


    }
}
