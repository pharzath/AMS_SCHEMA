using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelRequiredRelation
{
    public int Id { get; set; }

    public int LabelRelationFk { get; set; }

    public int ReqCount { get; set; }

    public bool? IsForceToCreateNew { get; set; }

    public int LabelFk { get; set; }

    public string? Description { get; set; }
}
