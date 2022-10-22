using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsOwnership
    {
        public int OwnershipId { get; set; }
        public string OwnershipTitle { get; set; } = null!;
        public int UnitId { get; set; }
        public DateTime? OwningDate { get; set; }
        public string? Description { get; set; }
    }
}
