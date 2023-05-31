using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeIndex
{
    public long Id { get; set; }

    public long? LabelId { get; set; }

    public string? Type { get; set; }

    public string? Over { get; set; }

    public string? Command { get; set; }

    public long? RelTypeId { get; set; }
}
