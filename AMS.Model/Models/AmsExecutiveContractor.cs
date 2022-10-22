using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsExecutiveContractor
    {
        public int ExecutiveContractorId { get; set; }
        public string Title { get; set; } = null!;
        public int PersonelGroupId { get; set; }
    }
}
