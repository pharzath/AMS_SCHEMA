using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Class;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Olive;

namespace AMS_SCHEMA.Pages.Schema.RelationType.Constraint
{
    public partial class RelationTypeConstraintDialog
    {
        [CascadingParameter]
        public MudDialogInstance DialogInstance { get; set; }

        [Inject] DataService DataService { get; set; }

        [Parameter]
        public AmsNeo4JNodeRelationType RelType { get; set; }

        [Parameter]
        public AmsNeo4JNodeConstraint Constraint { get; set; }

        public bool? IfNotExist { get; set; }

        protected override void OnInitialized()
        {
            Constraint.RelOverProp = RelType.Properties.FirstOrDefault(x => x.Name == Constraint.Over);
            base.OnInitialized();
        }

        void SaveAndCloseConstraintClick()
        {
            Constraint.RelTypeFk = RelType.Id;
            Constraint.Over = Constraint.RelOverProp.Name;

            DataService.SaveConstraint(Constraint);
            DialogInstance.Close();
        }


        Task<IEnumerable<AmsNeo4JNodeRelationPropery>> GetField(string? arg)
        {
            var formFields = RelType.Properties;

            if (!string.IsNullOrEmpty(arg))
            {
                formFields = formFields?.Where(x => x.Name.Contains(arg) || x.DisplayName.Contains(arg)).ToList();
            }

            if (formFields == null) return Task.FromResult(Enumerable.Empty<AmsNeo4JNodeRelationPropery>());

            var x = formFields.AsEnumerable();

            return Task.FromResult(x)!;

        }

        void MakeConstraintCommand()
        {
            Constraint.Command = @$"CREATE CONSTRAINT constraint_{Constraint.RelType.Name}_{Constraint.Over?.ToCamelCase()}
{@"IF NOT EXISTS
".OnlyWhen(IfNotExist is true)}FOR ()-[{RelType.Name.ToShortVariableName()}:{RelType.Name}]-() 
REQUIRE {RelType.Name?.ToShortVariableName()}.{Constraint.Over?.ToCamelCase()} {Constraint.Type} ";
        }

    }
}
