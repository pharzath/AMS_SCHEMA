using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Olive;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeLabelPropery
    {
        [ForeignKey("LabelId")]
        public AmsNeo4JNodeLabel Label { get; set; }

        [NotMapped]
        public IEnumerable<AmsNeo4JNodeConstraint> Constraints => Label.Constraints.Where(x => x.Over == Name);

        [NotMapped]
        public IEnumerable<AmsNeo4JNodeIndex> Indices => Label.Indices.Where(x => x.Over == Name);

        public static AmsNeo4JNodeLabelPropery CreateNewPropery(AmsNeo4JNodeLabel label,
            string? name = null ,string? neo4jName = null , string? type = null, string? validationType = null)
        {
            var prop = new AmsNeo4JNodeLabelPropery()
            {
                Label = label,
                Name = name,
                Neo4jName = neo4jName,
                DataType = type,
                ValidationType = validationType
            };
            label.Properties.Add(prop);
            return prop;
        }

        public override string ToString()
        {
            return Name + DisplayName.WithWrappers(" (",")");
        }
    }

    public class AmsNeo4JNodeLabelProperyValidator : AbstractValidator<AmsNeo4JNodeLabelPropery>
    {
        public AmsNeo4JNodeLabelProperyValidator()
        {
            RuleFor(x => x.LabelId).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.DataType).NotNull().NotEmpty();
            RuleFor(x => x.ValidationType).NotNull().NotEmpty();
        }
    }
}
