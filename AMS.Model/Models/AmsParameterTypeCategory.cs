using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsParameterTypeCategory
    {
        public AmsParameterTypeCategory()
        {
            AmsParametersTypes = new HashSet<AmsParametersType>();
            TcecParametersTypes = new HashSet<TcecParametersType>();
        }

        public int ItemId { get; set; }
        public string CategoryTitle { get; set; } = null!;
        public DateTime? LastModified { get; set; }
        public Guid ItemGuid { get; set; }

        public virtual ICollection<AmsParametersType> AmsParametersTypes { get; set; }
        public virtual ICollection<TcecParametersType> TcecParametersTypes { get; set; }
    }
}
