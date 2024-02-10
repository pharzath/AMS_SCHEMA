using AMS.Model;
using AMS.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace CloneBlazor.Classes;

public class SolutionProvider
{
	private readonly MyDbContext _dbContext;
	private const string MicroservicesSolFolderName = "Microservices";
	public Solution Solution { get; }
	public SolutionProvider(string solutionFilePath, MyDbContext dbContext)
	{
		_dbContext = dbContext;
		var solName = Path.GetFileNameWithoutExtension(solutionFilePath);
		Solution = new Solution(solName, solutionFilePath);
		Solution.InitNestedProjects();
		Solution.FetchSolutionFolders();
		Solution.FetchProjects();
	}

	public void GenerateMicroservice(AmsNeo4JMicroservice ms)
	{
		var solutionFolderItem = Solution.GetSolutionFolder(ms.Name);
		if (solutionFolderItem == null)
		{
			var microservices = Solution.GetSolutionFolder(MicroservicesSolFolderName);
			if (microservices != null)
				solutionFolderItem = Solution.AddSubSolFolder(microservices, ms.Name);
		}

		foreach (var module in ms.Modules)
		{
			GenerateModule(module, solutionFolderItem);
		}

	}

	private void GenerateModule(AmsNeo4JMicroserviceModule module, SolutionFolderItem? solutionFolderItem)
	{
		var moduleDirInfo = new DirectoryInfo(module.FullPath);
		
		if (!moduleDirInfo.Exists)
		{
			moduleDirInfo.Create();
		}

		var configs = _dbContext.AmsNeo4JMicroserviceModuleItemTemplateConfigs
						.Where(x => x.ModuleId == module.Id)
						.Include(x => x.FileSystemItem)
						.ToList();
		foreach (var config in configs.OrderByDescending(x => x.FileSystemItem.Type))
		{
			switch (config.FileSystemItem.Type)
			{
				case FileSystemItemType.Folder:
					var combine = Path.Combine(module.FullPath , config.FileSystemItem.RelativePath);
					Directory.CreateDirectory(combine);
					break;
				case FileSystemItemType.File:
					GenerateFile(module , config);
					break;
			}
		}
	}

	private void GenerateFile(
		AmsNeo4JMicroserviceModule module, 
		AmsNeo4JMicroserviceModuleItemTemplateConfig config)
	{
		if (config.GeneratorTemplatePath == null)
		{
			throw new Exception($"The Generator for File {config.FileSystemItem.RelativePath} does not exist");
		}

		var genFilePath = config.GeneratorTemplatePath;
	}
}