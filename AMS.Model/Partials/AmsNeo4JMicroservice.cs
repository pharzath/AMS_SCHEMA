using System.ComponentModel.DataAnnotations.Schema;
using AMS.Model.Models;
using QOQNOS.Core;
using static AMS.Model.AmsNeo4JMicroserviceModule;

namespace AMS.Model;

public partial class AmsNeo4JMicroservice : IHaveId<int>
{
    [ForeignKey("ProjectFk")]
    public AmsNeo4JProject Project { get; set; }

    //[ForeignKey("MicroserviceFk")]
    public List<AmsNeo4JMicroserviceModule> Modules { get; set; }

    public override string ToString()
    {
        return string.Join('.', Project?.Namespace , Name);
    }

    public string GetModuleName(string moduleName)
    {
        return $"{Name}.{moduleName}";
    }
    public AmsNeo4JMicroserviceModule GetModule(ModuleTypeEnum type)
    {
        return Modules.First(x => x.ModuleType == type);
    }
}