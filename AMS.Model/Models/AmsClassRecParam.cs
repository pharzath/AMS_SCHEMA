using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsClassRecParam
    {
        public int Cpvid { get; set; }
        public string CpvClassName { get; set; } = null!;
        public int CpvClassRecordId { get; set; }
        public int CpvParamId { get; set; }
        public string CpvParamText { get; set; } = null!;
        public string? CpvParamValue { get; set; }
        public string? CpvParamDesc { get; set; }
        public int? CpvStatus { get; set; }
        public int Rcid { get; set; }
        public Guid Guid { get; set; }
        public DateTime LastModified { get; set; }

        public virtual AmsParameter CpvParam { get; set; } = null!;
    }
}
