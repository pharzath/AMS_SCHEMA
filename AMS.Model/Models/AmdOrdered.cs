using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmdOrdered
    {
        public int Id { get; set; }
        public string? Qty { get; set; }
        public string? UnitPrice { get; set; }
    }
}
