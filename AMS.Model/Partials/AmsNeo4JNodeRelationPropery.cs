using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olive;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeRelationPropery
    {
        [ForeignKey("RelationId")]
        public AmsNeo4JNodeRelationType RelType { get; set; }

        [NotMapped]
        public IEnumerable<AmsNeo4JNodeConstraint> Constraints => RelType.Constraints.Where(x => x.Over == Name);

        [NotMapped]
        public IEnumerable<AmsNeo4JNodeIndex> Indices => RelType.Indices.Where(x => x.Over == Name);

        public override string ToString()
        {
            return $"{Name}{DisplayName.WithWrappers(" (", ")")}";
        }
    }
    public class AmsNeo4JNodeRelationProperyValidator : AbstractValidator<AmsNeo4JNodeRelationPropery>
    {
        public AmsNeo4JNodeRelationProperyValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.DataType).NotNull().NotEmpty();
        }
    }

}
