using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsPersonel
    {
        public int PersonelId { get; set; }
        public string Title { get; set; } = null!;
        public int PersonelGroupId { get; set; }
        public int? BoradOfDiretorRoleId { get; set; }
    }
}
