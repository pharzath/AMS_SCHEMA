using QOQNOS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model.Models
{
    public partial class AmsNeo4JProject : IHaveId<int>
    {
        public bool CanEdit() => true;
        public override string ToString()
        {
            return Namespace;
        }
    }
}
