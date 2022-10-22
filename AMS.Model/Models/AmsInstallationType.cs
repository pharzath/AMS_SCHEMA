using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsInstallationType
    {
        public AmsInstallationType()
        {
            InverseParentInstallation = new HashSet<AmsInstallationType>();
        }

        public int ItemId { get; set; }
        public int? ItemCreatedBy { get; set; }
        public DateTime? ItemCreatedWhen { get; set; }
        public int? ItemModifiedBy { get; set; }
        public DateTime? ItemModifiedWhen { get; set; }
        public int? ItemOrder { get; set; }
        public Guid ItemGuid { get; set; }
        public int? ParentInstallationId { get; set; }
        public string Title { get; set; } = null!;
        public int? Level { get; set; }

        public virtual AmsInstallationType? ParentInstallation { get; set; }
        public virtual ICollection<AmsInstallationType> InverseParentInstallation { get; set; }
    }
}
