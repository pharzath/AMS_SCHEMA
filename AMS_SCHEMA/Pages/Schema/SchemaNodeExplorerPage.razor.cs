using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Class;
using AMS_SCHEMA.CodeGenerator;
using BlazorTemplater;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Web;
using AMS_SCHEMA.Pages.Schema.Node;
using AMS_SCHEMA.Pages.Schema.Search;
using Toolbelt.Blazor.HotKeys;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;
using System.ComponentModel;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Olive;
using static AMS_SCHEMA.Pages.Schema.SchemaNodeExplorerPage;
using System.Net;
using AMS_SCHEMA.Pages.Schema.TestData;
using Neo4jClient.DataAnnotations.Cypher.Functions;

namespace AMS_SCHEMA.Pages.Schema;

public partial class SchemaNodeExplorerPage : IDisposable
{
    [Inject] IJsApiService JsApiService { get; set; }
    [Inject] AMSCodeGenerator CodeGenerator { get; set; }
    [Inject] IDialogService DialogService { get; set; }

    [Inject] DataService DataService { get; set; }

    [Inject] BrowserService BrowserService { get; set; }

    [Inject] HotKeys HotKeys { get; set; }
    HotKeysContext HotKeysContext;

    List<AmsNeo4JNode> _nodes;
    IEnumerable<AmsmoduleDepartment> _selectedDepartments;
    bool _andSelectDepartment;

    MudTextField<string?>? SearchTextField;
    MudSelect<AmsmoduleDepartment>? DepartmentSelectorField;

    protected override void OnInitialized()
    {
        this.HotKeysContext = this.HotKeys.CreateContext()
            .Add(ModKeys.None,
                Keys.F1,
                GotoDepartmentSelectBox,
                "Goto Department Selector Box")
            .Add(ModKeys.None,
                Keys.F2,
                GotoSearchBox,
                "Goto Search Box")
            .Add(ModKeys.None,
                Keys.ESC,
                ClearSearchBox,
                "close all aside panels",
                Exclude.None)
            .Add(ModKeys.None,
                Keys.Backspace,
                GoBackFromHistory,
                "Go Back from history")
            .Add(ModKeys.None,
                Keys.Num0,
                NavigateLabelDialog,
                "Search for Labels")
            .Add(ModKeys.Ctrl,
                Keys.Space,
                NavigateLabelDialog,
                "Search for Labels")
            .Add(ModKeys.None,
                Keys.PgDown,
                OpenQueryGenerator,
                "Query Generator")
            .Add(ModKeys.None,
                Keys.PgUp,
                CloseQueryGenerator,
                "Close Query Generator")
            ;

        SearchSchema(null);
        base.OnInitialized();
    }

    Task CloseQueryGenerator(HotKeyEntry arg)
    {
        openTop2 = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    async Task HistoryBackClick()
    {
        await GoBackFromHistory(null);
    }

    Task OpenQueryGenerator(HotKeyEntry arg)
    {
        openTop2 = true;
        StateHasChanged();
        return Task.CompletedTask;
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

    string GetQueryFromHistory()
    {
        var vars = new List<string>();
        var sb = new List<string>(){"MATCH "} ;// new StringBuilder("MATCH ");
        var node = _historyNodes.First;
        var v = "b";

        
        if (hKey == _historyNodes.Count.ToString())
            return hValue;

        while (node != null)
        {

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
            var r = hn.Relation.Name.WithWrappers($"[{vr}:", "]");
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

    async Task NavigateLabelDialog(HotKeyEntry arg)
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

    async Task GoBackFromHistory(HotKeyEntry arg)
    {
        _historyNodes.RemoveLast();
        var section = history.Pop();
        await BrowserService.GotoSection(section.Node.Name);
        openTop2 = history.Any();

        StateHasChanged();
    }

    async Task GotoDepartmentSelectBox(HotKeyEntry arg)
    {
        openTop = true;
        StateHasChanged();
        if (DepartmentSelectorField != null)
            await DepartmentSelectorField.FocusAsync();
    }

    async Task ClearSearchBox(HotKeyEntry arg)
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

    async Task GotoSearchBox(HotKeyEntry x)
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
            return $"{relation.From?.Name}";

        return $"{relation.From?.DisplayName}";
    }

    string GetRelationInfoTo(AmsNeo4JNodeRelation relation)
    {
        if (relation.IsEn)
            return $"{relation.To?.Name}";

        return $"{relation.To?.DisplayName}";
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
        var reference = DialogService.Show<NodeInfoDialog>($"Node: {node.Name} - {node.DisplayName}"
            , new DialogParameters()
            {
                ["Node"] = node,
                ["SelectedLabel"] = label,
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
        _nodes = DataService
            .GetNodes(searchText, SelectedDepartments, AndSelectDepartment)
            .OrderBy(x => x.Label.Name)
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


    List<string> GetPropertyTooltipText(AmsNeo4JNodeLabel label, AmsNeo4JNodeLabelPropery prop)
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
        DataService.SaveNode(node);
        var label = AmsNeo4JNodeLabel.CreateNewLabel(node);
        DataService.SaveLabel(label);

        await OpenNodeEditDialog(node, label);

        SearchSchema(SearchText);
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

    async Task GotoLabelClick(AmsNeo4JNode fromNode, AmsNeo4JNodeLabel to, AmsNeo4JNodeRelation rel, RelDirection dir)
    {
        var item = new HistoryNode()
        {
            Node = fromNode,
            Relation = rel,
            Dir = dir
        };
        history.Push(item);
        
        _historyNodes.AddLast(item);

        await BrowserService.GotoSection(to.Node.Name);
        openTop2 = history.Any();
    }


    [Inject] ISnackbar Snackbar { get; set; }
    void GenerateAll()
    {
        CodeGenerator.GenerateAll(_nodes);
        Snackbar.Add("Generate completed ", Severity.Success);
    }

    void GenerateEntityRelatedFilesClick(AmsNeo4JNode context)
    {
        CodeGenerator.GenerateEntityRelatedFiles(context);
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
            referenceLabelColor = "background: " + (label.ParentLabelId == null ?  Colors.Indigo.Lighten1 : Colors.DeepPurple.Lighten3);
        return referenceLabelColor;
    }
}