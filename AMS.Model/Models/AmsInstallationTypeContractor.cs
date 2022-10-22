using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsInstallationTypeContractor
    {
        public int ItemId { get; set; }
        public int InstallationTypeId { get; set; }
        public int ContractorId { get; set; }
        public int? ViewOrder { get; set; }
    }
}
