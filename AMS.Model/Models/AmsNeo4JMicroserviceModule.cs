using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model;

[Table("AMS_Neo4J_Microservice_Module")]
public partial class AmsNeo4JMicroserviceModule
{
    public int Id { get; set; }

    public int MicroserviceFk { get; set; }
    
    public string? Name { get; set; }

    public string? RootFolder { get; set; }

    public string? SubFolder { get; set; }


}