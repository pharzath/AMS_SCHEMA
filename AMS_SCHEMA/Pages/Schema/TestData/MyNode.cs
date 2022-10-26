using AMS.Model.Models;
using Neo4jClient;
using Newtonsoft.Json.Linq;

namespace AMS_SCHEMA.Pages.Schema.TestData;

public class MyNode
{
    public List<MyNode> Nodes { get; set; } = new List<MyNode>();

    public MyNode? ParentNode { get; set; }
    public AmsNeo4JNodeRelationType Relation { get; set; }
    public string RelType { get; set; }
    public RelationshipDirection Direction { get; set; }
    public QOQNOS.Core.EntityBase Entity { get; set; }
    public int Level { get; set; }
    public bool IsExpanded { get; set; } = false;
    public bool Loading { get; set; }
    public List<AmsNeo4JNodeLabel> Labels { get; set; }
    public string LabelsChainText => string.Concat(Labels.OrderBy(x => x.ParentLabelId).Select(x => x.Name));
    public string LabelsText => string.Join(" : ", Labels.OrderBy(x => x.ParentLabelId).Select(x => x.Name));
    public int Order { get; set; }
    public NodeStyle Style { get; set; }
    public bool EditingOrder { get; set; } = false;
    public JObject Jobj { get; set; }

    public MyNode(QOQNOS.Core.EntityBase entity)
    {
        Entity = entity;
    }

}
public class NodeStyle
{
    public double Opacity { get; set; } = 1;
}