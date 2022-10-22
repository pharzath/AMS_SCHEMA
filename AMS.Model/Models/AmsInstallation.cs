using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsInstallation
    {
        public int InstallationsId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? InstallationDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public int? CurrentStatus { get; set; }
        public DateTime? CurrentStatusDate { get; set; }
        public int? LastStatus { get; set; }
        public DateTime? LastStatusDate { get; set; }
        public string? CurrentStatusDescription { get; set; }
        public string? LastStatusDescription { get; set; }
        public string? LastServiceReport { get; set; }
        public int? YardId { get; set; }
        public int? ParentInstallationId { get; set; }
        public int? SharedAreaId { get; set; }
        public int? RoofId { get; set; }
    }
}
