using AMS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QOQNOS.Core;
using System.ComponentModel.DataAnnotations;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNode : IHaveId<int>
    {
        [JsonIgnore]
        public List<AmsNeo4JNodeDepartment> Departments { get; set; }

        [JsonIgnore]
        [ForeignKey("LabelId")]
        public AmsNeo4JNodeLabel Label { get; set; }


        public static AmsNeo4JNode CreateNewNode(string name)
        {
            var node = new AmsNeo4JNode()
            {
                Name = name
            };
            
            return node;
        }
    }
}
