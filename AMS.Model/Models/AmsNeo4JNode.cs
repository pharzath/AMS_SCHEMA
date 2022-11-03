using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNode
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? DisplayName { get; set; }
        public int? LabelId { get; set; }
        public int? ProjectFk { get; set; }
    }
}
