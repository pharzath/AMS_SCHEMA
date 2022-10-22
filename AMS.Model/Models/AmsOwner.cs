using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsOwner
    {
        public int OwnerId { get; set; }
        public int PersonId { get; set; }
        public string Title { get; set; } = null!;
        public int? ContractOwnerRate { get; set; }
        public bool? IsFinancialResponder { get; set; }
        public bool? IsManagingResponder { get; set; }
        public bool? NotifyBySms { get; set; }
        public bool? NotifyByEmail { get; set; }
        public bool? NotifyByPhoneCall { get; set; }
        public bool? NotifyByPrintOrder { get; set; }
        public DateTime? LastModified { get; set; }
        public int OwnershipId { get; set; }
        public int? BoradOfDiretorRoleId { get; set; }
    }
}
