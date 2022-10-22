using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Class;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Olive;

namespace AMS_SCHEMA.Pages.Schema.RelationType.Index
{
    public partial class RelationTypeIndexDialog
    {
        [CascadingParameter]
        public MudDialogInstance DialogInstance { get; set; }

        [Inject] DataService DataService { get; set; }

        [Parameter]
        public AmsNeo4JNodeRelationType RelationType { get; set; }

        [Parameter]
        public AmsNeo4JNodeIndex Index { get; set; }

        public bool? IfNotExist { get; set; }

        protected override void OnInitialized()
        {
            Index.RelOverProp = RelationType.Properties.FirstOrDefault(x => x.Name == Index.Over);
            base.OnInitialized();
        }

        void SaveAndCloseIndexClick()
        {
            Index.Over = Index.RelOverProp.Name;
            DataService.SaveIndex(Index);
            DialogInstance.Close();
        }


        Task<IEnumerable<AmsNeo4JNodeRelationPropery>> GetField(string? arg)
        {
            var formFields = RelationType.Properties;

            if (!string.IsNullOrEmpty(arg))
            {
                formFields = formFields?.Where(x => x.Name.Contains(arg) || x.DisplayName.Contains(arg)).ToList();
            }

            if (formFields == null) return Task.FromResult(Enumerable.Empty<AmsNeo4JNodeRelationPropery>());

            var x = formFields.AsEnumerable();

            return Task.FromResult(x)!;
        }

        void MakeIndexCommand()
        {
            Index.Command = @$"CREATE {Index.Type} INDEX index_{Index.RelType.Name}_{Index.Over?.ToCamelCase()}
{@"IF NOT EXISTS
".OnlyWhen(IfNotExist is true)}FOR ()-[{Index.RelType.Name?.ToShortVariableName()}:{Index.RelType.Name}]-() 
ON ({Index.RelType.Name?.ToShortVariableName()}.{Index.Over?.ToCamelCase()})";
        }

    }
}
