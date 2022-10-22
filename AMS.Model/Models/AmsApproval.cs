using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsApproval
    {
        public int ApprovalsId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DateApproval { get; set; }
        public int SessionAgendaId { get; set; }
    }
}
