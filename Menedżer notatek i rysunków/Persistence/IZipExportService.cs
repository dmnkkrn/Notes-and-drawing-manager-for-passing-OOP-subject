using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Persistence.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Persistence
{
    public interface IZipExportService
    {
        void ExportJsonToZip(string jsonPath, string zipPath);
        void ImportZip(string zipPath, string extractDirectory);
        void ExportNotesToZip(IEnumerable<Models.Note> notes, INoteFileService fileService, string jsonPath, string zipPath);
        void ExportNotesToEncryptedZip(IEnumerable<Models.Note> notes, INoteFileService fileService, string jsonPath, string zipPath, string encPath, IEncryptionService encryptionService, string password);

        List<Note> ImportNotesFromZip(string selectedPath, INoteFileService fileService, IEncryptionService encryptionService, string? password = null);
        List<Note> ImportAndMergeNotesFromZip(string selectedPath, IEnumerable<Note> existingNotes, INoteFileService fileService, IEncryptionService encryptionService, string? password = null);
    }
}
