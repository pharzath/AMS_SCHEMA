using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsmoduleGqlSchemaFileInfo
    {
        public int GqlSchemaFileInfoId { get; set; }
        public string? SchemaName { get; set; }
        public int? KeyCount { get; set; }
        public int? ParentCount { get; set; }
        public string? Parents { get; set; }
        public int? ChildCount { get; set; }
        public string? Children { get; set; }
        public string? Keys { get; set; }
        public bool? HasAuthOwner { get; set; }
        public string? AuthOwnerField { get; set; }
        public string? HasAuthGroup { get; set; }
        public string? AuthGroupField { get; set; }
        public string? AuthGroups { get; set; }
    }
}
