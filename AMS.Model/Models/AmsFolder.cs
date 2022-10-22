using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsFolder
    {
        public int FolderId { get; set; }
        public string Title { get; set; } = null!;
        public int? ParentFolderId { get; set; }
        public int? MaintenanceReportId { get; set; }
        public int? ResidentalComplexId { get; set; }
        public int? RcbfloorId { get; set; }
        public int? RcbfunitId { get; set; }
        public int? OwnershipId { get; set; }
        public int? ResidentationId { get; set; }
        public int? PersonelGroupId { get; set; }
    }
}
