using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsRoom
    {
        public int RoomId { get; set; }
        public string Title { get; set; } = null!;
        public int? Area { get; set; }
        public int UnitId { get; set; }
    }
}
