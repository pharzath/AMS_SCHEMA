using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsRcbfloor
    {
        public int RcbfloorId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? LastModified { get; set; }
        public decimal? Number { get; set; }
        public bool? HasParking { get; set; }
        public bool? HasStore { get; set; }
        public bool? IsCommonFloor { get; set; }
        public int BlockId { get; set; }
        public int? SharedAreaId { get; set; }
        public Guid? Guid { get; set; }
    }
}
