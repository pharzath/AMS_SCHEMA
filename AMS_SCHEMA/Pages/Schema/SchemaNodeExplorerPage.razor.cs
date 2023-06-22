using System.Collections.Immutable;
using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Class;
using AMS_SCHEMA.CodeGenerator;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using AMS_SCHEMA.Pages.Schema.Node;
using AMS_SCHEMA.Pages.Schema.Search;
using Toolbelt.Blazor.HotKeys2;
using System.ComponentModel;
using Olive;
using System.Net;
using AMS.Model;
using AMS_SCHEMA.Pages.Project;
using AMS_SCHEMA.Pages.Schema.Label.CodeGen;
using AMS_SCHEMA.Pages.Schema.Relation;
using AMS_SCHEMA.Pages.Schema.TestData;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Extensions;

namespace AMS_SCHEMA.Pages.Schema;

public partial class SchemaNodeExplorerPage : IDisposable
{
    [Inject] IJsApiService JsApiService { get; set; }
    [Inject] CodeGeneratorService CodeGeneratorService { get; set; }
    [Inject] IDialogService DialogService { get; set; }

    [Inject] DataService DataService { get; set; }

    [Inject] BrowserService BrowserService { get; set; }

    [Inject] HotKeys HotKeys { get; set; }
    HotKeysContext HotKeysContext;

    List<AmsNeo4JNode>? _nodes;
    List<AmsNeo4JDepartment>? _departments;

    IEnumerable<AmsNeo4JDepartment> _selectedDepartments;
    bool _andSelectDepartment;

    IEnumerable<AmsNeo4JProject>? _projects;

    object SelectedProjectObject
    {
        get => _selectedProjectObject;
        set
        {
            _selectedProjectObject = value;
            DataService.SelectedProject = SelectedProjectObject as AmsNeo4JProject;
            DataService.ClearCach();
            SearchSchema(SearchText).Wait();
        }
    }

    AmsNeo4JProject? SelectedProject => SelectedProjectObject as AmsNeo4JProject;

    MudTextField<string?>? SearchTextField;
    MudSelect<AmsNeo4JDepartment>? DepartmentSelectorField;

    protected override void OnInitialized()
    {
        HotKeysContext = HotKeys.CreateContext()
            .Add(ModCode.None,
                Code.F1,
                GotoDepartmentSelectBox,
                "Goto Department Selector Box"
                )
             .Add(ModCode.None,
                 Code.F2,
                 GotoSearchBox,
                 "Goto Search Box")
             .Add(ModCode.None,
                 Code.Escape,
                 ClearSearchBox,
                 "close all aside panels",
                 Exclude.None)
             .Add(ModCode.None,
                 Code.Backspace,
                 GoBackFromHistory,
                 "Go Back from history")
             .Add(ModCode.None,
                 Code.Num0,
                 NavigateLabelDialog,
                 "Search for Labels")
             .Add(ModCode.Ctrl,
                 Code.Space,
                 NavigateLabelDialog,
                 "Search for Labels")
             .Add(ModCode.None,
                 Code.PageDown,
                 OpenQueryGenerator,
                 "Query Generator")
             .Add(ModCode.None,
                 Code.PageUp,
                 CloseQueryGenerator,
                 "Close Query Generator")
            ;



        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        //_departments = await DataService.GetDepartments().ToListAsync();
        _projects = await DataService.GetProjects()?.ToListAsync()!;
        await SearchSchema(null);
        await base.OnInitializedAsync();
    }

    ValueTask CloseQueryGenerator(HotKeyEntry arg)
    {
        openTop2 = false;
        StateHasChanged();
        return ValueTask.CompletedTask;
    }

    async Task HistoryBackClick()
    {
        await GoBackFromHistory(null);
    }

    ValueTask OpenQueryGenerator(HotKeyEntry arg)
    {
        openTop2 = true;
        //        StateHasChanged();
        return ValueTask.CompletedTask;
    }

    string xMatchValue => GetQueryFromHistory();


    bool IgnoreRelationNameInMatch = false;
    async Task CopyMatchToclipboardClick()
    {
        await JsApiService.CopyToClipboardAsync(xMatchValue);
    }

    async Task OpenInNeo4jClick()
    {
        var urlEncode = WebUtility.UrlEncode(xMatchValue);
        var href = "neo4j-desktop://graphapps/neo4j-browser?cmd=edit&arg=" + urlEncode;
        await JsApiService.Open(href, "_blank");
    }

    readonly LinkedList<HistoryNode> _historyNodes = new();

    string hKey = "";
    string hValue = "";

    bool busy = false;

    string GetQueryFromHistory()
    {
        //if (busy) return "";
        //busy = true;
        //return "";
        var vars = new List<string>();
        var sb = new List<string>() { "MATCH " };// new StringBuilder("MATCH ");
        var node = _historyNodes.First;
        var v = "b";

        if (hKey == _historyNodes.Count.ToString())
            return hValue;

        Console.WriteLine(nameof(GetQueryFromHistory));

        int i = 0;

        while (node != null)
        {
            if (i++ > 40)
                Console.WriteLine("Fuck I'm stuck !");

            var hn = node.Value;
            var a = hn.Relation.From!;
            var b = hn.Relation.To!;
            if (hn.Dir == RelDirection.Incoming)
            {
                (a, b) = (b, a);
            }
            var prev = node.Previous?.Value;

            string? va = null;
            if (prev == null)
            {
                va = a.Name?.ToShortVariableName();
                vars.Add(va);
            }

            var vr = "";//GetVarName(vars, node.Value.Relation.Name?.ToShortVariableName()); 
            var vb = GetVarName(vars, b.Name.ToShortVariableName());

            var an = a.Name.WithWrappers($"({va}:", ")");
            var r = hn.Relation.RelType.Name.WithWrappers($"[{vr}:", "]");
            var bn = b.Name.WithWrappers($"({vb}:", ")");
            if (prev == null)
            {
                sb.Add(an);
                sb.Add(GetDash(hn.Dir, 1));
                //sb.Add("<-");
                sb.Add(r.OnlyWhen(!IgnoreRelationNameInMatch));
                //sb.Add("-");
                sb.Add(GetDash(hn.Dir, 2));
                sb.Add(bn);
                hn.Res = b;
            }
            else
            {
                if (prev.Res.Id == a.Id)
                {
                    sb.Add(GetDash(hn.Dir, 1));
                    sb.Add(r.OnlyWhen(!IgnoreRelationNameInMatch));
                    sb.Add(GetDash(hn.Dir, 2));
                    sb.Add(bn);
                    hn.Res = b;

                }
                else if (prev.Res.Id == b.Id)
                {
                    sb.Add(GetDash(hn.Dir, 1));
                    sb.Add(r.OnlyWhen(!IgnoreRelationNameInMatch));
                    sb.Add(GetDash(hn.Dir, 2));
                    sb.Add(an);
                    hn.Res = a;
                }
                else
                {
                    var labels = DataService.GetParentLabels(prev.Res);
                    if (prev.Res.ChildLabels != null)
                        labels.AddRange(prev.Res.ChildLabels);

                    if (labels.Any(x => x.Name == a.Name))
                    {
                        Console.WriteLine("X");
                        sb.RemoveAt(sb.Count - 1);
                        var bnx = $"({vb}:{prev.Res.Name}:{a.Name})";
                        sb.Add(bnx);

                        sb.Add(GetDash(hn.Dir, 1));
                        sb.Add(r.OnlyWhen(!IgnoreRelationNameInMatch));
                        sb.Add(GetDash(hn.Dir, 2));
                        sb.Add(bn);

                        hn.Res = b;
                    }
                    else if (labels.Any(x => x.Name == b.Name))
                    {
                        Console.WriteLine("Y");
                        sb.RemoveAt(sb.Count - 1);
                        var bnx = $"({vb}:{prev.Res.Name}:{b.Name})";
                        sb.Add(bnx);

                        sb.Add(GetDash(hn.Dir, 1));
                        sb.Add(r.OnlyWhen(!IgnoreRelationNameInMatch));
                        sb.Add(GetDash(hn.Dir, 2));
                        sb.Add(an);

                        hn.Res = a;
                    }
                    else
                    {
                        sb.Add("");
                        sb.Add("     ERROR    ");
                        sb.Add($"Expected relation from {prev.Relation.From.Name} or {prev.Relation.To.Name} ");
                        sb.Add($"But are from : {a.Name} and {b.Name} !");
                        hn.Res = b;
                    }
                }
            }

            node = node.Next;


        }

        busy = false;

        sb.Add("\r\n");
        sb.Add("RETURN " + string.Join(", ", vars));
        hKey = _historyNodes.Count.ToString();
        hValue = string.Concat(sb);
        return hValue;
    }

    string? GetVarName(List<string> vars, string? v)
    {
        if (v == null)
            return null;

        var x = 0;
        var vx = v;
        while (vars.Contains(vx))
        {
            vx = v + (++x);
        }

        vars.Add(vx);
        return vx;
    }

    string GetDash(RelDirection dir, int p1)
    {
        if (dir == RelDirection.Incoming)
        {
            switch (p1)
            {
                case 1:
                    return "<-";
                case 2:
                    return "-";
            }
        }
        else
        {
            switch (p1)
            {
                case 1:
                    return "-";
                case 2:
                    return "->";
            }
        }
        return "-";
    }

    async ValueTask NavigateLabelDialog(HotKeyEntry arg)
    {
        var dialogReference = DialogService.Show<NavigateToLabelDialog>("Search Label", new DialogOptions()
        {
            FullWidth = true,
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            CloseOnEscapeKey = true
        });
        var result = await dialogReference.Result;
        var nodeName = result.Data as string;
        await BrowserService.GotoSection(nodeName);
    }

    async ValueTask GoBackFromHistory(HotKeyEntry arg)
    {
        if (!_historyNodes.Any()) return;
        _historyNodes.RemoveLast();
        var section = history.Pop();
        await BrowserService.GotoSection(section.Node.Name);
        openTop2 = history.Any();

        StateHasChanged();
    }

    async ValueTask GotoDepartmentSelectBox(HotKeyEntry arg)
    {
        openTop = true;
        StateHasChanged();
        if (DepartmentSelectorField != null)
            await DepartmentSelectorField.FocusAsync();
    }

    async ValueTask ClearSearchBox(HotKeyEntry arg)
    {
        if (SearchTextField != null)
        {
            await SearchTextField.Clear();
        }
        openTop2 = false;
        openTop = false;
        open = false;
        StateHasChanged();
    }

    async ValueTask GotoSearchBox(HotKeyEntry x)
    {
        openTop = true;
        StateHasChanged();
        if (SearchTextField != null)
        {
            await SearchTextField.SelectAsync();
            await SearchTextField.FocusAsync();
        }
    }

    string GetRelationInfoFrom(AmsNeo4JNodeRelation relation)
    {
        if (relation.IsEn)
            return $"{relation.From?.Name.Or(relation.From?.DisplayName)}";

        return $"{relation.From?.DisplayName.Or(relation.From?.Name)}";
    }

    string GetRelationInfoTo(AmsNeo4JNodeRelation relation)
    {
        if (relation.IsEn)
            return $"{relation.To?.Name.Or(relation.To?.DisplayName)}";

        return $"{relation.To?.DisplayName.Or(relation.To?.Name)}";
    }

    string GetRelationInfo(AmsNeo4JNodeRelation relation)
    {
        if (relation.IsEn)
            return $"{relation.RelType?.Name}";

        return $"{relation.RelType?.DisplayName}";
    }

    Task EditNodeClick(AmsNeo4JNode node)
    {
        return OpenNodeEditDialog(node, null);
    }

    async Task OpenNodeEditDialog(AmsNeo4JNode node, AmsNeo4JNodeLabel? label)
    {
        var reference = await DialogService.ShowAsync<NodeInfoDialog>($"Node: {node.Name} - {node.DisplayName}", new DialogParameters()
        {
            //["Node"] = node,
            ["SelectedLabel"] = label
        }, new DialogOptions()
        {
            FullScreen = true,
            CloseButton = true
        });
        var referenceResult = await reference.Result;

        await SearchSchema(SearchText);

        StateHasChanged();

    }

    public string? SearchText { get; set; }

    public string SortOrder { get; set; } = "Custom";

    readonly object _myLockObj = 0;
    async Task SearchSchema(string? searchText)
    {
        IQueryable<AmsNeo4JNode> z;
        lock (_myLockObj)
        {
            var x = DataService
                .GetNodes(searchText, SelectedDepartments, AndSelectDepartment);
            z = SortOrder switch
            {
                "Custom" => x.OrderBy(keySelector: x => x.DisplayOrder),
                "Alphabetical" => x.OrderBy(keySelector: x => x.Label.Name),
                _ => x
            };
            
        }
        _nodes = await z.ToListAsync();
    }

    public IEnumerable<AmsNeo4JDepartment> SelectedDepartments
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


    static List<string> GetPropertyTooltipText(AmsNeo4JNodeLabel label,
                                               AmsNeo4JNodeLabelProperty prop)
    {
        var tooltip = new List<string>
        {
            $"{prop.DisplayName}",
            prop.Description
        };
        var constraint = Enumerable.FirstOrDefault(label.Constraints, x => x.Over == prop.Name);
        var index = Enumerable.FirstOrDefault(label.Indices, x => x.Over == prop.Name);
        if (constraint is { })
            tooltip.Add("with CONSTRAINT :" + constraint.Command);
        if (index is { })
            tooltip.Add("with INDEX :" + index.Command);

        return tooltip;
    }

    async Task CreateNewNodeClick()
    {
        var node = AmsNeo4JNode.CreateNewNode("New Node");
        await DataService.SaveNodeAsync(node);

        var label = AmsNeo4JNodeLabel.CreateNewLabel(node);
        await DataService.SaveLabel(label);

foreach (var n in _nodes)
{
    n.DisplayOrder++;
    await DataService.SaveNodeAsync(n);
}

        await OpenNodeEditDialog(node, label);

        await SearchSchema(SearchText);
        StateHasChanged();
    }


    async Task LabelClick(AmsNeo4JNodeLabel label)
    {
        await OpenNodeEditDialog(label.Node, label);
    }

    public void Dispose()
    {
        this.HotKeysContext.Dispose();
    }

    Stack<HistoryNode> history = new Stack<HistoryNode>();

    public class HistoryNode
    {
        AmsNeo4JNodeLabel _res;
        public AmsNeo4JNode Node { get; set; }
        public AmsNeo4JNodeRelation Relation { get; set; }
        public RelDirection Dir { get; set; }

        public AmsNeo4JNodeLabel Res
        {
            get => _res;
            set
            {
                _res = value;
                Console.WriteLine(_res.Name);
            }
        }
    }

    public enum RelDirection
    {
        [Description("<-")]
        Incoming,
        [Description("->")]
        Outgoing
    }

    bool open = false;
    bool openTop = false;
    bool openTop2 = false;
    object _selectedProjectObject;

    async Task GotoLabelClick(MouseEventArgs eventArgs, AmsNeo4JNode fromNode, AmsNeo4JNodeLabel to, AmsNeo4JNodeRelation rel, RelDirection dir)
    {
        Console.WriteLine("--------------------------");
        if (eventArgs.CtrlKey)
        {


            var item = new HistoryNode()
            {
                Node = fromNode,
                Relation = rel,
                Dir = dir
            };
            history.Push(item);

            _historyNodes.AddLast(item);
            openTop2 = history.Any();
        }

        await BrowserService.GotoSection(to.Node.Name);

    }


    [Inject] ISnackbar Snackbar { get; set; }

    async Task GenerateAll()
    {
        var dialogRef = await DialogService.ShowEx<SystemCodeGeneratorDialog>("Generate", dialog =>
        {

        },
         ex =>
        {
            ex.DragMode = MudDialogDragMode.Simple;
            ex.MaxWidth = MaxWidth.Large;
            ex.FullHeight = true;
            ex.Resizeable = true;
            ex.MaximizeButton = true;
            //            ex.FullWidth = true;
        });

    }

    void GenerateEntityRelatedFilesClick(AmsNeo4JNode context)
    {
        CodeGeneratorService.GenerateEntityRelatedFiles(context);
        Snackbar.Add("Generate completed ", Severity.Success);
    }

    async Task OpenDataSampleDialogClick(AmsNeo4JNodeLabel context)
    {
        var reference = DialogService.Show<NodeSampleDataDialog>("Sample Data",
            new DialogParameters()
            {
                ["Label"] = context
            },
            new DialogOptions()
            {
                FullWidth = true,
                MaxWidth = MaxWidth.Large
            });
        await reference.Result;
    }

    void ClearCachClick()
    {
        DataService.ClearCach();
    }

    string GetReferenceLabelColor(AmsNeo4JNodeLabel label)
    {
        var referenceLabelColor = $"background-color: {label.Color}".OnlyWhen(!label.Color.IsEmpty());
        if (referenceLabelColor.IsEmpty())
            referenceLabelColor = "background: " + (label.ParentLabelId == null ? Colors.Indigo.Lighten1 : Colors.DeepPurple.Lighten3);
        return referenceLabelColor;
    }

    void NewProjectClick()
    {
        var project = new AmsNeo4JProject
        {
            Name = "New Project",
            RootPath = "E:\\QOQNOS",
            Namespace = "QOQNOS"
        };
        OpenProjectDialog(project);
    }
    void EditProjectClick(AmsNeo4JProject project)
    {
        OpenProjectDialog(project);
    }

    void OpenProjectDialog(AmsNeo4JProject project)
    {
        DialogService.Show<ProjectDialog>("Project",
            new DialogParameters()
            {
                ["Project"] = project
            },
            new DialogOptions()
            {
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            });

    }
    void StateChangedClick()
    {
        StateHasChanged();
    }

    void SortOrderChanged(object arg)
    {
        Snackbar.Add("Sort Order changed");
    }

    async Task MoveNodeUpClick(AmsNeo4JNode node)
    {
        var i = _nodes!.IndexOf(node);
        if(i == 0)
            return;
        var nodeOrder = node.DisplayOrder;
        var nextNodeOrder = _nodes[i - 1].DisplayOrder;
        node.DisplayOrder = nextNodeOrder;
        _nodes[i - 1].DisplayOrder = nodeOrder;
        await DataService.SaveNodeAsync(node);
        await DataService.SaveNodeAsync(_nodes[i - 1]);
        await SearchSchema(SearchText);
    }

    async Task MoveNodeDownClick(AmsNeo4JNode node)
    {
        var i = _nodes!.IndexOf(node);
        if (i == _nodes.Count-1)
            return;
        var nodeOrder = node.DisplayOrder;
        var nextNodeOrder = _nodes[i + 1].DisplayOrder;
        node.DisplayOrder = nextNodeOrder;
        _nodes[i + 1].DisplayOrder = nodeOrder;
        await DataService.SaveNodeAsync(node);
        await DataService.SaveNodeAsync(_nodes[i + 1]);
        await SearchSchema(SearchText);

    }
    async Task MoveNodeLastClick(AmsNeo4JNode node)
    {

        for (var i = _nodes!.Count(x => x.DisplayOrder >= node.DisplayOrder) - 1; i >= 0; i--)
        {
            await MoveNodeDownClick(node);
        }
        await SearchSchema(SearchText);

    }

}