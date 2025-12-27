using System;
using System.Collections.Generic;
using System.Text;

namespace Menedżer_notatek_i_rysunków.Models
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
