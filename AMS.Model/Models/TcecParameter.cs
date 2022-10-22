using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class TcecParameter
    {
        public int Pid { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; } = null!;
        public string? DefValue { get; set; }
        public bool? Deleted { get; set; }
        public int? Order { get; set; }
        public string? ItemGroup { get; set; }
        public int? InputControl { get; set; }
        public string? InputControlConfig { get; set; }
        public bool? InputControlConfigIsSql { get; set; }
        public string? InputControlWatermark { get; set; }
        public string? InputControlLableText { get; set; }
        public string? InputContolNote { get; set; }
        public string? Icon { get; set; }
        public bool? SingleSelection { get; set; }
        public string? InputControlIndicatorText { get; set; }
        public string? InputControlIndicatorIcon { get; set; }
        public string? InputControlIndicatorTip { get; set; }
        public int TypeCategoryId { get; set; }
    }
}
