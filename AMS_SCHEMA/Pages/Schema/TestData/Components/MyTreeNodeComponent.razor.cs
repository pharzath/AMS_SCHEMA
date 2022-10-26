using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Pages.Schema.Node;
using AMS_SCHEMA.Pages.Schema.Relation;
using AMS_SCHEMA.Pages.Schema.TestData.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Neo4jClient;

namespace AMS_SCHEMA.Pages.Schema.TestData.Components
{
    public partial class MyTreeNodeComponent
    {
        [Inject] ISnackbar Snackbar { get; set; }
        [Parameter]
        public MyTreeNodeComponent ParentComponent { get; set; }

        [Parameter]
        public int NodeIndent { get; set; } = 20;

        [Inject] MyCustomTreeService CustomTreeService { get; set; }
        [Inject] DataService DataService { get; set; }

        readonly HashSet<MyNode> _nodes = new HashSet<MyNode>();


        [Parameter]
        public MyNode context { get; set; }

        public void RefreshState()
        {
            StateHasChanged();
        }

        string GetRelIcon(MyNode node)
        {
            if (node.ParentNode == null) return "";
            if (node.Direction == RelationshipDirection.Outgoing)
                return Icons.Filled.SubdirectoryArrowRight;
            return Icons.Filled.TurnSharpLeft;

        }

        async Task MoveNodeClick(MyNode node, int upDown = -1)
        {
            await CustomTreeService.MoveNodeUpDown(node,upDown);
            ParentComponent.RefreshState();
        }

        async Task UpdateOrdersClick(MyNode node)
        {
            await CustomTreeService.UpdateOrders(node);
            RefreshState();

        }

        async Task OpenChild(MyNode node)
        {
            node.Nodes.Clear();
            node.Nodes = await CustomTreeService.GetChildNodes(node);
            node.IsExpanded = true;
        }


        [Inject]
        public IDialogService DialogService { get; set; }

        async Task EditNodeClick(MyNode myNode)
        {
            var dialogReference = DialogService.Show<DataInfoDialog>($"Label Info {myNode.LabelsText} - ({context.Entity.Title})",
                new DialogParameters()
                {
                    ["context"] = myNode
                },
                new DialogOptions()
                {
                    MaxWidth = MaxWidth.Small,
                    FullWidth = true
                });
            await dialogReference.Result;
        }

        async Task UpdateNodeOrderSaved(MyNode myNode)
        {
            await CustomTreeService.UpdateNodeOrderSaved(myNode);
            Snackbar.Add("Node order updated successfully.");
        }

        async Task AddNewRelatedNodeClick(MyNode myNode)
        {
            var res = DialogService.Show<NodeRelationDialog>("Label : " + myNode.LabelsText + " - " + myNode.Entity.Title,
                new DialogParameters()
                {
                    ["context"] = myNode
                },
                new DialogOptions()
                {
                    FullWidth = true,
                    MaxWidth = MaxWidth.Large
                });
            await res.Result;
            await OpenChild(myNode);
            //ParentComponent.RefreshState();
        }
    }
}
