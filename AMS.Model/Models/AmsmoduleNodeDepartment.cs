using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsmoduleNodeDepartment
    {
        public int NodeDepartmentId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CmsClassId { get; set; }
        public int? NodeId { get; set; }
    }
}
