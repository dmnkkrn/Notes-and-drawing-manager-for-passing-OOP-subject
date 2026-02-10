using Menedżer_notatek_i_rysunków.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Services
{
    public class AudioService : IAudioService  //drawing service and audio service would inherit from the same class, maybe in future
    {
        private readonly string _audioDir;

        public AudioService(string audioDir)
        {
            _audioDir = audioDir;
        }

        public void EnsureDirectoryExists()
        {
            if (string.IsNullOrWhiteSpace(_audioDir))
                return;
            Directory.CreateDirectory(_audioDir);
        }
        public void AttachAudio(Note note, string sourceFilePath)
        {
            if (note is null)
                throw new ArgumentNullException(nameof(note));

            if (string.IsNullOrWhiteSpace(sourceFilePath))
                throw new ArgumentException("Source file path cannot be empty.", nameof(sourceFilePath));

            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("Selected audio file does not exist.", sourceFilePath);

            var ext = Path.GetExtension(sourceFilePath);
            if (string.IsNullOrWhiteSpace(ext) || !ext.Equals(".wav", StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("Only WAV files are supported (.wav).");

            var fileName = $"{note.Id}{ext}";
            var relativeTargetPath = Path.Combine(_audioDir ?? string.Empty, fileName);

            string absoluteTargetPath = Path.IsPathRooted(relativeTargetPath)
                ? relativeTargetPath
                : Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeTargetPath));

            var targetDir = Path.GetDirectoryName(absoluteTargetPath);
            if (!string.IsNullOrEmpty(targetDir))
                Directory.CreateDirectory(targetDir);

            File.Copy(sourceFilePath, absoluteTargetPath, overwrite: true);

            var storedPath = Path.IsPathRooted(relativeTargetPath) ? absoluteTargetPath : relativeTargetPath;

            if (note.Audio == null)
            {
                note.AttachAudio(new AudioNote(storedPath));
            }
            else
            {
                note.Audio.FilePath = storedPath;
            }
        }

        private void DeleteAudio(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                throw new Exception("Path to audio cannot be blank or must exist");
                return;
            }
            File.Delete(path);
        }

        public void DeleteAudioForNote(Note note)
        {
            if (note is null)
                throw new ArgumentNullException(nameof(note));

            var path = GetAudioPathForNote(note.Id);
            DeleteAudio(path);
            note.Audio = null;
        }

        public string GetAudioPathForNote(System.Guid noteId)
        {
            return Path.Combine(_audioDir, $"{noteId}.wav");
        }

        public bool HasEmbeddedAudio(Note note) => note.Audio != null;
    }
}
