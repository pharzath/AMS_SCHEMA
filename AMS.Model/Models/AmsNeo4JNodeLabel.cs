using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeLabel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? NodeId { get; set; }
        public string? Description { get; set; }
        public int? ParentLabelId { get; set; }
        public string? Icon { get; set; }
        public string? IconContent { get; set; }
        public string? Color { get; set; }
        public string? DisplayName { get; set; }
        public Guid? Guid { get; set; }
    }
}
