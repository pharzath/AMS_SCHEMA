using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsRcblok
    {
        public int RcblokId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double? ConstructDate { get; set; }
        public Guid? Guid { get; set; }
    }
}
