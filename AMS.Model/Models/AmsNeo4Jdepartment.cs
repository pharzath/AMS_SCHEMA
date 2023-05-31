using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JDepartment
{
    public long DepartmentId { get; set; }

    public string? Title { get; set; }

    public long? ProjectFk { get; set; }
}
