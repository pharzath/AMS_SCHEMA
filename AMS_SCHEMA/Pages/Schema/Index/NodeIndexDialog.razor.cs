using AMS.Model;
using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Class;


using Microsoft.AspNetCore.Components;
using MudBlazor;
using Olive;

namespace AMS_SCHEMA.Pages.Schema.Index
{
    public partial class NodeIndexDialog
    {
        [CascadingParameter]
        public MudDialogInstance DialogInstance { get; set; }

        [Inject] DataService DataService { get; set; }

        [Parameter]
        public AmsNeo4JNodeLabel Label { get; set; }


        [Parameter]
        public AmsNeo4JNodeIndex Index { get; set; }

        public bool? IfNotExist { get; set; }

        protected override void OnInitialized()
        {
            Index.OverProp = Label.Properties.FirstOrDefault(x => x.Name == Index.Over);
            base.OnInitialized();
        }

        void SaveAndCloseIndexClick()
        {
            Index.Over = Index.OverProp.Name;
            DataService.SaveIndex(Index);
            DialogInstance.Close();
        }

        Task<IEnumerable<AmsNeo4JNodeLabelProperty>> GetField(string? arg)
        {
            var formFields = Label.Properties;

            if (!string.IsNullOrEmpty(arg))
            {
                formFields = formFields?.Where(x => x.Name.Contains(arg) || x.DisplayName.Contains(arg)).ToList();
            }

            if (formFields == null) return Task.FromResult(Enumerable.Empty<AmsNeo4JNodeLabelProperty>());

            var x = formFields.ToList();
            // if (Label.ParentLabelId is null)
            //     x.AddRange(EntityBaseClassDefX.GetEntityBaseProperties());

            return Task.FromResult(x.AsEnumerable())!;

        }

        void MakeIndexCommand()
        {
            Index.Command = @$"CREATE {Index.Type} INDEX index_{Index.Label.Name}_{Index.OverProp.Neo4jName}
{@"IF NOT EXISTS
".OnlyWhen(IfNotExist is true)}FOR ({Index.Label.Name?.ToShortVariableName()}:{Index.Label.Name}) 
ON ({Index.Label.Name?.ToShortVariableName()}.{Index.OverProp.Neo4jName})";
        }

    }
}
