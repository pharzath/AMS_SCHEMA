using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsVehicle
    {
        public int VehicleId { get; set; }
        public string? Type { get; set; }
        public string? Color { get; set; }
        public string Brand { get; set; } = null!;
        public string? Pelak { get; set; }
    }
}
