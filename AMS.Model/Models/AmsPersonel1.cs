using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsPersonel1
    {
        public int PersonelsId { get; set; }
        public string Title { get; set; } = null!;
        public int? ParentPersonelGroupId { get; set; }
        public int? RcblockId { get; set; }
        public int? ResidentalComplexId { get; set; }
    }
}
