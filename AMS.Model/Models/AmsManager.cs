using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsManager
    {
        public int ManagerId { get; set; }
        public string Title { get; set; } = null!;
        public int? BoardOfDirectorId { get; set; }
    }
}
