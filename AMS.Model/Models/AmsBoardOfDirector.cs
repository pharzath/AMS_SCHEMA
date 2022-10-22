using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsBoardOfDirector
    {
        public int BoardOfDirectorsId { get; set; }
        public string Title { get; set; } = null!;
        public int PersonelGroupId { get; set; }
    }
}
