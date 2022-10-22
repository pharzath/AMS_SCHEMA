using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeConstraint
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Over { get; set; }
        public string? Command { get; set; }
        public int? LabelFk { get; set; }
        public int? RelTypeFk { get; set; }
    }
}
