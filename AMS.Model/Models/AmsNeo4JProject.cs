using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JProject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public Guid? Guid { get; set; }
        public string? Description { get; set; }
    }
}
