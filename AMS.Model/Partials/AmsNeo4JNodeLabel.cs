using System.ComponentModel.DataAnnotations.Schema;
using Olive;
using QOQNOS.Core;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeLabel : IHaveId<int>
    {
        [ForeignKey("NodeId")]
        public AmsNeo4JNode Node { get; set; }

        public AmsNeo4JNodeLabel? ParentLabel { get; set; }

        [ForeignKey("ParentLabelId")]

        public List<AmsNeo4JNodeLabel>? ChildLabels { get; set; }

        [ForeignKey("LabelFk")]
        public List<AmsNeo4JNodeConstraint> Constraints { get; set; }

        [ForeignKey("LabelId")]
        public List<AmsNeo4JNodeIndex> Indices { get; set; }

        public List<AmsNeo4JNodeLabelPropery>? Properties { get; set; }
        
        public override string ToString()
        {
            return Name;
        }

        public static AmsNeo4JNodeLabel CreateNewLabel(AmsNeo4JNode node)
        {
            var label = new AmsNeo4JNodeLabel()
            {
                Name = node.Name + " Label",
                NodeId = node.Id,
                Node = node,
                Constraints = new (),
                Indices = new(),
                Properties = new ()
            };

            if (node.LabelId != null) 
                return label;

            node.Label = label;


            var entityid = "Id";
            var neoName = "entityId";
            //var propery = AmsNeo4JNodeLabelPropery.CreateNewPropery(label, entityid, neoName, "Guid", "none");
            //label.Properties.Add(propery);
            var constraint = new AmsNeo4JNodeConstraint()
            {
                Label = label,
                Over = entityid,
                Type = "IS NODE KEY",
                Command =
                    $@"CREATE CONSTRAINT constraint_XXNODENAMEXX_{neoName}
IF NOT EXISTS
FOR (a:XXNODENAMEXX) 
REQUIRE a.{neoName} IS NODE KEY "
            };
            label.Constraints.Add(constraint);

            return label;
        }

        public static AmsNeo4JNodeLabel CreateNewLabel(AmsNeo4JNodeLabel label)
        {
            var newLabel = CreateNewLabel(label.Node);
            newLabel.Name = "Sub"+label.Name;
            newLabel.ParentLabelId = label.Id;
            return newLabel;
        }
    }

    public partial class AmsNeo4JNodeLabelTreenNode 
    {
        public AmsNeo4JNodeLabel Label { get; init; }
        public bool IsExpanded { get; set; } = true;

        public HashSet<AmsNeo4JNodeLabelTreenNode> Items { get; set; }
    }
}
