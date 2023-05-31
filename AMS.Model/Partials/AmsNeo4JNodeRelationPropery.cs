using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Olive;
using QOQNOS.Core;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeRelationProperty : IHaveId<long>
    {
        [JsonIgnore]
        [ForeignKey("RelationId")]
        public AmsNeo4JNodeRelationType RelType { get; set; }

        [JsonIgnore]
        [NotMapped]
        public IEnumerable<AmsNeo4JNodeConstraint> Constraints => RelType.Constraints.Where(x => x.Over == Name);

        [JsonIgnore]
        [NotMapped]
        public IEnumerable<AmsNeo4JNodeIndex> Indices => RelType.Indices.Where(x => x.Over == Name);

        public override string ToString()
        {
            return $"{Name}{DisplayName.WithWrappers(" (", ")")}";
        }
    }
    public class AmsNeo4JNodeRelationPropertyValidator : AbstractValidator<AmsNeo4JNodeRelationProperty>
    {
        public AmsNeo4JNodeRelationPropertyValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.DataType).NotNull().NotEmpty();
        }
    }

}
