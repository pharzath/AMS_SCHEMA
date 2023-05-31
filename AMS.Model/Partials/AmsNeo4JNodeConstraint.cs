using QOQNOS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeConstraint : IHaveId<long>
    {
        [JsonIgnore]
        public AmsNeo4JNodeLabel Label { get; set; }

        [JsonIgnore]
        [NotMapped]
        public AmsNeo4JNodeLabelProperty OverProp { get; set; }

        [JsonIgnore]
        public AmsNeo4JNodeRelationType RelType { get; set; }

        [JsonIgnore]
        [NotMapped]
        public AmsNeo4JNodeRelationProperty RelOverProp { get; set; }
    }
}
