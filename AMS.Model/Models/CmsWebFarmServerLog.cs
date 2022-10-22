using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class CmsWebFarmServerLog
    {
        public int WebFarmServerLogId { get; set; }
        public DateTime LogTime { get; set; }
        public string LogCode { get; set; } = null!;
        public int ServerId { get; set; }
    }
}
