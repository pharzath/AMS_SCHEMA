using AMS.Model.Models;
using AMS.Model.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AMS_SCHEMA.Pages.Schema.RelationType;

public partial class SchemaRelationExplorerPage
{
    [Inject]
    public IDialogService DialogService { get; set; }

    [Inject] DataService DataService { get; set; }

    List<AmsNeo4JNodeRelationType> _relations;
    IEnumerable<AmsmoduleDepartment> _selectedDepartments;
    bool _andSelectDepartment;

    protected override void OnInitialized()
    {
        SearchSchema(null);
        base.OnInitialized();
    }

    string GetRelationInfoFrom(AmsNeo4JNodeRelation relation)
    {
        if (relation.IsEn)
            return $"{relation.From.Name}-[{relation.Name}]->{relation.To.Name}";

        return $"{relation.From.Node.DisplayName}-[{relation.RelType.DisplayName}]->{relation.To.Node.DisplayName}";
    }

    string GetRelationInfoTo(AmsNeo4JNodeRelation relation)
    {
        if (relation.IsEn)
            return $"{relation.From.Name}-[{relation.Name}]->{relation.To.Name}";

        return $"{relation.From.Node.DisplayName}-[{relation.RelType.DisplayName}]->{relation.To.Node.DisplayName}";
    }

    Task EditNodeClick(AmsNeo4JNodeRelationType node)
    {
        return OpenRelationTypeEditDialog(node);
    }

    async Task OpenRelationTypeEditDialog(AmsNeo4JNodeRelationType context)
    {
        var reference = DialogService.Show<RelationTypeInfoDialog>($"Node: {context.Name} - {context.DisplayName}"
            , new DialogParameters()
            {
                ["RelationType"] = context
            }
            , new DialogOptions()
            {
                FullWidth = true,
                MaxWidth = MaxWidth.Large
            });
        var referenceResult = await reference.Result;
        SearchSchema(SearchText);

        StateHasChanged();

    }

    public string? SearchText { get; set; }

    void SearchSchema(string? searchText)
    {
        _relations = DataService
            .GetRelationTypes(searchText)
            .ToList();
    }

    public IEnumerable<AmsmoduleDepartment> SelectedDepartments
    {
        get => _selectedDepartments;
        set
        {
            _selectedDepartments = value;
            SearchSchema(SearchText);
        }
    }

    public bool AndSelectDepartment
    {
        get => _andSelectDepartment;
        set
        {
            _andSelectDepartment = value;
            SearchSchema(SearchText);
        }
    }

    List<string> GetPropertyTooltipText(AmsNeo4JNodeRelationType amsNeo4JNode, AmsNeo4JNodeRelationPropery prop)
    {
        var tooltip = new List<string>
        {
            $"{prop.DisplayName}",
            prop.Description
        };
        var constraint = prop.Constraints.FirstOrDefault(x => x.Over == prop.Name);
        var index = prop.Indices.FirstOrDefault(x => x.Over == prop.Name);
        if (constraint is { })
            tooltip.Add("with CONSTRAINT :" + constraint.Command);
        if (index is { })
            tooltip.Add("with INDEX :" + index.Command);

        return tooltip;
    }

    async Task CreateNewRelationTypeClick()
    {
        var relationType = AmsNeo4JNodeRelationType.CreateNewNode("RELATED");
        DataService.SaveRelationType(relationType);
        await OpenRelationTypeEditDialog(relationType);
        SearchSchema(SearchText);
        StateHasChanged();
    }

 
}