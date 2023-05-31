using QOQNOS.Core;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelQuery : IHaveId<long>
{
    [JsonIgnore]
    [ForeignKey("LabelId")]
    public AmsNeo4JNodeLabel Label { get; set; }

}