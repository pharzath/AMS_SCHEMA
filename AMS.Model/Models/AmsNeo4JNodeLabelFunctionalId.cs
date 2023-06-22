using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Model.Models;

[Table("Ams_Neo4J_Node_Label_FunctionalId")]
public class AmsNeo4JNodeLabelFunctionalId
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Prefix { get; set; }
    public FunctionalIdTypeEnum Type { get; set; }
    public bool IsDefault { get; set; }
}