using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JProject
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? DisplayName { get; set; }

    public string? Guid { get; set; }

    public string? Description { get; set; }
}
