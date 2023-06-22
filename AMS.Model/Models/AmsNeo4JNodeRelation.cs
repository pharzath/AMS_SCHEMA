using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeRelation
{
    public int Id { get; set; }

    public bool IsRequiredRelation { get; set; }

    public int? FromFk { get; set; }

    public int? ToFk { get; set; }

    public int? TypeFk { get; set; }

    public string? Description { get; set; }

    public string? InEntPropName { get; set; }
    
    public EntPropTypeEnum InEntPropType { get; set; }
    
    public string? OutEntPropName { get; set; }
    
    public EntPropTypeEnum OutEntPropType { get; set; }
}

