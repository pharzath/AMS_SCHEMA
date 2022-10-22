﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ChatPopupWindowSetting
    {
        public int ChatPopupWindowSettingsId { get; set; }
        public string MessageTransformationName { get; set; } = null!;
        public string ErrorTransformationName { get; set; } = null!;
        public string ErrorClearTransformationName { get; set; } = null!;
        public string UserTransformationName { get; set; } = null!;
        public int ChatPopupWindowSettingsHashCode { get; set; }
    }
}
