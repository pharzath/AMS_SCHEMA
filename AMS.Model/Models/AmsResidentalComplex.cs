using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsResidentalComplex
    {
        public int ResidentalComplexId { get; set; }
        public string Name { get; set; } = null!;
        public int? SharedAreaId { get; set; }
        public int? RcclassId { get; set; }
        public string? Status { get; set; }
    }
}
