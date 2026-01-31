using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Persistence.Security;

namespace Menedżer_notatek_i_rysunków.Persistence
{
    public class ZipExportService
    {   
        public void ExportJsonToZip(string jsonPath, string zipPath)
        {
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException("JSON file not found.", jsonPath);

            if (File.Exists(zipPath))
                File.Delete(zipPath);

            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(
                    jsonPath,
                    Path.GetFileName(jsonPath),
                    CompressionLevel.Optimal
                );
            }
        }

        public void ImportZip(string zipPath, string extractDirectory)
        {
            if (!File.Exists(zipPath))
                throw new FileNotFoundException("ZIP file not found.", zipPath);

            if (Directory.Exists(extractDirectory))
                Directory.Delete(extractDirectory, true);

            ZipFile.ExtractToDirectory(zipPath, extractDirectory);
        }

        public void ExportNotesToZip(IEnumerable<Note> notes, INoteFileService fileService, string jsonPath, string zipPath)
        {
            if (notes == null) throw new ArgumentNullException(nameof(notes));
            if (fileService == null) throw new ArgumentNullException(nameof(fileService));

            fileService.Save(jsonPath, notes);
            ExportJsonToZip(jsonPath, zipPath);
        }

        public void ExportNotesToEncryptedZip(IEnumerable<Note> notes, INoteFileService fileService, string jsonPath, string zipPath, string encPath, EncryptionService encryptionService, string password)
        {
            if (notes == null) throw new ArgumentNullException(nameof(notes));
            if (fileService == null) throw new ArgumentNullException(nameof(fileService));
            if (encryptionService == null) throw new ArgumentNullException(nameof(encryptionService));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password required for encrypted export.", nameof(password));

            fileService.Save(jsonPath, notes);
            ExportJsonToZip(jsonPath, zipPath);

            try
            {
                encryptionService.EncryptFile(zipPath, encPath, password);
            }
            finally
            {
                if (File.Exists(zipPath))
                {
                    try { File.Delete(zipPath); } catch { }
                }
            }
        }

        public List<Note> ImportNotesFromZip(string selectedPath, INoteFileService fileService, EncryptionService encryptionService, string? password = null)
        {
            if (string.IsNullOrWhiteSpace(selectedPath)) throw new ArgumentNullException(nameof(selectedPath));
            if (fileService == null) throw new ArgumentNullException(nameof(fileService));
            if (encryptionService == null) throw new ArgumentNullException(nameof(encryptionService));

            string workDir = Path.Combine(Path.GetTempPath(), "NaDM_Work_" + Guid.NewGuid());
            string extractDir = Path.Combine(workDir, "extract");

            Directory.CreateDirectory(workDir);
            Directory.CreateDirectory(extractDir);

            string zipToImport = selectedPath;
            string? tempZipPath = null;

            try
            {
                if (encryptionService.IsEncrypted(selectedPath))
                {
                    if (string.IsNullOrWhiteSpace(password))
                        throw new InvalidOperationException("Encrypted archive requires a password.");

                    tempZipPath = Path.Combine(workDir, Path.GetFileName(selectedPath).Replace(".enc", ""));
                    encryptionService.DecryptFile(selectedPath, tempZipPath, password);
                    zipToImport = tempZipPath;
                }

                ImportZip(zipToImport, extractDir);

                string importedJsonPath = Path.Combine(extractDir, "notes.json");
                if (!File.Exists(importedJsonPath))
                    throw new FileNotFoundException("notes.json not found inside archive.", importedJsonPath);

                var notes = fileService.Load(importedJsonPath);
                return notes;
            }
            finally
            {
                try
                {
                    if (Directory.Exists(workDir))
                        Directory.Delete(workDir, true);
                }
                catch { }
            }
        }
    }
}

