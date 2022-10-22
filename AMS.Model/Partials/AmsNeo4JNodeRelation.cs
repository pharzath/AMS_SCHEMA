using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS_SCHEMA.Class;
using Olive;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeRelation
    {
        [NotMapped] public bool IsEn { get; set; }

        [ForeignKey("FromFk")]
        public AmsNeo4JNodeLabel? From { get; set; }

        [ForeignKey("TypeFk")]
        public AmsNeo4JNodeRelationType RelType { get; set; }

        [ForeignKey("ToFk")]
        public AmsNeo4JNodeLabel? To { get; set; }

        public string GetRelationshiptClassName()
        {
            return $"{GetRelationFromFieldName()}{Name.ToPascalCase()}{GetRelationToFieldName()}Relationship";
        }

        public string GetRelationFileName()
        {
            return $"{From.Name}.{Name.ToPascalCase()}.{To.Name}.Relationship";
        }

        public string GetRelationFromFieldName()
        {
            return From?.Name;
        }

        public string GetRelationToFieldName()
        {
            return (From?.Name == To?.Name ? $"Sub{To?.Name}"
                :
                To?.Name)!;
        }
        public string GetRelationFrom_InversCollectionName(IEnumerable<AmsNeo4JNodeRelation> tos)
        {
            if (tos.Count(x => x.To.Id == To.Id) > 1)
            {
                return $"{GetRelationFromFieldName().ToPlural()}_{RelType.Name}";
            }
            return GetRelationFromFieldName().ToPlural();
        }
        public string GetRelationTo_CollectionName(IEnumerable<AmsNeo4JNodeRelation> tos)
        {
            if (tos.Count(x => x.To.Id == To.Id) > 1)
            {
                return $"{RelType.Name}_{GetRelationToFieldName().ToPlural()}";
            }
            return GetRelationToFieldName().ToPlural();
        }

        //--------------

        public string GetRelationTo_InversCollectionName(IEnumerable<AmsNeo4JNodeRelation> tos)
        {
            if (tos.Count(x => x.From.Id == From.Id) > 1)
            {
                return $"{RelType.Name}_{GetRelationToFieldName().ToPlural()}";
            }
            return GetRelationToFieldName().ToPlural();
        }

        public string GetRelationFrom_CollectionName(IEnumerable<AmsNeo4JNodeRelation> tos)
        {
            if (tos.Count(x => x.From.Id == From.Id) > 1)
            {
                return $"{GetRelationFromFieldName().ToPlural()}_{RelType.Name}";
            }
            return GetRelationFromFieldName().ToPlural();
        }

        public string GetRelationToPropertyName()
        {
            var x = (From?.Name == To?.Name ?
                "Sub" + To?.Name  :
                To?.Name)!;
            var pascalCase = Name?.ToPascalCase();
            var plural = x.ToPlural();
            if (x.Equals("Person", StringComparison.OrdinalIgnoreCase))
                plural = x;
            return pascalCase + plural;
        }


        public string GetCypherDefinition()
        {
            return $"({From.Name})-[{RelType.Name}]->({To.Name})";
        }
        public string GetCypherDefinitionByVars()
        {
            var v1 = From.Name.ToShortVariableName();
            var v2 = To.Name.ToShortVariableName();
            if (v1 == v2)
            {
                v1 += "1";
                v2 += "2";
            }
            return $"({v1}:{From.Name})-[{RelType.Name.ToShortVariableName()}:{RelType.Name}]->({v2}:{To.Name})";
        }
    }
}
