﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class OmContactChangeRecalculationQueue
    {
        public int ContactChangeRecalculationQueueId { get; set; }
        public int ContactChangeRecalculationQueueContactId { get; set; }
        public string? ContactChangeRecalculationQueueChangedColumns { get; set; }
        public bool ContactChangeRecalculationQueueContactIsNew { get; set; }
        public bool ContactChangeRecalculationQueueContactWasMerged { get; set; }
    }
}
