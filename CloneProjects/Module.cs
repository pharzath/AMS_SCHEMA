namespace CloneProjects;

public class Module
{
	public string Namespace { get; set; } = "QOQNOS";
	public string Microservice { get; set; } = "AUTH";
	public string ModuleName { get; set; }

	public string RootPath { get; set; }

	public DirectoryInfo RootDirectory => new(RootPath);

	public string GetModuleFullName()
	{
		return $"{Namespace}.{Microservice}.{ModuleName}";
	}

	public string GetDestinationRelatedTo(Module sourceModule)
	{
		return Path.Combine(sourceModule.RootDirectory.Parent!.FullName, Microservice.ToUpper());
	}


	public void EnsureRootFolderExist()
	{
		var dir = RootDirectory;
		if (!dir.Exists)
			dir.Create();
	}

	public DirectoryInfo EnsureModuleFolder()
	{
		var dir = RootDirectory;
		var subdir = dir.CreateSubdirectory(GetModuleFullName());
		return subdir;
	}

	public void CloneCsproj(Module sourceModule)
	{
		var moduleFolder = EnsureModuleFolder();

		var projFile = sourceModule.RootDirectory.GetFiles().First(x => x.Extension.Equals(".csproj", StringComparison.OrdinalIgnoreCase));
		var projFileLines = File.ReadAllLines(projFile.FullName);
		for (var index = 0; index < projFileLines.Length; index++)
		{
			var line = projFileLines[index];
			if (line.Contains("ProjectReference"))
			{
				if (line.Contains(sourceModule.GetModuleFullName()))
					projFileLines[index] = line.Replace(sourceModule.GetModuleFullName(), GetModuleFullName());
			}
		}

		var destFilePath = Path.Combine(moduleFolder.FullName, GetModuleFullName() + ".csproj");
		File.WriteAllLines(destFilePath, projFileLines);
	}

}