using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace AMS.Model;

[Table("AMS_Neo4J_Microservice_Module_Setting")]
[PrimaryKey(nameof(MicroserviceModuleFk), nameof(LabelFk) , nameof(Key))]
public partial class AmsNeo4JMicroserviceModuleSetting
{
    public int MicroserviceModuleFk { get; set; }
    
    public int LabelFk { get; set; }

    public SettingTypeEnum SettingType { get; set; }
    public string Key { get; set; }
    public string? Value { get; set; }
    public string? DataValue { get; set; }
    public string? Description { get; set; }

    public int? CopiedFromModuleFk { get; set; }

}