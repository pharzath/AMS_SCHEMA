using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsResident
    {
        public int Rid { get; set; }
        public int PersonId { get; set; }
        public bool? IsContractor { get; set; }
        public bool? IsFinancialResponder { get; set; }
        public bool? NotifyBySms { get; set; }
        public bool? NotifyByEmail { get; set; }
        public bool? NotifyByPhoneCall { get; set; }
        public bool? NotifyByPrintOrder { get; set; }
        public string ResidentTitle { get; set; } = null!;
        public int? ContractOwnerRate { get; set; }
        public bool? IsManagingResponder { get; set; }
        public string? ResidentationRelation { get; set; }
        public int ResidentationId { get; set; }
        public int? BoradOfDiretorRoleId { get; set; }
    }
}
