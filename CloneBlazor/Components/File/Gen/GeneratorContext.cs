﻿using System.Text.RegularExpressions;
using AMS.Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Charts;
using Nextended.Core.Extensions;
using Olive;
using ServiceStack;

namespace CloneBlazor.Components.File.Gen;

public sealed class GeneratorContext
{
	private const string BasePath = @"D:\PROJ\AMS\AMSCHEMA\AMS_SCHEMA\AMS_SCHEMA\CODEGEN\\";
	private const string templateRazorPath = @"D:\PROJ\AMS\AMSCHEMA\AMS_SCHEMA\AMS_SCHEMA\CODEGEN\template.razor";

	public event EventHandler? Update;

	public List<CodeGenLineItem> GenLines { get; } = new();
	public string FilePath { get; set; }
	public string GeneratorPath { get; set; }
	public AmsNeo4JMicroserviceModule Module { get; set; }
	public List<SourceLineItem> SourceLines { get; set; } = new();

	private string? Entity;
	private bool? IsPlural;

	public GeneratorContext(AmsNeo4JMicroserviceModule module, FileSystemItem fsPath)
	{
		Module = module;
		FilePath = fsPath.FullPath;

		List<string> stack =
		[
			Path.GetExtension(FilePath)
		];

		var f = fsPath;
		var regex = new Regex(@"(\w+)(\{(\w+)(?:\|(\w+))?\})?(\w+)?");
		while (f != null)
		{
			if (f.Config is { NamePolicy: not null })
			{
				if (f.Config != null && f.Config.NamePolicy.OrEmpty().Equals("{Entity}", StringComparison.OrdinalIgnoreCase))
					Entity = f.Name;

				var match = regex.Match(f.Config!.NamePolicy);
				if (match.Success)
				{
					var prefix = match.Groups[1].Value;
					var entitySection = match.Groups[3].Value;
					var func = match.Groups[4].Value;
					var postfix = match.Groups[5].Value;
					if (func.Equals("Plural", StringComparison.OrdinalIgnoreCase))
					{
						entitySection = entitySection.ToPlural().ToTitleCase();
						IsPlural = true;
					}

					var fileName = $"{prefix}{entitySection}{postfix}";
					stack.Add(fileName);
				}
			}

			f = f.Parent;
		}

		stack.Reverse();
		var path = string.Join('\\',stack);


		//var tempPath = FilePath.RemoveBefore(Module.Name);

		var destTempPath = Path.Combine(BasePath, path + ".razor");

		if (!System.IO.File.Exists(destTempPath))
		{
			var directoryPath = Path.GetDirectoryName(destTempPath);
			if (directoryPath != null && !Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			var tempRazorContent = System.IO.File.ReadAllText(templateRazorPath);
			System.IO.File.WriteAllText(destTempPath, tempRazorContent);
		}

		GeneratorPath = destTempPath;
		var _lines = System.IO.File.ReadAllLines(destTempPath);
		var scopeStack = new Stack<CodeGenSectionEnum>();
		foreach (var line in _lines)
		{

			scopeStack.TryPeek(out var scope);
			CodeGenLineItem lineItem = new(line, scope, LineStatausEnum.Default);

			if (lineItem.CodeGenSectionRole == CodeGenSectionRoleEnum.Start)
				scopeStack.Push(lineItem.CodeGenSection);

			if (lineItem.CodeGenSectionRole == CodeGenSectionRoleEnum.End)
				scopeStack.TryPop(out var result);

			GenLines.Add(lineItem);
		}


		var lines = System.IO.File.ReadAllLines(FilePath);
		var lineNumber = 0;
		foreach (var line in lines)
		{
			var item = new SourceLineItem(line, ++lineNumber, this);
			SourceLines.Add(item);
		}

	}

	public void DoCommand(
		CodeGenSectionEnum suggestionSection,
		string suggestionCode,
		SuggestionActionTypeEnum suggestionActionType)
	{
		switch (suggestionActionType)
		{
			case SuggestionActionTypeEnum.Insert:
				{
					var lineItem = GenLines.FirstOrDefault(x => x.CodeGenSection == suggestionSection && x.CodeGenSectionRole == CodeGenSectionRoleEnum.End);
					if (lineItem != null)
					{
						var indexOf = GenLines.IndexOf(lineItem);
						if (indexOf >= 0)
						{
							GenLines.Insert(indexOf, new CodeGenLineItem(suggestionCode, lineItem.Scope, LineStatausEnum.Added));
							OnUpdate();
						}

					}

					break;
				}
			case SuggestionActionTypeEnum.Update:
				CodeGenLineItem? lineItem2 = GenLines.FirstOrDefault(x => x.CodeGenSection == suggestionSection);
				if (lineItem2 != null)
				{
					var indexOf = GenLines.IndexOf(lineItem2);
					DeleteLine(lineItem2);
					GenLines.Insert(indexOf, new CodeGenLineItem(suggestionCode, lineItem2.Scope, LineStatausEnum.Updated));
					OnUpdate();
				}
				else
				{
					(string, CodeGenSectionEnum)[] locations = GetDefaultLocation(suggestionSection);
					foreach (var (pos, section) in locations)
					{
						CodeGenLineItem? lineItem3 = GetLineNumber(pos, section);
						if (lineItem3 != null)
						{
							var indexOf = GenLines.IndexOf(lineItem3);
							GenLines.Insert(indexOf,
								new CodeGenLineItem(suggestionCode, lineItem3.Scope, LineStatausEnum.Added));

							OnUpdate();
							break;
						}

					}
				}
				break;
		}
	}

	private CodeGenLineItem? GetLineNumber(string pos, CodeGenSectionEnum section)
	{
		switch (pos)
		{
			case before:
				var item = GenLines.FirstOrDefault(x => x.CodeGenSectionRole is CodeGenSectionRoleEnum.Start or CodeGenSectionRoleEnum.SelfClosed &&
															  x.CodeGenSection == section);
				if (item != null)
					return GenLines[GenLines.IndexOf(item) - 1];

				break;
			case inside:
				var item2 = GenLines.FirstOrDefault(x => x.CodeGenSectionRole is CodeGenSectionRoleEnum.End &&
													 x.CodeGenSection == section);
				if (item2 != null)
					return GenLines[GenLines.IndexOf(item2) - 1];

				break;
			case after:
				var item3 = GenLines.FirstOrDefault(x => x.CodeGenSectionRole is CodeGenSectionRoleEnum.SelfClosed or CodeGenSectionRoleEnum.End &&
													  x.CodeGenSection == section);
				if (item3 != null)
					return GenLines[GenLines.IndexOf(item3) + 1];
				break;
		}
		return null;
	}

	const string before = "before";
	const string after = "after";
	const string inside = "inside";
	public (string, CodeGenSectionEnum)[] GetDefaultLocation(CodeGenSectionEnum section)
	{
		switch (section)
		{
			case CodeGenSectionEnum.None:
				break;

			case CodeGenSectionEnum.GeneratorCode:
				return [(before, CodeGenSectionEnum.CodeSection)];

			case CodeGenSectionEnum.UsingCode:
				return [(inside, CodeGenSectionEnum.GeneratorCode)];

			case CodeGenSectionEnum.NamespaceCode:
				return [(after, CodeGenSectionEnum.UsingCode),
					(before, CodeGenSectionEnum.ClassCode),
					(inside, CodeGenSectionEnum.GeneratorCode)];

			case CodeGenSectionEnum.ClassCode:
				return [(after, CodeGenSectionEnum.NamespaceCode),
					(after, CodeGenSectionEnum.UsingCode),
					(inside, CodeGenSectionEnum.GeneratorCode)];

			case CodeGenSectionEnum.RegionCode:
				return [(inside, CodeGenSectionEnum.ClassCode)];

			case CodeGenSectionEnum.MethodsCode:
				return [(inside, CodeGenSectionEnum.ClassCode)];

			case CodeGenSectionEnum.CodeTemplate:
				return [(inside, CodeGenSectionEnum.MethodsCode)];

		}

		return [];
	}

	public void OnUpdate()
	{
		Update?.Invoke(this, EventArgs.Empty);
	}

	public IEnumerable<CodeGenLineItem> GetSectionLines(CodeGenLineItem lineItem)
	{
		if (lineItem.CodeGenSectionRole == CodeGenSectionRoleEnum.Start)
		{
			yield return lineItem;
			var line = GetNextLine(lineItem);
			Stack<CodeGenSectionEnum> currentScope = new();
			var cs = lineItem.CodeGenSection;
			currentScope.Push(cs);

			while (line != null && line.Scope == cs)
			{
				yield return line;
				if (line.CodeGenSectionRole == CodeGenSectionRoleEnum.Start)
					currentScope.Push(cs = line.CodeGenSection);
				if (line.CodeGenSectionRole == CodeGenSectionRoleEnum.End)
				{
					currentScope.TryPop(out var css);
					if (cs == css)
						currentScope.TryPop(out cs);
					else
						cs = css;
				}
				line = GetNextLine(line);
			}
		}
	}

	private CodeGenLineItem? GetNextLine(CodeGenLineItem lineItem)
	{
		var indexOf = GenLines.IndexOf(lineItem);
		if (indexOf < 0) return null;

		var index = indexOf + 1;
		return index > GenLines.Count
			? null
			: GenLines[index];
	}

	public bool GenCodeContains(string cmd)
	{
		var x = GenLines.FirstOrDefault(x => x.Content == cmd);
		return x != null;
	}

	public void MoveUpLine(CodeGenLineItem lineItem)
	{
		var indexOf = GenLines.IndexOf(lineItem);
		if (indexOf <= 0) return;
		(GenLines[indexOf], GenLines[indexOf - 1]) = (GenLines[indexOf - 1], GenLines[indexOf]);
		UpdateScopes();
		OnUpdate();
	}

	private void UpdateScopes()
	{

		var scopeStack = new Stack<CodeGenSectionEnum>();
		foreach (var lineItem in GenLines)
		{

			scopeStack.TryPeek(out var scope);
			lineItem.Scope = scope;

			if (lineItem.CodeGenSectionRole == CodeGenSectionRoleEnum.Start)
				scopeStack.Push(lineItem.CodeGenSection);

			if (lineItem.CodeGenSectionRole == CodeGenSectionRoleEnum.End)
				scopeStack.TryPop(out var result);

		}

	}

	public void MoveDownLine(CodeGenLineItem lineItem)
	{
		var indexOf = GenLines.IndexOf(lineItem);
		if (indexOf == GenLines.Count) return;
		(GenLines[indexOf], GenLines[indexOf + 1]) = (GenLines[indexOf + 1], GenLines[indexOf]);
		UpdateScopes();
		OnUpdate();
	}

	public void DeleteLine(CodeGenLineItem lineItem)
	{
		GenLines.Remove(lineItem);
		OnUpdate();
	}

	public string SaveSourceGenerator()
	{
		try
		{
			System.IO.File.WriteAllLines(GeneratorPath, GenLines.Select(x => x.Content).ToArray()!);
			return $"Code Generator Template Successfully Saved to path :{GeneratorPath}";

		}
		catch (Exception e)
		{
			return $"-Faled to Saved Code Generator template to path :{GeneratorPath} Detailed Error : {e.Message}";
		}
	}

	public void DeleteMarkedLines()
	{
		GenLines.RemoveAll(item => item.Status == LineStatausEnum.Deleted);
		OnUpdate();
	}

	public void InsertNewLine(CodeGenLineItem lineItem, string beforeAfter)
	{
		var indexOf = GenLines.IndexOf(lineItem);
		switch (beforeAfter)
		{
			case "before":
				if (indexOf > 1)
					GenLines.Insert(indexOf, new CodeGenLineItem(" PLACE HOLDER", lineItem.Scope, LineStatausEnum.Added));
				break;
			case "after":
				if (indexOf < GenLines.Count)
					GenLines.Insert(indexOf + 1, new CodeGenLineItem(" PLACE HOLDER", lineItem.Scope, LineStatausEnum.Added));
				break;
		}

		UpdateScopes();
		OnUpdate();

	}

	public void CopySelectedSourceLinesHere(CodeGenLineItem lineItem)
	{
		var indexOf = GenLines.IndexOf(lineItem);
		foreach (var item in SourceLines.Where(x => x.Selected))
		{
			var itemContent = item.Content.Replace("<", "< ");
			GenLines.Insert(indexOf++, new CodeGenLineItem(itemContent, lineItem.Scope, LineStatausEnum.Added));
		}
		UpdateScopes();
		OnUpdate();
	}

	public void SelectSourceLines(int selectSourceFrom, int selectSourceTo)
	{
		for (var i = selectSourceFrom - 1; i < selectSourceTo; i++)
		{
			SourceLines[i].Selected = true;
		}
	}

	public void UnselectSourceLines()
	{
		SourceLines.ForEach(x => x.Selected = false);
	}
}

public enum LineStatausEnum
{
	Default, Added, Updated, Deleted
}