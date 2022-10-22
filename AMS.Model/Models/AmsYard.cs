using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsYard
    {
        public int YardId { get; set; }
        public string Title { get; set; } = null!;
        public int? ParkingStatus { get; set; }
        public double? GardenArea { get; set; }
        public double? YardArea { get; set; }
        public bool? IsConectedToRamp { get; set; }
        public string? Description { get; set; }
        public int BlockId { get; set; }
    }
}
