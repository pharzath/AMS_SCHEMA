namespace CloneProjects;

public class CloneProjectProvider
{
	private readonly string _solutionFilePath;
	private readonly Module _sourceModule;
	private readonly string _microserviceName;

	public CloneProjectProvider(string solutionFilePath, Module sourceModule)
	{
		_solutionFilePath = solutionFilePath;
		_sourceModule = sourceModule;
	}

	public void CloneToNewModule(Module module)
	{
		module.RootPath = module.GetDestinationRelatedTo(_sourceModule);
		module.EnsureRootFolderExist();

		module.CloneCsproj(_sourceModule);
	}

}