using AMS.Model.Models;

namespace CodeGenerator.Gens;

public class RequestAction
{
    public string Name { get; set; }
    public ActionTypeEnum Return { get; set; }
    public enum ActionTypeEnum
    {
        Dto,
        CollectionDto,
        String,
        Int,
        Bool,
    }

    public string GetReturnType(AmsNeo4JNodeLabel label)
    {
        return Return switch
        {
            RequestAction.ActionTypeEnum.Dto => label.Name + "Dto",
            RequestAction.ActionTypeEnum.CollectionDto => $"IEnumerable<{label.Name}Dto>",
            RequestAction.ActionTypeEnum.String => "string",
            RequestAction.ActionTypeEnum.Int => "int",
            RequestAction.ActionTypeEnum.Bool => "bool",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override string ToString()
    {
        return Name;
    }
}