using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsMaintenanceReport
    {
        public int MaintenanceReportId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? MaintenanceDate { get; set; }
        public int MaintenanceTypeId { get; set; }
        public int? CurrentStatus { get; set; }
        public string? MaintenanceReportDescription { get; set; }
        public string? CurrentStatusDescription { get; set; }
        public string? ServicemanCode { get; set; }
        public string? ServicemanInformation { get; set; }
        public int? InstallationPartId { get; set; }
        public int? InstallationId { get; set; }
    }
}
