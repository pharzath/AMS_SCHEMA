using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Model;

[Table("AMS_Neo4J_Microservice")]
public partial class AmsNeo4JMicroservice
{
    public int Id { get; set; }

    public int ProjectFk { get; set; }
    
    public required string Name { get; set; }

    public required string Folder { get; set; }

}