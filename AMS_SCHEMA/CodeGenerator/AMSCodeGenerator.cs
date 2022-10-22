using System.Web;
using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.CodeGenerator.Api.Service;
using BlazorTemplater;

namespace AMS_SCHEMA.CodeGenerator;

public class AMSCodeGenerator
{
    readonly DataService _dataService;

    public AMSCodeGenerator(DataService dataService)
    {
        _dataService = dataService;
    }

    public void GenerateAll(List<AmsNeo4JNode> _nodes)
    {
        foreach (var pageType in _nodes)
        {
            GenerateEntityRelatedFiles(pageType);
        }

        GenerateRepositoryRegistrations(_nodes);
        GenerateDataContext(_nodes);
    }

    public void GenerateRepositoryRegistrations(List<AmsNeo4JNode> nodes)
    {
        var html = new ComponentRenderer<GenerateNeo4jEntityRepositoryRegistration>()
            .AddService(_dataService)
            .Set(c => c.Nodes, nodes)
            .Render();
        var fileName = @$"{applicationBasePath}\AMS\ServiceRegistration\RegisterRepositories.cs";
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
        Console.WriteLine(htmlDecode);
    }

    string modelBasePath = @"E:\Langram\NEO4J\TEST\QOQNOS.Neo4j.TEST.Model";
    string applicationBasePath = @"E:\Langram\NEO4J\TEST\QOQNOS.Neo4j.TEST.Application";
    string contextName = "AmsAppContext";

    public void GenerateDataContext(List<AmsNeo4JNode> nodes)
    {
        var html = new ComponentRenderer<GenerateDataContext>()
            .AddService(_dataService)
            .Set(c => c.Nodes, nodes)
            .Set(c => c.ContextName, "AmsDataContext")
            .Render();
        var fileName = @$"{applicationBasePath}\DataContext\" + contextName + ".cs";
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
        Console.WriteLine(htmlDecode);

    }


    public void GenerateEntityRelatedFiles(AmsNeo4JNode node)
    {

        var labels = _dataService.GetLabels(node).ToList();
        foreach (var label in labels)
        {
            GenerateEntityRelatedFiles(label);
        }

    }

    void GenerateEntityRelatedFiles(AmsNeo4JNodeLabel label)
    {
        var relations = _dataService.GetLabelRelations(label);
        var fromRelations = _dataService.GetLabelRelationFroms(label);
        var toRelations = _dataService.GetLabelRelationTos(label);
        var constraints = _dataService.GetNodeConstraints(label);

        GenerateEntity(label, relations);
        GenerateEntityPartial(label, fromRelations, toRelations);

        GenerateEntityRelations(label, relations);
        GenerateEntityRelationsPartial(label, relations);

        GenerateEntityValidator(label, fromRelations, toRelations, constraints);
        GenerateEntityValidatorPartial(label, fromRelations, toRelations, constraints);

        GenerateEntityRepository(label, relations, fromRelations, toRelations);
        GenerateEntityRepositoryPartial(label, relations, fromRelations, toRelations);

        GenerateEntityServiceApi(label,fromRelations);
        GenerateEntityServiceApiPartial(label);

        GenerateEntityServiceApiInterface(label);
        GenerateEntityServiceApiInterfacePartial(label);
        
        GenerateEntityServiceApiContracts(label,fromRelations,toRelations);
        GenerateEntityServiceApiContractsPartial(label);

    }

    void GenerateEntityServiceApi(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> fromRelations)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\API\{label.Name}ServiceApi.cs";
        var html = new ComponentRenderer<GenerateEntityServiceApi>()
            .Set(c => c.Label, label)
            .Set(c => c.FromRelations, fromRelations)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);

    }

    void GenerateEntityServiceApiPartial(AmsNeo4JNodeLabel label)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\API\{label.Name}ServiceApi.partial.cs";
        if (File.Exists(fileName)) return;

        var html = new ComponentRenderer<GenerateEntityServiceApiPartial>()
            .Set(c => c.Label, label)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);

    }

    void GenerateEntityServiceApiInterface(AmsNeo4JNodeLabel label)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\Interface\I{label.Name}ServiceApi.cs";
        var html = new ComponentRenderer<GenerateEntityServiceApiInterface>()
            .Set(c => c.Label, label)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);

    }
    void GenerateEntityServiceApiInterfacePartial(AmsNeo4JNodeLabel label)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\Interface\I{label.Name}ServiceApi.partial.cs";
        if (File.Exists(fileName)) return;

        var html = new ComponentRenderer<GenerateEntityServiceApiInterfacePartial>()
            .Set(c => c.Label, label)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);

    }


    void GenerateEntityServiceApiContracts(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> froms, List<AmsNeo4JNodeRelation> tos)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\Contract\{label.Name}Contracts.cs";
        var html = new ComponentRenderer<GenerateEntityServiceApiContract>()
            .Set(c => c.Label, label)
            .Set(c => c.FromRelations, froms)
            .Set(c => c.ToRelations, tos)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);

    }
    void GenerateEntityServiceApiContractsPartial(AmsNeo4JNodeLabel label)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\Contract\{label.Name}Contracts.partial.cs";
        if (File.Exists(fileName)) return;

        var html = new ComponentRenderer<GenerateEntityServiceApiInterfacePartial>()
            .Set(c => c.Label, label)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);

    }


    public void GenerateEntityRelations(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> relations)
    {
        foreach (var relation in relations)
        {
            GenerateEntityRelation(label, relation);
        }
    }

    public void GenerateEntityRelationsPartial(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> relations)
    {
        foreach (var relation in relations)
        {
            GenerateEntityRelationPartial(label, relation);
        }
    }

    public void GenerateEntityValidator(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> fromRelations, List<AmsNeo4JNodeRelation> toRelations, IEnumerable<AmsNeo4JNodeConstraint> constraints)
    {
        var fileName = $@"{applicationBasePath}\AMS\Domain\Validator\{label.Name}Validator.cs";
        var html = new ComponentRenderer<GenerateNeo4jEntityValidator>()
            .Set(c => c.Label, label)
            .Set(c => c.Relations, fromRelations)
            .Set(c => c.IncommingRelations, toRelations)
            .Set(c => c.Constraints, constraints)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
    }

    public void GenerateEntityValidatorPartial(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> fromRelations, List<AmsNeo4JNodeRelation> toRelations, IEnumerable<AmsNeo4JNodeConstraint> constraints)
    {
        var fileName = $@"{applicationBasePath}\AMS\Domain\Validator\{label.Name}Validator.partial.cs";
        if (File.Exists(fileName)) return;

        var html = new ComponentRenderer<GenerateNeo4jEntityValidatorPartial>()
            .Set(c => c.Label, label)
            .Set(c => c.Relations, fromRelations)
            .Set(c => c.IncommingRelations, toRelations)
            .Set(c => c.Constraints, constraints)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
    }

    public void GenerateEntityPartial(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> fromRelations, List<AmsNeo4JNodeRelation> toRelations)
    {
        var fileName = $@"{modelBasePath}\AMS\Model\{label.Name}.partial.cs";
        if (File.Exists(fileName)) return;

        var html = new ComponentRenderer<GenerateNeo4jEntityPartial>()
            .Set(c => c.Label, label)
            .Set(c => c.Relations, fromRelations)
            .Set(c => c.IncommingRelations, toRelations)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
    }

    public void GenerateEntity(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> relations)
    {
        var fileName = $@"{modelBasePath}\AMS\Model\{label.Name}.cs";
        var html = new ComponentRenderer<GenerateNeo4jEntity>()
            .AddService<DataService>(_dataService)
            .Set(c => c.Label, label)
            .Set(c => c.Relations, relations)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
    }

    public void GenerateEntityRelation(AmsNeo4JNodeLabel label, AmsNeo4JNodeRelation relation)
    {

        var fileName = $@"{modelBasePath}\AMS\Model\{relation.GetRelationFileName()}.cs";

        var html = new ComponentRenderer<GenerateNeo4jRelations>()
            .Set(c => c.Label, label)
            .Set(c => c.Relation, relation)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
        Console.WriteLine(htmlDecode);
    }
    public void GenerateEntityRelationPartial(AmsNeo4JNodeLabel label, AmsNeo4JNodeRelation relation)
    {

        var fileName = $@"{modelBasePath}\AMS\Model\{relation.GetRelationFileName()}.partial.cs";
        if (File.Exists(fileName)) return;
        
        var html = new ComponentRenderer<GenerateNeo4jRelationsPartial>()
            .Set(c => c.Label, label)
            .Set(c => c.Relation, relation)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
        Console.WriteLine(htmlDecode);
    }

    public void GenerateEntityRepository(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> all, List<AmsNeo4JNodeRelation> froms, List<AmsNeo4JNodeRelation> tos)
    {
        var html = new ComponentRenderer<GenerateNeo4jEntityRepository>()
            .AddService<DataService>(_dataService)
            .Set(c => c.Label, label)
            .Set(c => c.Relations, all)
            .Set(c => c.IncommingRelations, froms)
            .Set(c => c.OutgoingRelations, tos)
            .Render();
        var fileName = $@"{applicationBasePath}\AMS\Domain\Repository\{label.Name}Repository.cs";
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
        Console.WriteLine(htmlDecode);

    }
    public void GenerateEntityRepositoryPartial(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> all, List<AmsNeo4JNodeRelation> froms, List<AmsNeo4JNodeRelation> tos)
    {
        var fileName = $@"{applicationBasePath}\AMS\Domain\Repository\{label.Name}Repository.partial.cs";
        if(File.Exists(fileName)) return;
        
        var html = new ComponentRenderer<GenerateNeo4jEntityRepositoryPartial>()
            .Set(c => c.Label, label)
            .Set(c => c.Relations, all)
            .Set(c => c.IncommingRelations, froms)
            .Set(c => c.OutgoingRelations, tos)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
        Console.WriteLine(htmlDecode);

    }
}