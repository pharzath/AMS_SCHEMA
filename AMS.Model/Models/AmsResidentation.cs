using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsResidentation
    {
        public int Rstid { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? PlanToStay { get; set; }
        public int? TotalResidentCount { get; set; }
        public int? TotalChildCount { get; set; }
        public bool? HasBaby { get; set; }
        public DateTime? LastModified { get; set; }
        public int ResidentaionType { get; set; }
        public string ResidentationTitle { get; set; } = null!;
        public string? Description { get; set; }
        public int UnitId { get; set; }
        public int? ResidentId { get; set; }
    }
}
