using System.Reflection;
using System.Web;
using AMS.Model;
using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Application.ExtensionMethods;
using AMS_SCHEMA.CodeGenerator.Api;
using AMS_SCHEMA.CodeGenerator.Api.Interface;
using AMS_SCHEMA.CodeGenerator.Api.Service;
using AMS_SCHEMA.CodeGenerator.Application;
using AMS_SCHEMA.CodeGenerator.Application.Contract;
using AMS_SCHEMA.CodeGenerator.BP41;
using AMS_SCHEMA.CodeGenerator.Grain;
using AMS_SCHEMA.CodeGenerator.GrainInterface;
using AMS_SCHEMA.Pages.Schema.Label.Module;
using Azure.Core;
using BlazorTemplater;
using Blueprint41.DatastoreTemplates;
using MudBlazor;
using Neo4jClient.DataAnnotations.Cypher;
using Newtonsoft.Json.Linq;
using Olive;


namespace AMS_SCHEMA.CodeGenerator;

public class CodeGeneratorService
{
    readonly DataService _dataService;

    public CodeGeneratorService(DataService dataService)
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

        GenerateEntityServiceApi(label, fromRelations);
        GenerateEntityServiceApiPartial(label);

        GenerateEntityServiceApiInterface(label);
        GenerateEntityServiceApiInterfacePartial(label);

        GenerateEntityServiceApiContracts(label, fromRelations, toRelations);
        GenerateEntityServiceApiContractsPartial(label);

    }

    void GenerateEntityServiceApi(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> fromRelations)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\API\{label.Name}ServiceApi.cs";
        var html = new ComponentRenderer<GenerateEntityApiController>()
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

        var html = new ComponentRenderer<GenerateEntityApiControllerPartial>()
            .Set(c => c.Label, label)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);

    }

    void GenerateEntityServiceApiInterface(AmsNeo4JNodeLabel label)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\Interface\I{label.Name}ServiceApi.cs";
        var html = new ComponentRenderer<GenerateEntityApiInterface>()
            .Set(c => c.Label, label)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);

    }
    void GenerateEntityServiceApiInterfacePartial(AmsNeo4JNodeLabel label)
    {
        var fileName = $@"{applicationBasePath}\AMS\Services\Interface\I{label.Name}ServiceApi.partial.cs";
        if (File.Exists(fileName)) return;

        var html = new ComponentRenderer<GenerateEntityApiInterfacePartial>()
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

        var html = new ComponentRenderer<GenerateEntityApiInterfacePartial>()
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

    public void GenerateEntityPartial(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> fromRelations, List<AmsNeo4JNodeRelation> toRelations, AmsNeo4JMicroserviceModule? module = null)
    {
        var fileName = module.Model_GetEntityPartialFileName(label);
        if (File.Exists(fileName)) return;

        var html = new ComponentRenderer<GenerateNeo4jEntityPartial>()
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Relations, fromRelations)
            .Set(c => c.IncommingRelations, toRelations)
            .Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        File.WriteAllText(fileName, htmlDecode);
    }

    public void GenerateEntity(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> relations, AmsNeo4JMicroserviceModule? module = null)
    {
        var fileName = module.Model_GetEntityFileName(label);

        var html = new ComponentRenderer<GenerateNeo4jEntity>()
            .AddService<DataService>(_dataService)
            .Set(c => c.Module, module)
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
        if (File.Exists(fileName)) return;

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


    // -------------------------
    // -------------------------
    // -------------------------
    // -------------------------


    public void GenerateAPIInterface(
        AmsNeo4JNodeLabel label,
        AmsNeo4JMicroserviceModule? module,
        List<AmsNeo4JNodeRelation> relations,
        IEnumerable<HandlerSettingItem> handlers,
        bool withPartial = true)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityApiInterface>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Handlers, handlers)
            .GenerateFile()
            ;


        if (withPartial)
            GenerateAPIInterfacePartial(label, relations, module);
    }


    public void GenerateModule(AmsNeo4JNodeLabel label, AmsNeo4JMicroserviceModule? module)
    {
        if (module == null)
            return;

        var relations = _dataService.GetLabelRelations(label);
        var fromRelations = _dataService.GetLabelRelationFroms(label);
        var toRelations = _dataService.GetLabelRelationTos(label);
        var constraints = _dataService.GetNodeConstraints(label);


        var list = _dataService.GetLabelModuleSettings(label.Id, module.Id)!;
        var handlers = list
            .Where(x => x.DataValue.HasValue())
            .Select(x => JObject.Parse(x.DataValue).ToObject<HandlerSettingItem>())
            .ToList();


        switch (module.ModuleType)
        {
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.API:
                GenerateAPIController(label, module, fromRelations, handlers, false);

                Console.WriteLine($" =================> Generating {module.ModuleType}: ({module.Name})   for  ({label.Name}) ");

                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.API_Interface:


                foreach (var handler in handlers)
                {
                    GenerateAPIInterfaceRequest(label, module, handler, false);
                }

                GenerateAPIInterface(label, module, relations, handlers, false);


                Console.WriteLine($" =================> Generating {module.ModuleType}: ({module.Name})   for  ({label.Name}) ");

                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.APPLICATION_CONTRACT:
                GenerateApplicationContractDto(label, module);

                Console.WriteLine($" =================> Generating {module.ModuleType}: ({module.Name})   for  ({label.Name}) ");
                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.APPLICATION:

                GenerateApplicationService(label, module, handlers, false);
                foreach (var handler in handlers)
                    GenerateApplicationCommand(label, module, handler, false);

                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.APPLICATION_GRAINS:
                GenerateGrain(label, module, handlers);
                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.APPLICATION_GRAINS_Interface:
                GenerateGrainInterface(label, module, handlers);
                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.APPLICATION_SILO:
                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.DOMAIN:
                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.MODEL:
                GenerateEntity(label, relations, module);
                GenerateEntityPartial(label, fromRelations, toRelations, module);
                Console.WriteLine($" =================> Generating Module: ({module.Name})   for  ({label.Name}) ");
                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.MOBILE:
                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.UI_WEB:
                break;
            case AmsNeo4JMicroserviceModule.ModuleTypeEnum.UI_WEB_CORE:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void GenerateApplicationContractDto(AmsNeo4JNodeLabel label, AmsNeo4JMicroserviceModule module)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityDto>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .GenerateFile()
            ;
    }

    public void GenerateAPIInterfacePartial(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> relations, AmsNeo4JMicroserviceModule? module = null)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityApiInterfacePartial>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .GenerateFile()
            ;


    }

    void GenerateAPIInterfaceRequest(
        AmsNeo4JNodeLabel label,
        AmsNeo4JMicroserviceModule? module,
        HandlerSettingItem? handler,
        bool withPartial = true)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityApiInterfaceRequest>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Handler, handler)
            .GenerateFile();

        if (withPartial)
            GenerateAPIInterfaceRequestPartial(label, module, handler);

    }

    void GenerateAPIInterfaceRequestPartial(AmsNeo4JNodeLabel label, AmsNeo4JMicroserviceModule? module, HandlerSettingItem? handler)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityApiInterfaceRequestPartial>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Handler, handler)
            .GenerateFile()
            ;

    }

    //================
    //================
    //================
    //================

    public void GenerateAPIController(
        AmsNeo4JNodeLabel label,
        AmsNeo4JMicroserviceModule? module,
        List<AmsNeo4JNodeRelation> relations,
        IEnumerable<HandlerSettingItem?> requests,
        bool withPartial = false)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityApiController>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Handlers, requests)
            .Set(c => c.FromRelations, relations)
            .GenerateFile()
            ;


        if (withPartial)
            GenerateAPIControllerPartial(label, relations, module);
    }

    void GenerateAPIControllerPartial(AmsNeo4JNodeLabel label, List<AmsNeo4JNodeRelation> relations, AmsNeo4JMicroserviceModule module)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityApiControllerPartial>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .GenerateFile()
            ;
    }

    //================
    //================
    //================
    //================

    public void GenerateApplicationService(
        AmsNeo4JNodeLabel label,
        AmsNeo4JMicroserviceModule? module,
        IEnumerable<HandlerSettingItem?> handlers,
        bool withPartial = false)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityApplicationService>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Handlers, handlers)
            .GenerateFile()
            ;


        if (withPartial)
            GenerateApplicationServicePartial(label, module);
    }

    void GenerateApplicationServicePartial(AmsNeo4JNodeLabel label, AmsNeo4JMicroserviceModule module)
    {
        throw new NotImplementedException();
    }

    public void GenerateApplicationCommand(
        AmsNeo4JNodeLabel label,
        AmsNeo4JMicroserviceModule? module,
        HandlerSettingItem? handler,
        bool withPartial = false)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityApplicationCommand>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Handler, handler)
            .GenerateFile()
            ;


        if (withPartial)
            GenerateApplicationCommandPartial(label, module);
    }

    void GenerateApplicationCommandPartial(AmsNeo4JNodeLabel label, AmsNeo4JMicroserviceModule module)
    {
        throw new NotImplementedException();
    }

    //================
    //================
    //================
    //================

    public void GenerateGrainInterface(
        AmsNeo4JNodeLabel label,
        AmsNeo4JMicroserviceModule? module,
        IEnumerable<HandlerSettingItem?> handlers,
        bool withPartial = false)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityGrainInterface>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Handlers, handlers)
            .GenerateFile()
            ;


        if (withPartial)
            GenerateApplicationServicePartial(label, module);
    }

    //================
    //================
    //================
    //================

    public void GenerateGrain(
        AmsNeo4JNodeLabel label,
        AmsNeo4JMicroserviceModule? module,
        IEnumerable<HandlerSettingItem?> handlers,
        bool withPartial = false)
    {
        if (module == null)
            throw new Exception("Module is NULL, Generate Method required a Module.");

        new CodeGeneratorComponentRenderer<GenerateEntityGrain>()
            .AddService(_dataService)
            .Set(c => c.Module, module)
            .Set(c => c.Label, label)
            .Set(c => c.Handlers, handlers)
            .GenerateFile()
            ;


        if (withPartial)
            GenerateApplicationServicePartial(label, module);
    }

    // *********************************
    // *********************************
    // *********************************
    // *********************************
    public string GenerateBPDataStore(
IEnumerable<AmsNeo4JNodeLabel> labels,
        List<AmsNeo4JNodeRelation> relations)
    {
        var generatedCode = new CodeGeneratorComponentRenderer<BPDataStore>()
            .AddService(_dataService)
            .Set(c => c.Labels, labels)
            .GetGeneratedCode();
        return generatedCode;
    }


}