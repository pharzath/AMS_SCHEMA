using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Olive;
using QOQNOS.Core;

namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabel : IHaveId<int>
{
    [JsonIgnore]
    [ForeignKey("NodeId")]
    public AmsNeo4JNode Node { get; set; }

    [JsonIgnore]
    public AmsNeo4JNodeLabel? ParentLabel { get; set; }

    [JsonIgnore]
    [ForeignKey("ParentLabelId")]
    public List<AmsNeo4JNodeLabel>? ChildLabels { get; set; }

    [JsonIgnore]
    [ForeignKey("LabelFk")]
    public List<AmsNeo4JNodeConstraint> Constraints { get; set; }

    [JsonIgnore]
    [ForeignKey("LabelId")]
    public List<AmsNeo4JNodeIndex> Indices { get; set; }

    [JsonIgnore]
    public List<AmsNeo4JNodeLabelProperty>? Properties { get; set; }

    [JsonIgnore]
    [ForeignKey("MicroserviceFk")]
    public AmsNeo4JMicroservice? Microservice { get; set; }

    //Relation is Specified in DbContext on Model Creating
    public AmsNeo4JNodeLabel? InheriteFromLabel { get; set; }
    
    [ForeignKey(nameof(FunctionalIdFk))]
    public AmsNeo4JNodeLabelFunctionalId? FunctionalId { get; set; }


    public List<AmsNeo4JMicroserviceModuleSetting>? ModuleSettings { get; set; }

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
        //var propery = AmsNeo4JNodeLabelProperty.CreateNewProperty(label, entityid, neoName, "Guid", "none");
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