using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JDepartment
    {
        public int DepartmentId { get; set; }
        public string? Title { get; set; }
        public int? ProjectFk { get; set; }
    }
}
