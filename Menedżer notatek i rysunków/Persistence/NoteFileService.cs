using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Menedżer_notatek_i_rysunków.Models;

namespace Menedżer_notatek_i_rysunków.Persistence
{
    public class NoteFileService : INoteFileService
    {
        private readonly JsonSerializerOptions _options =
            new JsonSerializerOptions
            {
                WriteIndented = true
            };

        public void Save(string path, IEnumerable<Note> notes)
        {
            string json = JsonSerializer.Serialize(notes, _options);
            File.WriteAllText(path, json);
        }

        public List<Note> Load(string path)
        {
            if (!File.Exists(path))
                return new List<Note>();

            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
                return new List<Note>();

            try
            {
                return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
            }
            catch (System.Text.Json.JsonException)
            {
                return new List<Note>();
            }
        }
    }
}
