using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class TcecParametersType
    {
        public int Ptid { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }

        public virtual AmsParameterTypeCategory Category { get; set; } = null!;
    }
}
