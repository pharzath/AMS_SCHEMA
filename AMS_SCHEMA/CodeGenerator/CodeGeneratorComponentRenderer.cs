using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using AMS.Model;
using AMS.Model.Models;
using AMS_SCHEMA.CodeGenerator.Api;
using AMS_SCHEMA.CodeGenerator.Api.Interface;
using AMS_SCHEMA.CodeGenerator.Api.Service;
using AMS_SCHEMA.CodeGenerator.Application;
using AMS_SCHEMA.CodeGenerator.Application.Contract;
using AMS_SCHEMA.CodeGenerator.Grain;
using AMS_SCHEMA.CodeGenerator.GrainInterface;
using AMS_SCHEMA.Pages.Schema.Label.Module;
using BlazorTemplater;
using Microsoft.AspNetCore.Components;
using Olive;

namespace AMS_SCHEMA.CodeGenerator;

public class CodeGeneratorComponentRenderer<T> where T : IComponent, IHaveDestinationFilePath
{
    public ComponentRenderer<T> Renderer { get; set; }

    public CodeGeneratorComponentRenderer()
    {
        Renderer = new ComponentRenderer<T>();

    }

    AmsNeo4JMicroserviceModule _module;
    AmsNeo4JNodeLabel _label;

    public CodeGeneratorComponentRenderer<T> AddService<TValue>(TValue service)
    {
        Renderer = Renderer.AddService<TValue>(service);

        return this;
    }

    readonly Dictionary<ParameterEnum, object?> _params = new();

    public enum ParameterEnum { Handler }

    public CodeGeneratorComponentRenderer<T> AddParam<TValue>(ParameterEnum param, TValue? value)
    {
        _params.Add(param, value);
        return this;
    }

    public CodeGeneratorComponentRenderer<T> Set<TValue>(Expression<Func<T, TValue>> exp, TValue value) 
        
        where TValue : class
    {
        switch (value)
        {
            case AmsNeo4JMicroserviceModule module:
                _module = module;
                break;
            case AmsNeo4JNodeLabel label:
                _label = label;
                break;
            case HandlerSettingItem handler:
                _params.Add(ParameterEnum.Handler, handler);
                break;
            case string param:
                var memberName = GetMemberName(exp);
                switch (memberName)
                {
                    case "Handler":
                        _params.Add(ParameterEnum.Handler, value);
                        break;
                }
                break;
        }

        Renderer = Renderer.Set(exp, value);
        return this;
    }

    public static string GetMemberName<T, TValue>(Expression<Func<T, TValue>> exp)
    {
        if (exp.Body is MemberExpression memberExp)
        {
            return memberExp.Member.Name;
        }
        else if (exp.Body is UnaryExpression { Operand: MemberExpression unaryMemberExp })
        {
            return unaryMemberExp.Member.Name;
        }
        else
        {
            throw new ArgumentException("Invalid expression. Expected a member access expression.");
        }
    }

    public string GetGeneratedCode()
    {
        var html = Renderer.Render();
        var htmlDecode = HttpUtility.HtmlDecode(html);
        return htmlDecode;
    }

    public void GenerateFile(bool isPartial = false)
    {
        var fileName = GetFileName();
        Renderer.Set(component => component.DestFilePath , fileName);
        var generatedCode = GetGeneratedCode();
        var regionNamesRegex = new Regex(@"\#region\s+(.*)([\s\S]*?)\#endregion", RegexOptions.Multiline);
        var regionNamesInGeneratedCode = regionNamesRegex.Matches(generatedCode);

        if (File.Exists(fileName))
        {
            if (isPartial)
            {
                Console.WriteLine($"Partial File Already Exist : " + fileName);
                return;
            }

            var exisitingFileContetnt = File.ReadAllText(fileName);
            foreach (Match match in regionNamesInGeneratedCode)
            {
                var regeRegionContentRegex = new Regex($@"#region\s+{match.Groups[1].Value}([\s\S]*?)#endregion", RegexOptions.Multiline);
                var matches = regeRegionContentRegex.Matches(exisitingFileContetnt);
                foreach (Match m in matches)
                {
                    generatedCode = generatedCode.Replace(match.Value, m.Value);
                }
            }

            BackupOldFile(fileName);

        }

        File.WriteAllText(fileName, generatedCode);
    }

    void BackupOldFile(string fileName)
    {
        var backupFolder = @"E:\QOQNOS\BackupGen\" + _module.Name;
        if (!Directory.Exists(backupFolder)) 
            Directory.CreateDirectory(backupFolder);

        var secondPart = fileName.RemoveBeforeAndIncluding(_module.Name).Replace("\\","-");
        var destFileName = Path.Combine(backupFolder , secondPart.Replace(".cs", $"{DateTime.Now:.yyyy-MM-dd.hh-mm-ss}.gensave"));

        try
        {
            File.Move(fileName, destFileName);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    string GetFileName()
    {

        if (typeof(T) == typeof(GenerateEntityApiInterface))
            return _module.ApiInterface_GetApiControllerInterfaceFileName(_label);

        if (typeof(T) == typeof(GenerateEntityApiInterfacePartial))
            return _module.ApiInterface_GetApiControllerInterfacePartialFileName(_label);

        if (typeof(T) == typeof(GenerateEntityApiInterfaceRequest))
            return _module.ApiInterface_GetApiControllerInterfaceRequestFileName(_label, _params[ParameterEnum.Handler]?.ToString());

        if (typeof(T) == typeof(GenerateEntityApiInterfaceRequestPartial))
            return _module.ApiInterface_GetApiControllerInterfaceRequestPartialFileName(_label, _params[ParameterEnum.Handler]?.ToString());

        if (typeof(T) == typeof(GenerateEntityApiController))
            return _module.Api_GetApiControllerFileName(_label);

        if (typeof(T) == typeof(GenerateEntityApiControllerPartial))
            return _module.Api_GetApiControllerPartialFileName(_label);

        if (typeof(T) == typeof(GenerateEntityApplicationService))
            return _module.Application_GetApplicationServiceFileName(_label);

        if (typeof(T) == typeof(GenerateEntityDto))
            return _module.Application_Contract_GetEntityDtoFileName(_label);

        if (typeof(T) == typeof(GenerateEntityApplicationServicePartial))
            return _module.Application_GetApplicationServicePartialFileName(_label);

        if (typeof(T) == typeof(GenerateEntityApplicationCommand))
            return _module.Application_GetApplicationCommandFileName(_label, _params[ParameterEnum.Handler]?.ToString());

        if (typeof(T) == typeof(GenerateEntityGrainInterface))
            return _module.Grain_GetGrainInterfaceFileName(_label);

        if (typeof(T) == typeof(GenerateEntityGrain))
            return _module.Grain_GetGrainFileName(_label);

        throw new Exception("Not Supported Generator");
    }


    // var templaterField = Renderer.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic)[1];
    // var templater = templaterField.GetValue(Renderer);
    // var fieldInfo = typeof(ComponentRenderer<>).MakeGenericType(typeof(T))
    //     .GetField("TComponentType", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
    // var myCodeGenTemplate = fieldInfo?.GetValue(Renderer);

}