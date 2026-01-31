using System.Security.Principal;

namespace Menedżer_notatek_i_rysunków.Models
{
    public class Note : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public DateTime CreatedAt { get; set; }

        public Drawing Drawing { get; set; }
        public AudioNote Audio { get; set; }

        public Note()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        public Note(string title, string textContent)
        {
            Id = Guid.NewGuid();
            Title = title;
            TextContent = textContent;
            CreatedAt = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            TextContent = newText;
        }

        public void AttachDrawing(Drawing drawing)
        {
            Drawing = drawing;
        }

        public void AttachAudio(AudioNote audio)
        {
            Audio = audio;
        }

        public string ListDisplay => $"{Title} [{CreatedAt:yyyy-MM-dd HH:mm}]";

        public static bool operator ==(Note a, Note b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Id == b.Id;
        }

        public static bool operator !=(Note a, Note b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is Note note && Id == note.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator <(Note a, Note b)
        {
            return a.CreatedAt < b.CreatedAt;
        }

        public static bool operator >(Note a, Note b)
        {
            return a.CreatedAt > b.CreatedAt;
        }
    }
}
