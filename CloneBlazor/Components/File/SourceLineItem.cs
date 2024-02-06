using AMS.Model;
using CloneBlazor.Components.File.Gen;
using Microsoft.AspNetCore.Components;
using Olive;
using ServiceStack.Html;
using System.Text.RegularExpressions;
using ServiceStack;

namespace CloneBlazor.Components.File;

public class SourceLineItem(string content, int lineNumber, GeneratorContext generatorContext)
{
	public GeneratorContext GeneratorContext { get; set; } = generatorContext;
	public int LineNumber { get; set; } = lineNumber;
	public string Content { get; set; } = content;
    public bool Selected { get; set; }

	public List<Suggestion> GetSuggestions()
	{
		List<Suggestion> suggestions = new();
		if (Content.StartsWith("using"))
		{
			return UsingSuggestions();
		}
		
		if (Content.StartsWith("namespace"))
		{
			return NamespaceSuggestions();
		}


		// class|struct|interface|enum|record
		if (
			Content.Contains(" class ") ||
			Content.Contains(" interface ") ||
			Content.Contains(" struct ") ||
			Content.Contains(" enum ") ||
			Content.Contains(" record ") 
		    )
		{
			return ClassSuggestions();
		}

		return suggestions;
	}

	private List<Suggestion> ClassSuggestions()
	{
		List<Suggestion> suggestions = [];

		var pattern =
			@"\b(?:(public|private|protected|internal)\s+)?(?:(static|sealed|abstract|partial)\s+)?\s*(?:(class|struct|interface|enum)\s+)(\w+)\b";
		var regex = new Regex(pattern);
		var match = regex.Match(Content);
        if (!match.Success) 
            return suggestions;

        var accessModifier = match.Groups[1].Value;
        var statusModifier = match.Groups[2].Value;
        var classModifier = match.Groups[3].Value;
        var name = match.Groups[4].Value;
        var suggestion = new Suggestion("Add/Update Class Code Section" , 
            "",
            CodeGenSectionEnum.ClassCode ,
            $@"<ClassCode Type=""ClassCode.ClassTypeEnum.{classModifier.ToTitleCase()}"" {accessModifier.WithWrappers(@"Accessor=""",@"""")}" +
            $"{(statusModifier.Equals("partial") ? @" IsPartial=""true""" : "")} " +
            $"{(statusModifier.Equals("static") ? @" IsStatic=""true""" : "")}  >",
            SuggestionActionTypeEnum.Update
        );
        suggestions.Add(suggestion);

        return suggestions;
	}

	private List<Suggestion> NamespaceSuggestions()
	{
		List<Suggestion> suggestions = [];

		var @namespace = Content.TrimEnd(';').RemoveBeforeAndIncluding("namespace").Trim();
		var msFullName = GeneratorContext.Module.Microservice.Fullname;
		if (@namespace.StartsWith(msFullName))
		{
			var moduleTypes = Enum.GetValues<ModuleTypeEnum>();
			foreach (var moduleTyp in moduleTypes)
			{
				var moduleTypeName = moduleTyp.ToString().Replace("_", ".");
				var module = $"{msFullName}.{moduleTypeName}";
				if (@namespace.StartsWith(module))
				{
					var postfix = @namespace.RemoveBeforeAndIncluding(module).Trim('.').WithWrappers(" PostFix=\"", "\"");
					var cmd = @$"<NamespaceCode {postfix} />";
					if (!generatorContext.GenCodeContains(cmd))
						suggestions.Add(
							new Suggestion(
							"Add/Update NamespaceCode Section", cmd,
							CodeGenSectionEnum.NamespaceCode,
							cmd , 
							SuggestionActionTypeEnum.Update
							));
					else
					{
						return suggestions;
					}
				}
			}
		}


		return suggestions;
	}

	private List<Suggestion> UsingSuggestions()
	{
		List<Suggestion> suggestions = [];
		var @namespace = Content.TrimEnd(';').RemoveBeforeAndIncluding("using").Trim();

		var msFullName = GeneratorContext.Module.Microservice.Fullname;
		if (@namespace.StartsWith(msFullName))
		{
			var moduleTypes = Enum.GetValues<ModuleTypeEnum>();
			foreach (var moduleTyp in moduleTypes)
			{
				var moduleTypeName = moduleTyp.ToString().Replace("_", ".");
				var module = $"{msFullName}.{moduleTypeName}";
				if (@namespace.StartsWith(module))
				{
					var postfix = @namespace.RemoveBeforeAndIncluding(module).Trim('.').WithWrappers(" PostFix=\"" , "\"");
					var cmd = @$"<UsingCodeItem Type=""ModuleTypeEnum.{moduleTyp}""{postfix}/>";
					if(!generatorContext.GenCodeContains(cmd))
						suggestions.Add(new Suggestion(
							"Add UsingCodeItem to Usings Section", 
							$@"Type=""ModuleTypeEnum.{moduleTyp}""{postfix}",
							CodeGenSectionEnum.UsingCode,
							cmd
						));
					else
						return suggestions;
				}
			}
		}

		if (!generatorContext.GenCodeContains(Content))
			suggestions.Add(new Suggestion("Add to Usings", @namespace, CodeGenSectionEnum.UsingCode , Content));

		return suggestions;
	}

	public void DoSuggestion(Suggestion suggestion)
	{
		GeneratorContext.DoCommand(suggestion.Section , suggestion.Code , suggestion.ActionType);
		suggestion.Complete = true;
	}
}

public class Suggestion(string title, string parameter, CodeGenSectionEnum section, string content , SuggestionActionTypeEnum actionType = SuggestionActionTypeEnum.Insert)
{
	public string Title { get; } = title;
	public string Parameter { get; } = parameter;
	public CodeGenSectionEnum Section { get; } = section;

	public string Code { get; } = content;
	public SuggestionActionTypeEnum ActionType { get; } = actionType;
	public bool Complete { get; set; }
}

public enum SuggestionActionTypeEnum { Insert , Update }