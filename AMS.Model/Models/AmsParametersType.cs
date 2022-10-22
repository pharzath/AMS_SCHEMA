using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsParametersType
    {
        public AmsParametersType()
        {
            AmsParameters = new HashSet<AmsParameter>();
        }

        public int Ptid { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public DateTime? LastModified { get; set; }
        public Guid ItemGuid { get; set; }

        public virtual AmsParameterTypeCategory Category { get; set; } = null!;
        public virtual ICollection<AmsParameter> AmsParameters { get; set; }
    }
}
