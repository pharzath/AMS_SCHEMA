using AMS.Model;
using AMS.Model.Models;
using AMS_SCHEMA.Class;
using CommunityToolkit.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AMS_SCHEMA.CodeGenerator;

public static class CodeGeneratorExtensionMethods
{

    public static bool ConstructorExist(this string filePath, string className, string[] parameterTypes)
    {
        // Load the C# syntax tree from the file
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));

        // Create a compilation object
        Compilation compilation = CSharpCompilation.Create("MyCompilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTree);

        // Get the root of the syntax tree
        SyntaxNode root = syntaxTree.GetRoot();


        // Find the method declaration within the class
        var methodDeclaration = root?.DescendantNodes()
            .OfType<ConstructorDeclarationSyntax>()
            .FirstOrDefault(m =>
                m.Identifier.ValueText == className &&
                m.ParameterList.Parameters.Count >= parameterTypes.Length &&
                parameterTypes.All(x=> m.ParameterList.Parameters.Select(p => p.Type.ToString()).Contains(x)))
            ;

        return methodDeclaration != null;

    }   
    
    
    public static bool DoesMethodExist(this string filePath,string methodName, string[] parameterTypes)
    {
        // Load the C# syntax tree from the file
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));

        // Get the root of the syntax tree
        SyntaxNode root = syntaxTree.GetRoot();


        // Find the method declaration within the class
        MethodDeclarationSyntax methodDeclaration = root?.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(m =>
                m.Identifier.ValueText == methodName &&
                m.ParameterList.Parameters.Count == parameterTypes.Length &&
                m.ParameterList.Parameters.Select(p =>
                    {
                        Console.WriteLine(p.Type.ToString());
                        return p.Type.ToString();
                    })
                    .SequenceEqual(parameterTypes));

        return methodDeclaration != null;

    }

    public static string? GetConstructorSourceCode(this string filePath, string className, string[] parameterTypes)
    {
        // Load the C# syntax tree from the file
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));

        // Get the root of the syntax tree
        SyntaxNode root = syntaxTree.GetRoot();

        // Find the method declaration
        var methodDeclaration = root.DescendantNodes()
            .OfType<ConstructorDeclarationSyntax>()
            .FirstOrDefault(m =>
                m.Identifier.ValueText == className &&
                m.ParameterList.Parameters.Count >= parameterTypes.Length &&
                parameterTypes.All(x => m.ParameterList.Parameters.Select(p => p.Type.ToString()).Contains(x)))
            ;

        // Retrieve the method source code
        var methodSourceCode = methodDeclaration?.ToString();
        return methodSourceCode;
    }

    public static string? GetMethodSourceCode(this string filePath, string methodName, string[] parameterTypes)
    {
        // Load the C# syntax tree from the file
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));

        // Get the root of the syntax tree
        SyntaxNode root = syntaxTree.GetRoot();

        // Find the method declaration
        MethodDeclarationSyntax methodDeclaration = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(m =>
                m.Identifier.ValueText == methodName &&
                m.ParameterList.Parameters.Count == parameterTypes.Length &&
                m.ParameterList.Parameters.Select(p => p.Type.ToString())
                    .SequenceEqual(parameterTypes));

        // Retrieve the method source code
        var methodSourceCode = methodDeclaration?.ToString();
        return methodSourceCode;
    }


//===============================================================================
//===============================================================================
//===============================================================================


    static string GetMe(AmsNeo4JMicroserviceModule module, ModuleTypeEnum moduleType,
        string className)
    {
        if (module.ModuleType != moduleType)
            throw new Exception($"Needs to specify a {moduleType} Module to generate this file type");

        var moduleRootFolder = module.FullPath!;
        var fileName = Path.Combine(moduleRootFolder, className.TrimStart('\\'));
        return fileName;
    }

    public static string Model_GetEntityFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {
        var fileName = GetMe(module, ModuleTypeEnum.Model, label.Name.ToPascalCase()) + ".cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
        {
            fileInfo.Directory.Create();
        }

        return fileName;
    }


    public static string Model_GetEntityPartialFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {
        if (module.ModuleType != ModuleTypeEnum.Model)
            throw new Exception($"Needs to specify a MODEL Module to generate this file type");

        var fileName = Path.Combine(module?.FullPath ?? "", "Partials", label.Name.ToPascalCase()) + ".partial.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
        {
            fileInfo.Directory.Create();
        }
        return fileName;
    }

    public static string Application_Contract_GetEntityDtoFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {
        var fileName = $"{GetMe(module, ModuleTypeEnum.Contracts,
            $"Dto\\{label.Name}\\{label.Name.ToPascalCase()}Dto")}.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
        {
            fileInfo.Directory.Create();
        }

        return fileName;
    }
    public static string ApiInterface_GetApiControllerInterfaceFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {
        var fileName = $"{GetMe(module, ModuleTypeEnum.API_Interface,
            $"{label.Name}\\I{label.Name.ToPascalCase()}Controller")}.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
        {
            fileInfo.Directory.Create();
        }

        return fileName;
    }
    public static string ApiInterface_GetApiControllerInterfacePartialFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {
        var fileName = $"{GetMe(module, ModuleTypeEnum.API_Interface,
            $"{label.Name}\\I{label.Name.ToPascalCase()}Controller")}.partial.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
        {
            fileInfo.Directory.Create();
        }

        return fileName;
    }
    public static string ApiInterface_GetApiControllerInterfaceRequestFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label, string? request)
    {
        if (request == null)
            throw new Exception("Invalid Handler Name , Handler is null");

        var fileName = $"{GetMe(module, ModuleTypeEnum.API_Interface,
            $"{label.Name.ToPascalCase()}\\Requests\\{request}Request")}.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
        {
            fileInfo.Directory.Create();
        }

        return fileName;
    }
    public static string ApiInterface_GetApiControllerInterfaceRequestPartialFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label, string? request)
    {
        if (request == null)
            throw new Exception("Invalid Handler Name , Handler is null");

        var fileName = $"{GetMe(module, ModuleTypeEnum.API_Interface,
            $"{label.Name.ToPascalCase()}\\Requests\\{request}Request")}.partial.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false) 
            fileInfo.Directory.Create();

        return fileName;
    }

    public static string Api_GetApiControllerFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {
        
        var fileName = $"{GetMe(module, ModuleTypeEnum.API_Endpoint,
            $"\\Controllers\\{label.Name.ToPascalCase()}Controller")}.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false) 
            fileInfo.Directory.Create();

        return fileName;
    }

    public static string Api_GetApiControllerPartialFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {

        var fileName = $"{GetMe(module, ModuleTypeEnum.API_Endpoint,
            $"\\Controllers\\{label.Name.ToPascalCase()}Controller")}.partial.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false) 
            fileInfo.Directory.Create();

        return fileName;
    }

    public static string Application_GetApplicationServiceFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {

        var fileName = $"{GetMe(module, ModuleTypeEnum.Application,
            $"Services\\{label.Name.ToPascalCase()}\\{label.Name.ToPascalCase()}Service")}.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false) 
            fileInfo.Directory.Create();

        return fileName;
    }

    public static string Application_GetApplicationServicePartialFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {

        var fileName = $"{GetMe(module, ModuleTypeEnum.Application,
            $"Services\\{label.Name.ToPascalCase()}\\{label.Name.ToPascalCase()}Service")}.partial.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false) 
            fileInfo.Directory.Create();

        return fileName;
    }

    public static string Application_GetApplicationCommandFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label, string? request)
    {

        var fileName = $"{GetMe(module, ModuleTypeEnum.Application,
            $"Services\\{label.Name.ToPascalCase()}\\{request}Command")}.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
            fileInfo.Directory.Create();

        return fileName;
    }

    public static string Grain_GetGrainInterfaceFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {

        var fileName = $"{GetMe(module, ModuleTypeEnum.GrainInterface,
            $"I{label.Name.ToPascalCase()}Grain")}.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
            fileInfo.Directory.Create();

        return fileName;
    }
    public static string Grain_GetGrainFileName(this AmsNeo4JMicroserviceModule module, AmsNeo4JNodeLabel label)
    {

        var fileName = $"{GetMe(module, ModuleTypeEnum.GrainInterface,
            $"{label.Name.ToPascalCase()}Grain")}.cs";

        var fileInfo = new FileInfo(fileName);
        if (!fileInfo.Directory?.Exists ?? false)
            fileInfo.Directory.Create();

        return fileName;
    }

}