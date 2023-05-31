using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelRequiredRelation
{
    public long Id { get; set; }

    public long LabelRelationFk { get; set; }

    public long ReqCount { get; set; }

    public long? IsForceToCreateNew { get; set; }

    public long LabelFk { get; set; }

    public string? Description { get; set; }
}
