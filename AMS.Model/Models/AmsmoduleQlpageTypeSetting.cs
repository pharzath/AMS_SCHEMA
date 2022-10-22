using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsmoduleQlpageTypeSetting
    {
        public int QlpageTypeSettingsId { get; set; }
        public Guid QlpageTypeSettingsGuid { get; set; }
        public DateTime QlpageTypeSettingsLastModified { get; set; }
        public string? Auth { get; set; }
        public int? PageTypeId { get; set; }
        public string Type { get; set; } = null!;
        public bool? IsModel { get; set; }
        public string GeneratePath { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string? FkMappings { get; set; }
        public string? PageTypeType { get; set; }
        public string? IgnoreFields { get; set; }
        public string? SkipFieldPrefix { get; set; }
        public string? ExtraKeys { get; set; }
        public bool? HasOfflineSubscriptions { get; set; }
        public string? Subscriptions { get; set; }
        public string? ExtendedFields { get; set; }
        public bool? IgnoreInGroupGenerate { get; set; }
        public string Namespace { get; set; } = null!;
    }
}
