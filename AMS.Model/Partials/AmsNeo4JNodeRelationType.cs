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
    public partial class AmsNeo4JNodeRelationType : IHaveId<long>
    {
        [JsonIgnore]
        public List<AmsNeo4JNodeRelationProperty> Properties { get; set; }

        [JsonIgnore]
        [ForeignKey("RelTypeFk")]
        public List<AmsNeo4JNodeConstraint> Constraints { get; set; }

        [JsonIgnore]
        [ForeignKey("RelTypeId")]
        public List<AmsNeo4JNodeIndex> Indices { get; set; }

        public static AmsNeo4JNodeRelationType CreateNewNode(string relName,string? dispName = null)
        {
            return new AmsNeo4JNodeRelationType()
            {
                Name = relName,
                DisplayName = dispName
            };
        }
    }
}
