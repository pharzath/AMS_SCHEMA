using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ComWishlist
    {
        public int UserId { get; set; }
        public int Skuid { get; set; }
        public int SiteId { get; set; }

        public virtual CmsSite Site { get; set; } = null!;
        public virtual ComSku Sku { get; set; } = null!;
        public virtual CmsUser User { get; set; } = null!;
    }
}
