////using System;
using System.Collections.Generic;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class NoteRepository<T>
    {
        private readonly List<T> _items;

        public NoteRepository()
        {
            _items = new List<T>();
        }

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
    }

}
