using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeRelation
{
    public long Id { get; set; }

    public long? FromFk { get; set; }

    public long? ToFk { get; set; }

    public long? TypeFk { get; set; }

    public string? Description { get; set; }

    public string Guid { get; set; } = null!;
}
