﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ViewOmContactGroupMemberAccountJoined
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string? AccountAddress1 { get; set; }
        public string? AccountAddress2 { get; set; }
        public string? AccountCity { get; set; }
        public string? AccountZip { get; set; }
        public int? AccountStateId { get; set; }
        public int? AccountCountryId { get; set; }
        public string? AccountWebSite { get; set; }
        public string? AccountPhone { get; set; }
        public string? AccountEmail { get; set; }
        public string? AccountFax { get; set; }
        public int? AccountPrimaryContactId { get; set; }
        public int? AccountSecondaryContactId { get; set; }
        public int? AccountStatusId { get; set; }
        public string? AccountNotes { get; set; }
        public int? AccountOwnerUserId { get; set; }
        public int? AccountSubsidiaryOfId { get; set; }
        public Guid AccountGuid { get; set; }
        public DateTime AccountLastModified { get; set; }
        public DateTime AccountCreated { get; set; }
        public int ContactGroupMemberContactGroupId { get; set; }
        public int ContactGroupMemberId { get; set; }
    }
}
