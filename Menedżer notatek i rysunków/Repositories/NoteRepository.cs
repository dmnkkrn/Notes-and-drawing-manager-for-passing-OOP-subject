using Menedżer_notatek_i_rysunków.Models;
using System.Collections.Generic;

namespace Menedżer_notatek_i_rysunków.Repositories
{
    public class NoteRepository<T> where T : Note
    {
        private readonly List<T> _items = new();

        public void Add(T item)
        {
            _items.Add(item);
        }

        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        public IReadOnlyList<T> GetAll()
        {
            return _items.AsReadOnly();
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void SortAscending()
        {
            _items.Sort((a, b) =>
            {
                if (a < b) return -1;
                if (a > b) return 1;
                return 0;
            });
        }

        public void SortDescending()
        {
            _items.Sort((a, b) =>
            {
                if (a > b) return -1;
                if (a < b) return 1;
                return 0;
            });
        }
    }
}
