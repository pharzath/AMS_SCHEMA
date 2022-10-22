using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeLabelPropery
    {
        public int Id { get; set; }
        public int? LabelId { get; set; }
        public string? Name { get; set; }
        public string? DataType { get; set; }
        public string? Description { get; set; }
        public string? DisplayName { get; set; }
        public string? ValidationType { get; set; }
        public int? Order { get; set; }
        public string? Neo4jName { get; set; }
    }
}
