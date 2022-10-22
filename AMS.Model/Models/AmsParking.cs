using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsParking
    {
        public int ParkingId { get; set; }
        public string Title { get; set; } = null!;
        public int UnitOwnerId { get; set; }
        public bool? IsKnocker { get; set; }
        public string? DescriptionParking { get; set; }
        public Guid? Guid { get; set; }
        public DateTime? LastModified { get; set; }
        public string? Location { get; set; }
        public bool? IsInOwnershipDocuments { get; set; }
        public int? UseByUnitId { get; set; }
        public int? RcbfloorId { get; set; }
    }
}
