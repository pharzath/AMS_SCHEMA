using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ViewCmsUserRoleMembershipRoleValidOnlyJoined
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
