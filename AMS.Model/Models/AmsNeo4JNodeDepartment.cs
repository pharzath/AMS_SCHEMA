using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeDepartment
{
    public long NodeDepartmentId { get; set; }

    public long? DepartmentId { get; set; }

    public long? CmsClassId { get; set; }

    public long? NodeId { get; set; }
}
