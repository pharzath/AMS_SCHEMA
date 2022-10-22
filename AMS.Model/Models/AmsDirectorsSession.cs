using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsDirectorsSession
    {
        public int DirectorsSessionId { get; set; }
        public string Title { get; set; } = null!;
        public string DirectorsSessionFolderId { get; set; } = null!;
    }
}
