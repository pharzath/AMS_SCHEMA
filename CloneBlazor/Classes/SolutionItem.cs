using System.Text.RegularExpressions;
using AMS.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using MudBlazor;
using Nextended.Core.Extensions;
using static MudBlazor.CategoryTypes;

namespace CloneBlazor.Classes;

public class Solution(string name, string path)
{
	private const string SolutionFolderTypeKey = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";
	private const string ProjectTypeKey = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";

	private readonly List<string> _lines = File.ReadAllLines(path).ToList();
	public readonly SolutionRoot Root = new(name, path, Icons.Material.Filled.Light);

	public readonly List<SolutionFolderItem> _solFolders = [];

	private readonly Dictionary<string, string> _nested = [];
	public void InitNestedProjects()
	{
		var (start, end) = GetRangeNestedProjectsSection();

		var pattern = "({[^}]+}) = ({[^}]+})";
		var regex = new Regex(pattern);

		for (var i = start + 1; i < end; i++)
		{
			var line = _lines[i];
			var match = regex.Match(line);
			if (match.Success)
			{
				var child = match.Groups[1].Value;
				var parent = match.Groups[2].Value;
				_nested.Add(child, parent);
			}
		}
	}

	private (int start, int end) GetRangeNestedProjectsSection()
	{
		var start = _lines
			.Select((item, Index) => new { item, Index })
			.First(x => x.item.Contains("NestedProjects"))
			.Index;

		var end = _lines
			.Select((item, Index) => new { item, Index })
			.First(x => x.Index > start && x.item.Contains("EndGlobalSection"))
			.Index;
		return (start, end);
	}

	public void FetchSolutionFolders()
	{
		var solFolItems = _lines.Where(x => x.Contains($"{SolutionFolderTypeKey}")).ToArray();
		var pattern = @"\(\""{[^\""]*}\""\) = \""([^\""]+)\"", \""([^\""]+)\"", \""({[^\""]*})";
		var regex = new Regex(pattern);
		foreach (var item in solFolItems)
		{
			var match = regex.Match(item);
			if (!match.Success) continue;

			var solFolderName = match.Groups[1].Value;
			var solFolderId = match.Groups[3].Value;
			_solFolders.Add(new SolutionFolderItem(solFolderName, solFolderId));
		}


		foreach (var solutionFolderItem in _solFolders)
		{
			if (_nested.TryGetValue(solutionFolderItem.Id, out var parent))
			{
				var item = _solFolders.First(x => x.Id == parent);
				item.Items.Add(solutionFolderItem);
			}
			else
			{
				Root.Items.Add(solutionFolderItem);
			}
		}

	}

	public void FetchProjects()
	{
		var solFolItems = _lines.Where(x => x.Contains($"{ProjectTypeKey}")).ToArray();
		var pattern = @"\(\""{[^\""]*}\""\) = \""([^\""]+)\"", \""([^\""]+)\"", \""({[^\""]*})";
		var regex = new Regex(pattern);
		foreach (var item in solFolItems)
		{
			var match = regex.Match(item);
			if (!match.Success) continue;

			var projectName = match.Groups[1].Value;
			var projectRelPath = match.Groups[2].Value;
			var projectId = match.Groups[3].Value;
			var projAbsPath = Path.Combine(Root.FileInfo.Directory?.FullName ?? "", projectRelPath);
			ProjectItem projectItem;
			if (_nested.TryGetValue(projectId, out var parent))
			{
				var solutionFolderItem = _solFolders.First(x => x.Id == parent);
				projectItem = new ProjectItem(projectName, projAbsPath, projectId, solutionFolderItem);
				solutionFolderItem.AddProject(projectItem);
			}
			else
			{
				projectItem = new ProjectItem(projectName, projAbsPath, projectId, Root);
				Root.Items.Add(projectItem);
			}

		}
	}

	private T? GetItemById<T>(string Id, SortedSet<SolutionItem> solutionItems) where T : class, IHaveStringId
	{
		foreach (var solutionItem in solutionItems)
		{
			if (solutionItem is IHaveStringId item && item.Id == Id)
				return solutionItem as T;

			if (solutionItem.Items != null && solutionItem.Items.Any())
				return GetItemById<T>(Id, solutionItem.Items);
		}

		return null;
	}

	public SolutionFolderItem? GetSolutionFolder(string name)
	{
		return _solFolders.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
	}

	public SolutionFolderItem AddSubSolFolder(SolutionFolderItem microservices, string name)
	{
		var newSolFolder = new SolutionFolderItem(name, Guid.NewGuid().ToString("B").ToUpper());
		microservices.AddSubSolutionFolder(newSolFolder);

		WriteSolutionFolderToFile(microservices, newSolFolder);

		return newSolFolder;
	}

	private void WriteSolutionFolderToFile(SolutionFolderItem parent, SolutionFolderItem child)
	{
		var lastLine = _lines
			.Select((line, index) => new { line, index })
			.Last(x => x.line.Trim().StartsWith("EndProject"))
			.index+1;

		var l1 = $"Project(\"{SolutionFolderTypeKey}\") = \"{child.Name}\", \"{child.Name}\", \"{child.Id}\"";
		var l2 = "EndProject";
		_lines.Insert(lastLine++, l1);
		_lines.Insert(lastLine++, l2);

		var (_, end) = GetRangeNestedProjectsSection();
		var l3 = $"\t\t{child.Id} = {parent.Id}";
		_lines.Insert(end, l3);

		File.WriteAllLines(path, _lines);
	}
}

public class SolutionRoot(string name, string path, string icon, bool isExpanded = true)
	: SolutionItem(name, icon, isExpanded)
{
	public FileInfo FileInfo { get; } = new(path);
	public override SolutionItem Parent { get; set; }
	public override SortedSet<SolutionItem> Items { get; set; } = [];
}

public abstract class SolutionItem(string name, string icon, bool isExpanded = true) : IComparable<SolutionItem>
{
	public abstract SolutionItem Parent { get; set; }
	public abstract SortedSet<SolutionItem>? Items { get; set; }
	public string Name { get; } = name;
	public string Icon { get; set; } = icon;
	public bool IsExpanded { get; set; } = isExpanded;
	public object? Tag { get; set; }

	public int CompareTo(SolutionItem? other)
	{
		if (ReferenceEquals(this, other)) return 0;
		if (ReferenceEquals(null, other)) return 1;
		return string.Compare(Name, other.Name, StringComparison.Ordinal);
	}
}

public class SolutionFolderItem(string name, string id)
	: SolutionItem(name, Icons.Material.Outlined.Folder), IHaveStringId
{
	public string Id { get; set; } = id;

	public override SolutionItem? Parent { get; set; }

	public override SortedSet<SolutionItem> Items { get; set; } = [];

	public void AddProject(ProjectItem project)
	{
		Items.Add(project);
	}

	public void AddSubSolutionFolder(SolutionFolderItem solFolderItem)
	{
		Items.Add(solFolderItem);
	}
}

public sealed class ProjectItem : FolderItem, IHaveStringId
{
	public FileInfo ProjectFileInfo { get; }

	public ProjectItem(string name, string path, string id, SolutionItem parent, string icon = Icons.Material.Filled.Workspaces)
		: base(name, path, parent, icon)
	{
		ProjectFileInfo = new(path);
		Parent = parent;
		Id = id;
		IsExpanded = false;
		FetchContent();

	}

	public string Id { get; set; }

}

public class FolderItem : SolutionItem
{

	protected FolderItem(string name, string path, SolutionItem parent, string icon = Icons.Material.Filled.Folder) : base(name, icon)
	{
		if (File.Exists(path))
			path = new FileInfo(path).Directory!.FullName;

		DirectoryInfo = new(path);
		Parent = parent;
		FetchContent();
	}

	public DirectoryInfo DirectoryInfo { get; }

	public sealed override SolutionItem? Parent { get; set; }

	public override SortedSet<SolutionItem> Items { get; set; } = [];

	public void AddFolder(FolderItem folder)
	{
		Items.Add(folder);
	}

	public void AddFile(FileItem file)
	{
		Items.Add(file);
	}

	protected void FetchContent()
	{
		string[] ignoredDir = ["bin", "obj"];
		foreach (var fileInfo in DirectoryInfo.GetFiles().Where(x => x.Extension != ".csproj"))
			AddFile(new FileItem(fileInfo.Name, fileInfo.FullName, this));

		foreach (var directoryInfo in DirectoryInfo.GetDirectories().Where(x => !ignoredDir.Any(z => z == x.Name)))
			AddFolder(new FolderItem(directoryInfo.Name, directoryInfo.FullName, this));
	}
}

public class FileItem : SolutionItem
{
	public FileItem(string name, string path, SolutionItem parent) : base(name, Icons.Material.Filled.FilePresent)
	{
		FileInfo = new(path);
		Parent = parent;
		InitIcon();
		FetchContent();
	}

	private void InitIcon()
	{
		switch (FileInfo.Extension)
		{
			case ".cs":
				Icon = Icons.Material.Filled.Numbers;
				break;
			case ".csproj":
				Icon = Icons.Material.Filled.BuildCircle;
				break;
		}
	}

	public FileInfo FileInfo { get; }
	public sealed override SolutionItem Parent { get; set; }
	public override SortedSet<SolutionItem> Items { get; set; } = [];

	public void AddChildItem(SolutionItem child)
	{
		Items.Add(child);
	}

	public void FetchContent()
	{

	}
}

