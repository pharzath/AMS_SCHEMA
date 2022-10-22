﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ViewCmsResourceTranslatedJoined
    {
        public int StringId { get; set; }
        public string StringKey { get; set; } = null!;
        public string? TranslationText { get; set; }
        public int CultureId { get; set; }
        public string CultureName { get; set; } = null!;
        public string CultureCode { get; set; } = null!;
    }
}
