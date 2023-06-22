using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using QOQNOS.Core;

namespace AMS.Model;

[Table("AMS_Neo4J_Microservice_Module_Setting_Default")]
public partial class AmsNeo4JMicroserviceModuleSettingDefault
{
    public int Id { get; set; }

    public int MicroserviceModuleFk { get; set; }

    public string DefaultKey { get; set; }
    public string? DefaultValue { get; set; }
    public string? Description { get; set; }
}


