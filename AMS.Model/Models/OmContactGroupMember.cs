﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class OmContactGroupMember
    {
        public int ContactGroupMemberId { get; set; }
        public int ContactGroupMemberContactGroupId { get; set; }
        public int ContactGroupMemberType { get; set; }
        public int ContactGroupMemberRelatedId { get; set; }
        public bool? ContactGroupMemberFromCondition { get; set; }
        public bool? ContactGroupMemberFromAccount { get; set; }
        public bool? ContactGroupMemberFromManual { get; set; }

        public virtual OmContactGroup ContactGroupMemberContactGroup { get; set; } = null!;
    }
}
