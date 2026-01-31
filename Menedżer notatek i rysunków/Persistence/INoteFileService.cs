using System.Collections.Generic;
using System.Threading.Tasks;
using Menedżer_notatek_i_rysunków.Models;

namespace Menedżer_notatek_i_rysunków.Persistence
{
    public interface INoteFileService
    {
        void Save(string path, IEnumerable<Note> notes);
        List<Note> Load(string path);
    }
}
