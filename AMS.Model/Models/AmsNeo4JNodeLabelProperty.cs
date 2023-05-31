using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelProperty
{
    public long Id { get; set; }

    public long? LabelId { get; set; }

    public string Name { get; set; } = null!;

    public string? DataType { get; set; }

    public string? Description { get; set; }

    public string? DisplayName { get; set; }

    public string? ValidationType { get; set; }

    public long? Order { get; set; }

    public string? Neo4jName { get; set; }

    public string Guid { get; set; } = null!;
}
