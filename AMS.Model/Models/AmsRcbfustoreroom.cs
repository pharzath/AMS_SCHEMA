using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsRcbfustoreroom
    {
        public int RcbfustoreroomId { get; set; }
        public string? StSpace { get; set; }
        public string? StDescription { get; set; }
        public string? StFloorUnit { get; set; }
        public bool? StHasPrivatePower { get; set; }
        public string Title { get; set; } = null!;
        public Guid? Guid { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? StHasPublicPower { get; set; }
        public int UnitOwnerId { get; set; }
        public int UseByUnitId { get; set; }
        public int? FloorId { get; set; }
        public int? RoofId { get; set; }
    }
}
