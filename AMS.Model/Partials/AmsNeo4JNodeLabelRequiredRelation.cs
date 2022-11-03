using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOQNOS.Core;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelRequiredRelation : IHaveId<int>
{
    [JsonIgnore]
    [ForeignKey(nameof(LabelRelationFk))]
    public AmsNeo4JNodeRelation Relation { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(LabelFk))]
    public AmsNeo4JNodeLabel Label { get; set; }

}