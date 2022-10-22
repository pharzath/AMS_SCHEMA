using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ViewCmsRoleResourcePermissionJoined
    {
        public int RoleId { get; set; }
        public string ResourceName { get; set; } = null!;
        public string PermissionName { get; set; } = null!;
        public int PermissionId { get; set; }
    }
}
