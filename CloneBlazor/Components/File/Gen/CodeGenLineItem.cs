using ServiceStack.Html;
using System.Text.RegularExpressions;

namespace CloneBlazor.Components.File.Gen;

public class CodeGenLineItem
{
	private readonly GeneratorContext _generatorContext;
	public string? Content { get; set; }
	public CodeGenSectionEnum CodeGenSection { get; set; }
	public CodeGenSectionRoleEnum CodeGenSectionRole { get; set; }

	public Dictionary<string, string> Properties { get; } = new();
	public List<string>? AllProperties =>
		_sectionProps.TryGetValue(CodeGenSection, out var props) ? props : null;

	public CodeGenSectionEnum Scope { get; set; }
	public LineStatausEnum Status { get; private set; }

	public string Color => _sectionColor[CodeGenSection];

	readonly Dictionary<CodeGenSectionEnum, List<string>> _sectionProps = new();
	readonly Dictionary<CodeGenSectionEnum, string> _sectionColor = new()
	{
		{ CodeGenSectionEnum.None  , "black"},
		{ CodeGenSectionEnum.GeneratorCode  , "#00a67d"},
		{ CodeGenSectionEnum.UsingCode  , "#c2188b"},
		{ CodeGenSectionEnum.UsingCodeItem  , "#c2188b"},
		{ CodeGenSectionEnum.NamespaceCode  , "#47a437"},
		{ CodeGenSectionEnum.ClassCode  , "#1351ca"},
		{ CodeGenSectionEnum.ClassInheritance  , "#1351ca"},
		{ CodeGenSectionEnum.ClassContent  , "#1351ca"},
		{ CodeGenSectionEnum.RegionCode  , "#747a87"},
		{ CodeGenSectionEnum.MethodsCode  , "#d77b15"},
		{ CodeGenSectionEnum.CodeTemplate  , "#9027f4"},
	};

	private readonly LineStatausEnum _origStatus;
	private readonly string _origContent;

	public CodeGenLineItem(string line, CodeGenSectionEnum scope, LineStatausEnum status,
		GeneratorContext generatorContext)
	{
		_generatorContext = generatorContext;
		InitPropsDef();
		Content = _origContent = line;
		Content = _generatorContext.PrepareContent(Content);
		Status = _origStatus = status;
		Scope = scope;
		GetSectionIfAny(line);
	}

	private void InitPropsDef()
	{
		_sectionProps.Add(CodeGenSectionEnum.GeneratorCode, ["CodeGen", "CodeGenHints"]);
		_sectionProps.Add(CodeGenSectionEnum.NamespaceCode, ["PostFix"]);
		_sectionProps.Add(CodeGenSectionEnum.ClassCode, ["Type","Accessor"]);
		_sectionProps.Add(CodeGenSectionEnum.RegionCode, ["Name"]);
		_sectionProps.Add(CodeGenSectionEnum.CodeTemplate, ["Context"]);
	}

	public void GetSectionIfAny(string line)
	{
		var sections = Enum.GetValues<CodeGenSectionEnum>();
		foreach (var section in sections)
		{
			if (IsTag(line, section.ToString()))
			{
				CodeGenSection = section;
				CodeGenSectionRole = CodeGenSectionRoleEnum.Start;

				if (IsTagSelfClosed(line, section))
					CodeGenSectionRole = CodeGenSectionRoleEnum.SelfClosed;

				if (_sectionProps.TryGetValue(section, out var props))
				{


					var pattern = @"(\w+)=[""']?((?:.(?![""']?\s+(?:\S+)=|\s*\/?[>""']))+.)[""']?";
					var regex = new Regex(pattern);
					var matchs = regex.Matches(line);
					if (matchs.Count > 0)
					{
						foreach (Match match in matchs)
						{
							// Access the captured properties and values
							for (var i = 1; i < match.Groups.Count; i += 2)
							{
								var property = match.Groups[i].Value;
								var value = match.Groups[i + 1].Value;

								Properties.Add(property, value);
							}
						}
					}

					foreach (var prop in _sectionProps[CodeGenSection])
						Properties.TryAdd(prop, string.Empty);
				}
			}

			if (IsEndTag(line, section.ToString()))
			{
				CodeGenSection = section;
				CodeGenSectionRole = CodeGenSectionRoleEnum.End;
			}
		}
	}

	private bool IsTag(string line, string tag)
	{
		return line.Trim().StartsWith($"<{tag} ") || line.Trim().StartsWith($"<{tag}>");
	}

	private bool IsEndTag(string line)
	{
		return line.Trim().StartsWith("</");
	}

	private bool IsTagSelfClosed(string line, CodeGenSectionEnum section)
	{
		return line.Trim().Replace(" ", "").EndsWith("/>") ||
						line.Trim().Replace(" ", "").EndsWith($"</{section}>");
	}

	private bool IsEndTag(string code, string tag)
	{
		return code.Trim().Replace(" ", "").Equals($"</{tag}>");
	}

	public void Remove()
	{
		Status = LineStatausEnum.Deleted;
	}

	public void UndoRemove()
	{
		Status = _origStatus;
	}
}

public enum CodeGenSectionRoleEnum
{
	None, Start, Body, End, SelfClosed
}

public enum CodeGenSectionEnum
{
	None,
	CodeSection,
	GeneratorCode,
	UsingCode,
	UsingCodeItem,
	NamespaceCode,
	ClassCode,
	ClassInheritance,
	ClassContent,
	RegionCode,
	MethodsCode,
	CodeTemplate
}