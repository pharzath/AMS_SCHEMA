namespace CloneBlazor.Classes;

public class TreeItemData
{

	protected TreeItemData(string name, string icon, bool isExpanded = true)
	{
		Name = name;
		Icon = icon;
		IsExpanded = isExpanded;
	}

	public string Name { get; }
	public string Icon { get; set; }

	public int? Number { get; set; }

	public bool IsExpanded { get; set; }

	public HashSet<TreeItemData> TreeItems { get; set; }

}