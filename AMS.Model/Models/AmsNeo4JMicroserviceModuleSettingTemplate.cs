using System.ComponentModel.DataAnnotations.Schema;

namespace AMS.Model;

[Table("AMS_Neo4J_Microservice_Module_Setting_Template")]
public class AmsNeo4JMicroserviceModuleSettingTemplate 
{
    public int Id { get; set; }

    public SettingTypeEnum SettingType { get; set; }
    public string Key { get; set; }
    public string? Value { get; set; }
    
    [Column(TypeName = "nvarchar(max)")]
    public string? DataValue { get; set; }
    public string? Description { get; set; }

    [NotMapped]
    public HandlerSettingItem? HandlerSettingItem { get; set; }
}