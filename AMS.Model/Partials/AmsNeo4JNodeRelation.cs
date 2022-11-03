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
    public partial class AmsNeo4JNodeRelation : IHaveId<int>
    {
        [JsonIgnore]
        [NotMapped] 
        public bool IsEn { get; set; }

        [JsonIgnore]
        [ForeignKey("FromFk")]
        public AmsNeo4JNodeLabel? From { get; set; }

        [JsonIgnore]
        [ForeignKey("TypeFk")]
        public AmsNeo4JNodeRelationType RelType { get; set; }

        [JsonIgnore]
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

        public override string ToString()
        {
            return $"{From.Name.WithWrappers("(",")")}-{RelType.Name.WithWrappers("[","]")}->{To.Name.WithWrappers("(",")")}";
        }

        public AmsNeo4JNodeLabel GetOtherSideLabel(AmsNeo4JNodeLabel selectedLabel)
        {
            return FromFk != selectedLabel.Id ? From! : To!;
        }
    }
}
