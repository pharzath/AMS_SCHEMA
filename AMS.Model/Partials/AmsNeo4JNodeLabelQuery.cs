using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelQuery
{
    [ForeignKey("LabelId")]
    public AmsNeo4JNodeLabel Label { get; set; }

}