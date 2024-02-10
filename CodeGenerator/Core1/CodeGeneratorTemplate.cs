using System.Text.RegularExpressions;
using AMS.Model;
using AMS.Model.Models;
using AMS_SCHEMA.Class;
using CodeGenerator.Gens;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis;
using Olive;

namespace CodeGenerator.Core1;

public class CodeGeneratorTemplate : ComponentBase, IHaveDestinationFilePath
{
	[Parameter]
	public string DestFilePath { get; set; }

	[Parameter]
	public AmsNeo4JNodeLabel Label { get; set; }

	[Parameter]
	public AmsNeo4JMicroserviceModule Module { get; set; }

	[Parameter]
	public IEnumerable<HandlerSettingItem> Handlers { get; set; }

	internal bool _codeGenHint = false;
	protected override void OnInitialized()
	{
		_codeGenHint = true;
		if (DestFilePath.HasValue())
		{
			var regex = new Regex(@"//\s*Code\s*Gen\s*Hints\s*=\s*(?<val>.*)");
			var hintConf = File.ReadAllLines(DestFilePath).FirstOrDefault(x => regex.Match(x).Success);
			if (hintConf == null)
			{
				_codeGenHint = true;
				return;
			}
			var match = regex.Match(hintConf);
			_codeGenHint = match.Success && match.Groups["val"].Value.ToLower() == "true";
		}

		base.OnInitialized();
	}

	string GetParentClass()
	{
		if (Label.ParentLabel is { })
		{
			return Label.ParentLabel.Name.ToPascalCase();
		}

		return $"AbstractEntity<{Label.Name.ToPascalCase()}, Guid>";
	}

	string IsNullable(AmsNeo4JNodeLabelProperty fld)
	{
		return fld.Label.Constraints.Any(x => x.Over == fld.Name && x.Type is "IS NODE KEY" or "IS NOT NULL")
			? ""
			: "?";
	}

	public virtual string? GetHandlerParameters(HandlerSettingItem handler)
	{
		var altKey = Label.Properties.FirstOrDefault(x => x.AlternateGrainKey is true);
		if (handler.Parameters.Count == 1 &&
			(handler.Parameters.First().Name.OrEmpty().Equals("key", StringComparison.OrdinalIgnoreCase) ||
			 (altKey is { } && handler.Parameters.First().Name.OrEmpty() == altKey.Name))
		   )
			return "";
		var sb = handler.Parameters.Select(parameter => $"{GetParamType(parameter)} {GetParamName(parameter)}");
		return string.Join(',', sb);
	}

	internal string GetParamName(MethodParameter parameter, bool propertyName = false)
	{
		var name = parameter.IsSelfDto ? $"{Entity}Dto" : parameter.Name;
		if (parameter.IsCollection) name = name.ToPlural();
		return propertyName ? name.ToPascalCase() : name.ToCamelCase();
	}

	internal string GetParamType(MethodParameter param)
	{

		var typeName = "";
		typeName = param.IsSelfDto ? $"{Label.Name.ToPascalCase()}Dto" : param.Type;
		if (param.IsCollection)
			typeName = typeName.WithWrappers("IEnumerable<", ">");
		return typeName;
	}

	protected virtual string[]? GetMethodParameterTypes(HandlerSettingItem handler)
	{
		return null;
	}


	public bool MethodExists(HandlerSettingItem handler)
	{
		var paramTypes = GetMethodParameterTypes(handler);
		var exist = DestFilePath.DoesMethodExist(handler.Name.ToPascalCase(), paramTypes);

		return exist;
	}

	public string MethodSourceCode(HandlerSettingItem handler)
	{
		var paramTypes = GetMethodParameterTypes(handler);
		var source = DestFilePath.GetMethodSourceCode(handler.Name.ToPascalCase(), paramTypes);

		return source;
	}

	public SyntaxTrivia? MethodWrapedByRegion(HandlerSettingItem handler)
	{
		var paramTypes = GetMethodParameterTypes(handler);
		var region = DestFilePath.MethodIsInRegion(handler.Name.ToPascalCase(), paramTypes);

		return region;
	}

	protected virtual string ClassNameFormat => "{0}";
	public string ClassName => string.Format(Prepare(ClassNameFormat), Entity);
	protected string Entity => Label.Name.ToPascalCase();
	protected string entity => Label.Name.ToCamelCase();

	public bool ConstructorExist(params string[] paramTypes)
	{
		var exist = DestFilePath.ConstructorExist(ClassName, paramTypes);

		return exist;

	}

	public string GetConstructorSource(params string[] paramTypes)
	{
		var source = DestFilePath.GetConstructorSourceCode(ClassName, paramTypes);

		return source;
	}

	public string Prepare(string postFix)
	{
		return postFix.Replace("@(Entityt)", Entity)
				   	  .Replace("{ENTITY}", Entity)
				      .Replace("{MODULE_NAME}", Module.Name);
	}
}