using QOQNOS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model;

public partial class AmsNeo4JMicroserviceModuleSettingDefault : IHaveId<int>
{

    [ForeignKey(nameof(MicroserviceModuleFk))]
    public AmsNeo4JMicroserviceModule Module { get; set; }

}