using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsSessionAgendum
    {
        public int SessionAgendaId { get; set; }
        public string Title { get; set; } = null!;
        public int DirectorsSessionId { get; set; }
    }
}
