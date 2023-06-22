using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QOQNOS.Core;

namespace AMS.Model;

public partial class AmsNeo4JMicroserviceModule : IHaveId<int>
{
    [ForeignKey("MicroserviceFk")]
    public AmsNeo4JMicroservice Microservice { get; set; }


    public ModuleTypeEnum ModuleType { get; set; }

    public enum ModuleTypeEnum
    {
        API,
        APPLICATION,
        APPLICATION_GRAINS,
        APPLICATION_GRAINS_Interface,
        APPLICATION_SILO,
        DOMAIN,
        MODEL,
        MOBILE,
        UI_WEB,
        UI_WEB_CORE,
        API_Interface,
        APPLICATION_CONTRACT
    }
    public override string ToString()
    {
        return Name;
    }

}