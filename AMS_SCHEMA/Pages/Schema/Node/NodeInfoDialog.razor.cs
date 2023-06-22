using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Application.Services;
using AMS_SCHEMA.Class;
using AMS_SCHEMA.Pages.Schema.Label;
using AMS_SCHEMA.Pages.Schema.Label.ClassConfig;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Extensions.Components.ObjectEdit;
using MudBlazor.Extensions.Options;

namespace AMS_SCHEMA.Pages.Schema.Node
{
    public partial class NodeInfoDialog
    {
        MudTabs _tabs;

        AmsNeo4JNodeLabelTreenNode? _selectedTreeNodeLabel;
        AmsNeo4JNodeLabel _selectedLabel;
        LabelListComponent? _labelList;

        [Inject] public DataService DataService { get; set; }
        [Inject] IDialogService DialogService { get; set; }

        [CascadingParameter]
        public MudDialogInstance DialogInstance { get; set; }

        // [Parameter]
        // public AmsNeo4JNode Node { get; set; }

        [Parameter]
        public int? SelectedTabIndex{ get; set; }

        [Parameter]
        public AmsNeo4JNodeLabel SelectedLabel
        {
            get => _selectedLabel;
            set
            {
                _selectedLabel = value;
                _selectedTreeNodeLabel ??= FindNode(RootLabels, _selectedLabel?.Id);
            }
        }

        protected override void OnInitialized()
        {
            Console.Clear();
            base.OnInitialized();
        }

        AmsNeo4JNodeLabelTreenNode? _tmp;
        AmsNeo4JNodeLabelTreenNode? FindNode(HashSet<AmsNeo4JNodeLabelTreenNode> parents, long? id)
        {
            _tmp = parents.FirstOrDefault(x => x.Label.Id == id);
            if (_tmp != null)
                return _tmp;

            foreach (var parent in parents)
            {
                FindNode(parent.Items, id);
            }
            return _tmp;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (SelectedLabel.Node.Name.StartsWith("New"))
                {
                    await EditNodeClick();
                    await _labelList?.EditLabelClick(SelectedLabel)!;
                }

                if (SelectedTabIndex is not null)
                {
                    _tabs.ActivatePanel(SelectedTabIndex);
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }


        async Task EditNodeClick()
        {
            var reference = await DialogService.ShowAsync<NodeDialog>("Node",
                new DialogParameters()
                {
                    ["Node"] = SelectedLabel.Node
                },
                new DialogOptions()
                {
                    FullWidth = true,
                    MaxWidth = MaxWidth.Medium
                });
            await reference.Result;
            StateHasChanged();
        }

        void SaveAndCloseClick()
        {
            DataService.SaveNode(SelectedLabel.Node);
            DialogInstance.Close();
        }



        Task<HashSet<AmsNeo4JNodeLabelTreenNode>> LoadServerData(AmsNeo4JNodeLabelTreenNode label)
        {
            HashSet<AmsNeo4JNodeLabelTreenNode> children = DataService.GetChildLabels(label);
            return Task.FromResult(children);
        }

        public HashSet<AmsNeo4JNodeLabelTreenNode> RootLabels
        {
            get
            {
                using var ds = new DataService(new MyDbContext(), new MyCachProvider());
                var root = Utils.GetNodes(ds, SelectedLabel.Node);
                _selectedLabel ??= root.FirstOrDefault()?.Label;
                return root;
            }
        }


        async Task AddNewSubLabelClick(AmsNeo4JNodeLabelTreenNode context)
        {
            var newLabel = AmsNeo4JNodeLabel.CreateNewLabel(context.Label);

            await DataService.SaveLabel(newLabel);
            StateHasChanged();
        }


        public AmsNeo4JNodeLabelTreenNode? SelectedTreeNodeLabel
        {
            get => _selectedTreeNodeLabel;
            set
            {
                _selectedTreeNodeLabel = value;
                SelectedLabel = _selectedTreeNodeLabel?.Label;
            }
        }


        async Task ClassConfigurationClick()
        {
            var dialogReference = await DialogService.ShowAsync<ClassConfigEditorDialog>("Class Configuration" , new DialogParameters()
            {
                ["Label"] = _selectedLabel
            } , new DialogOptions()
            {
                MaxWidth = MaxWidth.Medium , 
                FullWidth = true
            });
            var result = await dialogReference.Result;
            if (result.Canceled is false)
            {
                await DataService.SaveLabel((AmsNeo4JNodeLabel)result.Data);
            }
        }
    }
}