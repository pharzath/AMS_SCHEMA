using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsSessionProceeding
    {
        public int SessionProceedingsId { get; set; }
        public string Title { get; set; } = null!;
        public int DirectorsSessionId { get; set; }
    }
}
