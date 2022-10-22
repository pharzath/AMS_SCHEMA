﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class CmsWebFarmServerTask
    {
        public int ServerId { get; set; }
        public int TaskId { get; set; }
        public string? ErrorMessage { get; set; }

        public virtual CmsWebFarmServer Server { get; set; } = null!;
        public virtual CmsWebFarmTask Task { get; set; } = null!;
    }
}
