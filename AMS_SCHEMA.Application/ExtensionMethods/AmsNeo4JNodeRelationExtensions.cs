using AMS.Model.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AMS_SCHEMA.Class;
using Olive;

namespace AMS_SCHEMA.Application.ExtensionMethods
{
    public static class AmsNeo4JNodeRelationExtensions
    {
        public static string GetRelationshiptClassName(this AmsNeo4JNodeRelation rel)
        {
            return $"{rel.GetRelationFromFieldName()}{rel.Name.ToPascalCase()}{rel.GetRelationToFieldName()}Relationship";
        }

        public static string GetRelationFileName(this AmsNeo4JNodeRelation rel)
        {
            return $"{rel.From.Name}.{rel.Name.ToPascalCase()}.{rel.To.Name}.Relationship";
        }

        public static string GetRelationTo_InversCollectionName(this AmsNeo4JNodeRelation rel, IEnumerable<AmsNeo4JNodeRelation> tos)
        {
            if (tos.Count(x => x.From.Id == rel.From.Id) > 1)
            {
                return $"{rel.RelType.Name}_{rel.GetRelationToFieldName().ToPlural()}";
            }
            return rel.GetRelationToFieldName().ToPlural();
        }

        public static string GetRelationFrom_CollectionName(this AmsNeo4JNodeRelation rel, IEnumerable<AmsNeo4JNodeRelation> tos)
        {
            if (tos.Count(x => x.From.Id == rel.From.Id) > 1)
            {
                return $"{rel.GetRelationFromFieldName().ToPlural()}_{rel.RelType.Name}";
            }
            return rel.GetRelationFromFieldName().ToPlural();
        }

        public static string GetRelationToPropertyName(this AmsNeo4JNodeRelation rel)
        {
            var x = (rel.From?.Name == rel.To?.Name ?
            "Sub" + rel.To?.Name :
            rel.To?.Name)!;
            var pascalCase = rel.Name?.ToPascalCase();
            var plural = x.ToPlural();
            if (x.Equals("Person", StringComparison.OrdinalIgnoreCase))
                plural = x;
            return pascalCase + plural;
        }


        public static string GetCypherDefinition(this AmsNeo4JNodeRelation rel)
        {
            return $"({rel.From.Name})-[{rel.RelType.Name}]->({rel.To.Name})";
        }
        public static string GetCypherDefinitionByVars(this AmsNeo4JNodeRelation rel)
        {
            var v1 = rel.From.Name.ToShortVariableName();
            var v2 = rel.To.Name.ToShortVariableName();
            if (v1 == v2)
            {
                v1 += "1";
                v2 += "2";
            }
            return $"({v1}:{rel.From.Name})-[{rel.RelType.Name.ToShortVariableName()}:{rel.RelType.Name}]->({v2}:{rel.To.Name})";
        }

    }
}
