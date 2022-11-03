using AMS.Model;
using AMS.Model.Models;
using AMS.Model.Services;
using AMS_SCHEMA.Class;


using Microsoft.AspNetCore.Components;
using MudBlazor;
using Olive;

namespace AMS_SCHEMA.Pages.Schema.Constraint
{
    public partial class NodeConstraintDialog
    {
        [CascadingParameter]
        public MudDialogInstance DialogInstance { get; set; }

        [Inject] DataService DataService { get; set; }

        // [Parameter]
        // public AmsNeo4JNode Node { get; set; }

        [Parameter]
        public AmsNeo4JNodeLabel Label { get; set; }


        [Parameter]
        public AmsNeo4JNodeConstraint Constraint { get; set; }

        public bool? IfNotExist { get; set; }

        //form? Form;
        protected override void OnInitialized()
        {
            Constraint.OverProp = Label.Properties.FirstOrDefault(x=>x.Name == Constraint.Over);
            base.OnInitialized();
        }

        void SaveAndCloseConstraintClick()
        {
            Constraint.Over = Constraint.OverProp.Name;
            DataService.SaveConstraint(Constraint);
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

            if(Label.ParentLabelId is null)
                x.AddRange(EntityBaseClassDef.GetEntityBaseProperties());

            return Task.FromResult(x.AsEnumerable())!;

        }

        void MakeConstraintCommand()
        {
            Constraint.Command = @$"CREATE CONSTRAINT constraint_{Constraint.Label.Name}_{Constraint.OverProp.Neo4jName}_{Constraint.Type?.ToCamelCase()}
{@"IF NOT EXISTS
".OnlyWhen(IfNotExist is true)}FOR ({Constraint.Label.Name?.ToShortVariableName()}:{Constraint.Label.Name}) 
REQUIRE {Constraint.Label.Name?.ToShortVariableName()}.{Constraint.Over?.ToCamelCase()} {Constraint.Type} ";
        }

    }
}
