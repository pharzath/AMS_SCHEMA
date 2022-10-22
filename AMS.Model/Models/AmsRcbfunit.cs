using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsRcbfunit
    {
        public int RcbfunitId { get; set; }
        public string Title { get; set; } = null!;
        public string? Direction { get; set; }
        public string Space { get; set; } = null!;
        public string? TellNumber { get; set; }
        public string? UseTitle { get; set; }
        public string? Description { get; set; }
        public bool? IsComplex { get; set; }
        public string? ComplexUnitsFk { get; set; }
        public int? UnitNumber { get; set; }
        public int? RoomsCount { get; set; }
        public string? UseDescription { get; set; }
        public int FloorId { get; set; }
    }
}
