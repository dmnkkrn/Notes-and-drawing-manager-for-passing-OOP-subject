using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Persistence.Security;

namespace Menedżer_notatek_i_rysunków.Persistence
{
    public class ZipExportService : IZipExportService
    {
        public void ExportJsonToZip(string jsonPath, string zipPath)
        {
            EnsureFileExists(jsonPath);
            DeleteIfExists(zipPath);
            CreateZipFromFile(jsonPath, zipPath);
        }

        public void ImportZip(string zipPath, string extractDirectory)
        {
            EnsureFileExists(zipPath);
            PrepareDirectoryClean(extractDirectory);
            ZipFile.ExtractToDirectory(zipPath, extractDirectory);
        }

        public void ExportNotesToZip(IEnumerable<Note> notes, INoteFileService fileService, string jsonPath, string zipPath)
        {
            ValidateNotNull(notes);
            ValidateNotNull(fileService);
            SaveNotesAndCreateZip(notes, fileService, jsonPath, zipPath);
        }

        public void ExportNotesToEncryptedZip(IEnumerable<Note> notes, INoteFileService fileService, string jsonPath, string zipPath, string encPath, IEncryptionService encryptionService, string password)
        {
            ValidateNotNull(notes);
            ValidateNotNull(fileService);
            ValidateNotNull(encryptionService);
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password required for encrypted export.", nameof(password));
            SaveNotesAndCreateZip(notes, fileService, jsonPath, zipPath);
            try
            {
                encryptionService.EncryptFile(zipPath, encPath, password);
            }
            finally
            {
                SafeDeleteFile(zipPath);
            }
        }

        public List<Note> ImportNotesFromZip(string selectedPath, INoteFileService fileService, IEncryptionService encryptionService, string? password = null)
        {
            if (string.IsNullOrWhiteSpace(selectedPath)) throw new ArgumentNullException(nameof(selectedPath));
            ValidateNotNull(fileService);
            ValidateNotNull(encryptionService);
            EnsureFileExists(selectedPath);

            var workDir = Path.Combine(Path.GetTempPath(), "NaDM_Work_" + Guid.NewGuid());
            var extractDir = Path.Combine(workDir, "extract");
            Directory.CreateDirectory(workDir);
            Directory.CreateDirectory(extractDir);

            var zipToImport = selectedPath;
            string? tempZipPath = null;

            try
            {
                if (encryptionService.IsEncrypted(selectedPath))
                {
                    if (string.IsNullOrWhiteSpace(password)) throw new InvalidOperationException("Encrypted archive requires a password.");
                    tempZipPath = Path.Combine(workDir, Path.GetFileName(selectedPath).Replace(".enc", ""));
                    encryptionService.DecryptFile(selectedPath, tempZipPath, password);
                    zipToImport = tempZipPath;
                }

                ImportZip(zipToImport, extractDir);

                var appBase = AppDomain.CurrentDomain.BaseDirectory;
                var extractedDrawings = Path.Combine(extractDir, FileStrings.drawingsDir);
                var extractedAudio = Path.Combine(extractDir, FileStrings.audioDir);
                var targetDrawings = Path.Combine(appBase, FileStrings.drawingsDir);
                var targetAudio = Path.Combine(appBase, FileStrings.audioDir);

                CopyDirectoryContents(extractedDrawings, targetDrawings);
                CopyDirectoryContents(extractedAudio, targetAudio);

                var importedJsonPath = Path.Combine(extractDir, FileStrings.NotesFileName);
                if (!File.Exists(importedJsonPath)) throw new FileNotFoundException("notes.json not found inside archive.", importedJsonPath);

                var importedNotes = fileService.Load(importedJsonPath);
                var existingNotes = fileService.Load(FileStrings.jsonPath);

                foreach (var imp in importedNotes)
                {
                    if (!existingNotes.Any(e => e == imp))
                        existingNotes.Add(imp);
                }

                fileService.Save(FileStrings.jsonPath, existingNotes);
                return existingNotes;
            }
            finally
            {
                try
                {
                    if (Directory.Exists(workDir)) Directory.Delete(workDir, true);
                }
                catch { }
            }
        }

        private static void ValidateNotNull(object? obj)
        {
            if (obj == null) throw new ArgumentNullException();
        }

        private static void EnsureFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) throw new FileNotFoundException("File not found.", path);
        }

        private static void DeleteIfExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            try { if (File.Exists(path)) File.Delete(path); } catch { }
        }

        private static void SafeDeleteFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            try { if (File.Exists(path)) File.Delete(path); } catch { }
        }

        private static void PrepareDirectoryClean(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir)) return;
            try
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
                Directory.CreateDirectory(dir);
            }
            catch { }
        }

        private static void CreateZipFromFile(string sourceFilePath, string zipPath)
        {
            DeleteIfExists(zipPath);
            var baseDir = Path.GetDirectoryName(sourceFilePath) ?? Directory.GetCurrentDirectory();
            var appBase = AppDomain.CurrentDomain.BaseDirectory;

            var candidateDirs = new[]
            {
                Path.Combine(baseDir, FileStrings.drawingsDir),
                Path.Combine(baseDir, FileStrings.audioDir),
                Path.Combine(appBase, FileStrings.drawingsDir),
                Path.Combine(appBase, FileStrings.audioDir)
            }
            .Select(Path.GetFullPath)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

            using var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            zip.CreateEntryFromFile(sourceFilePath, Path.GetFileName(sourceFilePath), CompressionLevel.Optimal);

            foreach (var dir in candidateDirs)
            {
                if (Directory.Exists(dir)) AddDirectoryToZip(zip, dir);
            }
        }

        private static void AddDirectoryToZip(ZipArchive zip, string directoryPath)
        {
            var topFolderName = Path.GetFileName(directoryPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            foreach (var file in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                var rel = Path.GetRelativePath(directoryPath, file);
                var entryPath = Path.Combine(topFolderName, rel).Replace(Path.DirectorySeparatorChar, '/').Replace(Path.AltDirectorySeparatorChar, '/');
                zip.CreateEntryFromFile(file, entryPath, CompressionLevel.Optimal);
            }
        }

        private static void SaveNotesAndCreateZip(IEnumerable<Note> notes, INoteFileService fileService, string jsonPath, string zipPath)
        {
            fileService.Save(jsonPath, notes);
            CreateZipFromFile(jsonPath, zipPath);
        }

        private static void CopyDirectoryContents(string sourceDir, string destDir)
        {
            if (string.IsNullOrWhiteSpace(sourceDir) || !Directory.Exists(sourceDir)) return;
            if (string.IsNullOrWhiteSpace(destDir)) return;
            Directory.CreateDirectory(destDir);

            foreach (var sourceFile in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                var rel = Path.GetRelativePath(sourceDir, sourceFile);
                var destFile = Path.Combine(destDir, rel);
                var destFolder = Path.GetDirectoryName(destFile);
                if (!string.IsNullOrEmpty(destFolder)) Directory.CreateDirectory(destFolder);
                File.Copy(sourceFile, destFile, overwrite: true);
            }
        }
    }
}

