using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsSharedArea
    {
        public int SharedAreaId { get; set; }
        public string Title { get; set; } = null!;
        public int? FloorId { get; set; }
        public int? ParentSharedAreaId { get; set; }
        public int? BlockId { get; set; }
        public int? RoofId { get; set; }
    }
}
