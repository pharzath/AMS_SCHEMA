using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Application.ExtensionMethods;
using AMS_SCHEMA.Pages.Schema.Node;
using AMS_SCHEMA.Pages.Schema.Relation;
using AMS_SCHEMA.Pages.Schema.TestData.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Neo4jClient;
using Newtonsoft.Json.Linq;
using QOQNOS.Core;
using QOQNOS.Neo4j.TEST.Application.AMS.Domain.Repository.Generic;

namespace AMS_SCHEMA.Pages.Schema.TestData.Components
{
    public partial class MyTreeNodeComponent
    {
        [Inject] ISnackbar Snackbar { get; set; }
        
        [Parameter]
        public MyTreeNodeComponent? ParentComponent { get; set; }

        [Parameter] public int NodeIndent { get; set; } = 20;

        [Inject] MyCustomTreeService CustomTreeService { get; set; }
        [Inject] DataService DataService { get; set; }

        [Parameter] public MyNode context { get; set; }

        [Inject] GenericRepository GR { get; set; }

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

        async Task UpdateOrdersClick()
        {
            await CustomTreeService.UpdateOrders(context);
            RefreshState();

        }

        public async Task OpenChild(MyNode node)
        {
            node.Nodes.Clear();
            node.Nodes = await CustomTreeService.GetChildNodes(node);
            node.IsExpanded = true;
        }


        [Inject]
        public IDialogService DialogService { get; set; }

        async Task EditNodeClick()
        {
            var label = context.Labels.Last();

            var dialogReference = DialogService.Show<DataInfoDialog>($"Label Info {context.LabelsText} - ({context.Entity.Title})",
                new DialogParameters()
                {
                    ["Label"] = label,
                    ["Entity"] = context.Entity
                },
                new DialogOptions()
                {
                    MaxWidth = MaxWidth.Small,
                    FullWidth = true
                });

            var result = await dialogReference.Result;
            if (result.Cancelled is false)
            {
                context.Jobj = (JObject)result.Data;
                context.Entity = context.Jobj.ToObject<EntityBase>()!;
                StateHasChanged();
            }
        }

        async Task UpdateNodeOrderSaved(MyNode myNode)
        {
            await CustomTreeService.UpdateNodeOrderSaved(myNode);
            Snackbar.Add("Node order updated successfully.");
        }

        async Task AddNewRelatedNodeClick()
        {
            var res = DialogService.Show<AddNewRelationToDataNodeDialog>("Label : " + context.LabelsText + " - " + context.Entity.Title,
                new DialogParameters
                {
                    ["Label"] = context.Labels.Last(),
                    ["Entity"] = context.Entity
                },
                new DialogOptions
                {
                    FullWidth = true,
                    MaxWidth = MaxWidth.Medium
                });
            await res.Result;

            await OpenChild(context);
            
        }

        async Task DeleteNodeAndRelatedDataNodes()
        {
            var delete = await DialogService.ShowMessageBox("Delete Data Node","Are you sure to delete this Data node record?","Yes","No");
            if (delete is true)
            {
                await GR.DeleteEntityWithIdAndAllRelations(context.Entity.Id);
                if (context.ParentNode != null)
                {
                    await ParentComponent?.OpenChild(context.ParentNode)!;
                    ParentComponent.StateHasChanged();
                }
                Snackbar.Add("Data Node deleted successfully");
                
            }
        }
    }
}
