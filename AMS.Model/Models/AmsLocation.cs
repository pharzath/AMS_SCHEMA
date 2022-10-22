using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsLocation
    {
        public AmsLocation()
        {
            InverseParent = new HashSet<AmsLocation>();
        }

        public int ItemId { get; set; }
        public int LocationTypeId { get; set; }
        public int? ParentId { get; set; }
        public string LocationName { get; set; } = null!;
        public int? Zoom { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid ItemGuid { get; set; }
        public string? LocationPoints { get; set; }
        public string? LocType { get; set; }

        public virtual AmsLocationType LocationType { get; set; } = null!;
        public virtual AmsLocation? Parent { get; set; }
        public virtual ICollection<AmsLocation> InverseParent { get; set; }
    }
}
