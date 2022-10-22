using AMS.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JNode
    {
        public List<AmsmoduleNodeDepartment> Departments { get; set; }
        
        [ForeignKey("LabelId")]
        public AmsNeo4JNodeLabel Label { get; set; }
        /*[NotMapped]
        public AmsNeo4JNodeLabel Label => Labels.FirstOrDefault();*/


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
