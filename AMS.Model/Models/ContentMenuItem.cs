using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ContentMenuItem
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = null!;
        public Guid? MenuItemTeaserImage { get; set; }
        public string? MenuItemGroup { get; set; }
    }
}
