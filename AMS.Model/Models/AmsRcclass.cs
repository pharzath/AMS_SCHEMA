using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsRcclass
    {
        public AmsRcclass()
        {
            AmsParameters = new HashSet<AmsParameter>();
        }

        public int ItemId { get; set; }
        public string Title { get; set; } = null!;
        public int ClassValue { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid ItemGuid { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public int? MaxFloors { get; set; }
        public int? MinFloors { get; set; }
        public string? DefualtFloorsValue { get; set; }
        public string? UnitPerFloor { get; set; }
        public int ClassGroupId { get; set; }
        public string? ImageName { get; set; }

        public virtual ICollection<AmsParameter> AmsParameters { get; set; }
    }
}
