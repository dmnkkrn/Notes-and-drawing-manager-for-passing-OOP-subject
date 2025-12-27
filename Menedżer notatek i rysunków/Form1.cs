using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Persistence;
using Menedżer_notatek_i_rysunków.Repositories;

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
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _fileService.Save("notes.json", _repository.GetAll());
        }
    }
}
