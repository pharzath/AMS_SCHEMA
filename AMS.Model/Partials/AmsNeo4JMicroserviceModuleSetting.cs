using System.ComponentModel.DataAnnotations.Schema;
using AMS.Model.Models;

namespace AMS.Model;

public partial class AmsNeo4JMicroserviceModuleSetting
{
    [ForeignKey("MicroserviceModuleFk")]
    public AmsNeo4JMicroserviceModule Module { get; set; }

    [ForeignKey("LabelFk")]
    public AmsNeo4JNodeLabel Label { get; set; }


    [ForeignKey(nameof(CopiedFromModuleFk))]
    public AmsNeo4JMicroserviceModule? CopiedFromModule { get; set; }



}
public enum SettingTypeEnum { Simple, MethodHandler }
