using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsWelfare
    {
        public int WelfareId { get; set; }
        public string WlTitle { get; set; } = null!;
        public bool? WlIsPublic { get; set; }
        public bool? WlIsWelfare { get; set; }
        public bool? WlIsPrivate { get; set; }
        public string? WlUnit { get; set; }
        public bool? WlIsIncome { get; set; }
        public bool? WlIsIncomeDaily { get; set; }
        public string? WlIncomeDailyPrice { get; set; }
        public bool? WlIsIncomeWeekly { get; set; }
        public string? WlIncomeWeeklyPrice { get; set; }
        public bool? WlIsIncomeMonthly { get; set; }
        public string? WlIncomeMonthlyPrice { get; set; }
        public bool? WlIsIncomeyearly { get; set; }
        public string? WlIncomeYearlyPrice { get; set; }
        public bool? WlIsCost { get; set; }
        public bool? WlIsCostDaily { get; set; }
        public string? WlCostDailyPrice { get; set; }
        public bool? WlIsCostWeekly { get; set; }
        public string? WlCostWeeklyPrice { get; set; }
        public bool? WlIsCostMonthly { get; set; }
        public string? WlCostMonthlyPrice { get; set; }
        public bool? WlIsCostYearly { get; set; }
        public string? WlCostYearlyPrice { get; set; }
        public Guid Guid { get; set; }
        public DateTime LastModified { get; set; }
        public int? ResidentalComplexId { get; set; }
        public int? BlockId { get; set; }
        public int? UnitId { get; set; }
    }
}
