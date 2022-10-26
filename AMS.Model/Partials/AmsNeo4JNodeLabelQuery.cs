using QOQNOS.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelQuery : IHaveId<int>
{
    [ForeignKey("LabelId")]
    public AmsNeo4JNodeLabel Label { get; set; }

}