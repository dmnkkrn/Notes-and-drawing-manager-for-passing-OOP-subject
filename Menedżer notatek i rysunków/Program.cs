using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Persistence;
using Menedżer_notatek_i_rysunków.Persistence.Security;
using Menedżer_notatek_i_rysunków.Repositories;
using Menedżer_notatek_i_rysunków.Services;
using System.Diagnostics;

namespace Menedżer_notatek_i_rysunków
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            INoteFileService fileService = new NoteFileService();
            var notes = fileService.Load("notes.json");
            IZipExportService zipService = new ZipExportService();
            var encryptionService = new EncryptionService();

            var repo = new NoteRepository<Note>();
            foreach (var note in notes)
            {
                repo.Add(note);
            }

            var drawingService = new DrawingService(FileStrings.drawingsDir);

            Application.Run(new Form1(repo, fileService, zipService, encryptionService, drawingService));
        }
    }
}