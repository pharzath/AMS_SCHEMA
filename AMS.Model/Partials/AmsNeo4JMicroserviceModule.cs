using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QOQNOS.Core;
using static ServiceStack.Svg;

namespace AMS.Model;

public partial class AmsNeo4JMicroserviceModule : IHaveId<int>
{
    [ForeignKey("MicroserviceFk")]
    public AmsNeo4JMicroservice Microservice { get; set; }


    public ModuleTypeEnum ModuleType { get; set; }

    public override string ToString()
    {
        return Name;
    }


    [NotMapped]
    public string FullPath => GetFullPath();

    public DirectoryInfo RootDirectory()
    {
	    return new DirectoryInfo(GetFullPath());
    }

    public string GetFullPath()
    {
	    return Path.Combine(this.Microservice.GetFullPath(), GetFullname());
    }

    public string GetFullname()
    {
	    return $"{Microservice.Fullname}.{Name}";
    }

    public bool IsFolderExist()
    {
	    return Directory.Exists(GetFullPath());
    }
}

public enum ModuleTypeEnum
{
	//API_Interface,
	API_Endpoint,
	API_Web,
	Application,
	Contracts,
	Domain,
	Grain,
	GrainInterface,
	Infrastructure,
	Model,
	SharedKernel,
	UseCases,
	UI,
	UI_Blazor,
}

public class FileSystemItem : IHaveId<int>
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string FullPath { get; set; }
	public string RelativePath { get; set; }
	public FileSystemItemType Type { get; set; }
	public HashSet<FileSystemItem> Children { get; set; } = [];
	public bool IsExpanded { get; set; }

	public string Icon => Type == FileSystemItemType.Folder ? MudBlazor.Icons.Material.Filled.Folder : GetFileIcon();

	[NotMapped] public AmsNeo4JMicroserviceModuleItemTemplateConfig? Config { get; set; }
	[NotMapped] public FileSystemItem? Parent { get; set; }

	private string GetFileIcon()
	{
		var fileInfo = new FileInfo(FullPath);
		return fileInfo.Extension switch
		{
			".cs" => MudBlazor.Icons.Material.Filled.Numbers,
			".csproj" => MudBlazor.Icons.Material.Filled.Workspaces,
			_ => MudBlazor.Icons.Material.Filled.FilePresent
		};
	}

}

public partial class AmsNeo4JMicroserviceModuleItemTemplateConfig : IHaveId<int>
{
	public int Id { get; set; }

	public int ModuleId { get; set; }
	
	[ForeignKey(nameof(ModuleId))]
	public AmsNeo4JMicroserviceModule Module { get; set; }

	public FileSystemItem FileSystemItem { get; set; }

	//public string? RelativePath { get; set; }
	public string? GeneratorTemplatePath { get; set; }

	public bool Ignored { get; set; } = false;
	public string? NamePolicy { get; set; } // Role -> {EntityName}
	public string? FileExtension { get; set; } 
}

public enum FileSystemItemType
{
	File,
	Folder
}
