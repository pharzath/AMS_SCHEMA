using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabel
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? NodeId { get; set; }

    public string? Description { get; set; }

    public long? ParentLabelId { get; set; }

    public string? Icon { get; set; }

    public string? IconContent { get; set; }

    public string? Color { get; set; }

    public string? DisplayName { get; set; }

    public string? Guid { get; set; }

    public long IsAbstract { get; set; }

    public long IsVirtual { get; set; }

    public long IsStaticData { get; set; }

    public string? FunctionalId { get; set; }
}
