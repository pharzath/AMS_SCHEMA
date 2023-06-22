namespace AMS.Model.Models;

public partial class AmsNeo4JNodeLabelTreenNode 
{
    public AmsNeo4JNodeLabel Label { get; init; }
    public bool IsExpanded { get; set; } = true;

    public HashSet<AmsNeo4JNodeLabelTreenNode> Items { get; set; }
}