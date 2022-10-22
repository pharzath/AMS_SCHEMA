using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class FrzTmpView
    {
        public string? X { get; set; }
        public string? Y { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? FromFk { get; set; }
        public int? ToFk { get; set; }
        public int? TypeFk { get; set; }
        public string? Description { get; set; }
        public string? ClassDisplayName { get; set; }
    }
}
