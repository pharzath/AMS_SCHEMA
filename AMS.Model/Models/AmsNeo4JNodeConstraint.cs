using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeConstraint
{
    public long Id { get; set; }

    public string? Type { get; set; }

    public string? Over { get; set; }

    public string? Command { get; set; }

    public long? LabelFk { get; set; }

    public long? RelTypeFk { get; set; }
}
