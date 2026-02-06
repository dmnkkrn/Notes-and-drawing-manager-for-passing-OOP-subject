using Menedżer_notatek_i_rysunków.Models;
using System;

namespace Menedżer_notatek_i_rysunków.Services
{
    public interface IAudioService
    {
        void AttachAudio(Note note, string sourceFilePath);
        void DeleteAudioForNote(Guid noteId); 
        string GetAudioPathForNote(Guid noteId); 

        void EnsureDirectoryExists();
        bool HasEmbeddedAudio(Note note);
    }
}
