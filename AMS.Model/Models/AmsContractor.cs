using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsContractor
    {
        public int ContractorId { get; set; }
        public string? ContractorType { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ExecutiveContractorId { get; set; }
    }
}
