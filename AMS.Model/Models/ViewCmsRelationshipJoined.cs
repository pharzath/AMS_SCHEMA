using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ViewCmsRelationshipJoined
    {
        public int LeftNodeId { get; set; }
        public Guid LeftNodeGuid { get; set; }
        public string LeftNodeName { get; set; } = null!;
        public string RelationshipName { get; set; } = null!;
        public int RelationshipNameId { get; set; }
        public int RightNodeId { get; set; }
        public Guid RightNodeGuid { get; set; }
        public string RightNodeName { get; set; } = null!;
        public string RelationshipDisplayName { get; set; } = null!;
        public string? RelationshipCustomData { get; set; }
        public int LeftClassId { get; set; }
        public int RightClassId { get; set; }
        public int RelationshipId { get; set; }
        public int? RelationshipOrder { get; set; }
    }
}
