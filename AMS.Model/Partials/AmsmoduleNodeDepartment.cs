using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model.Models
{
    public partial class AmsmoduleNodeDepartment
    {
        [ForeignKey("DepartmentId")]
        public AmsmoduleDepartment Department { get; set; }
        
        [ForeignKey("NodeId")]
        public AmsNeo4JNode Node { get; set; }

    }
}
