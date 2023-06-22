using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelProperty
{
    public int Id { get; set; }

    public int LabelId { get; set; }

    public string Name { get; set; } = null!;

    public string? DataType { get; set; }

    public string? Description { get; set; }

    public string? DisplayName { get; set; }

    public string? ValidationType { get; set; }

    public int? Order { get; set; }

    public string? Neo4jName { get; set; }

    public Guid Guid { get; set; }
    public bool? AllowNull { get; set; }
    public bool? AlternateGrainKey { get; set; }
    public string? ItemsStringValue { get; set; }
}
