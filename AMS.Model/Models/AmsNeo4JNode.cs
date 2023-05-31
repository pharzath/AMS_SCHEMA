using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNode
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? DisplayName { get; set; }

    public long? LabelId { get; set; }

    public long? ProjectFk { get; set; }
}
