using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelQuery
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public long? LabelId { get; set; }

    public string? RelatedLabels { get; set; }

    public string? CypherQuery { get; set; }
}
