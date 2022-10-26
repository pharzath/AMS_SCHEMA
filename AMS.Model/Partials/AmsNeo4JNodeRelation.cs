using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Olive;
using QOQNOS.Core;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNodeRelation : IHaveId<int>
    {
        [NotMapped] public bool IsEn { get; set; }

        [ForeignKey("FromFk")]
        public AmsNeo4JNodeLabel? From { get; set; }

        [ForeignKey("TypeFk")]
        public AmsNeo4JNodeRelationType RelType { get; set; }

        [ForeignKey("ToFk")]
        public AmsNeo4JNodeLabel? To { get; set; }


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

    }
}
