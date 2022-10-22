using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsLocationType
    {
        public AmsLocationType()
        {
            AmsLocations = new HashSet<AmsLocation>();
        }

        public int LocTypeId { get; set; }
        public string? LocationTypeName { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid ItemGuid { get; set; }

        public virtual ICollection<AmsLocation> AmsLocations { get; set; }
    }
}
