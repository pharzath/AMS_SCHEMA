using QOQNOS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeRelationType : IHaveId<int>
    {
        public List<AmsNeo4JNodeRelationPropery> Properties { get; set; }
        
        [ForeignKey("RelTypeFk")]
        public List<AmsNeo4JNodeConstraint> Constraints { get; set; }

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
