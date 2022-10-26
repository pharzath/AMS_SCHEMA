using QOQNOS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeConstraint : IHaveId<int>
    {
        public AmsNeo4JNodeLabel Label { get; set; }

        [NotMapped]
        public AmsNeo4JNodeLabelPropery OverProp { get; set; }   
        
        public AmsNeo4JNodeRelationType RelType { get; set; }
        [NotMapped]
        public AmsNeo4JNodeRelationPropery RelOverProp { get; set; }
    }
}
