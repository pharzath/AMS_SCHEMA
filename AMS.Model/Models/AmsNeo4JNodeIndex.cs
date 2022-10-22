using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeIndex
    {
        public int Id { get; set; }
        public int? LabelId { get; set; }
        public string? Type { get; set; }
        public string? Over { get; set; }
        public string? Command { get; set; }
        public int? RelTypeId { get; set; }
    }
}
