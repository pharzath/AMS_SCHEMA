using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabel
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? NodeId { get; set; }

    public string? Description { get; set; }

    public int? ParentLabelId { get; set; }

    public string? Icon { get; set; }

    public string? IconContent { get; set; }

    public string? Color { get; set; }

    public string? DisplayName { get; set; }

    public bool? IsAbstract { get; set; }

    public bool? IsVirtual { get; set; }

    public bool? IsStaticData { get; set; }

    public int? InheriteFromLabelId { get; set; }

    public int? FunctionalIdFk { get; set; }

    public int? MicroserviceFk { get; set;}

    public bool InheritPropsFromParentLabel { get; set; }
}
