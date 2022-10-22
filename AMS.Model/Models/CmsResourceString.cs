﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class CmsResourceString
    {
        public CmsResourceString()
        {
            CmsResourceTranslations = new HashSet<CmsResourceTranslation>();
        }

        public int StringId { get; set; }
        public string StringKey { get; set; } = null!;
        public bool StringIsCustom { get; set; }
        public Guid StringGuid { get; set; }

        public virtual ICollection<CmsResourceTranslation> CmsResourceTranslations { get; set; }
    }
}
