using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsmoduleIonicgeneratorSetting
    {
        public int Id { get; set; }
        public string PagePath { get; set; } = null!;
        public string? PageName { get; set; }
        public bool? HasPageSpec { get; set; }
        public int? PageTypeId { get; set; }
        public string? PageTypeType { get; set; }
        public string? ComponentPrefixReplacement { get; set; }
        public string? ComponentListControlNameFormat { get; set; }
        public string? ComponentsBasePath { get; set; }
        public string? ComponentItemControlNameFormat { get; set; }
        public string? ComponentItemDetailControlNameFormat { get; set; }
        public string? ComponentItemDetailModalNameFormat { get; set; }
    }
}
