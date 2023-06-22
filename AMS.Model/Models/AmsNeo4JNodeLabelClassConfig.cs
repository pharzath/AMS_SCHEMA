using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model.Models;

//[Table("Ams_Neo4J_Node_Label_ClassConfig")]

public class AmsNeo4JNodeLabelClassConfigX
{
    public int Id { get; set; }
    public int LabelFk { get; set; }
    
    [ForeignKey(nameof(LabelFk))]
    public AmsNeo4JNodeLabel Label { get; set; }

    public bool? IsVirtual { get; set; }
    public bool? IsAbstract { get; set; }

    // Inherited from label
    public int? InheriteFromLabelFk { get; set; }

    [ForeignKey(nameof(InheriteFromLabelFk))]
    public AmsNeo4JNodeLabel? InheriteFromLabel { get; set; }
    public int? FunctionalIdFk { get; set; }
    
    [ForeignKey(nameof(FunctionalIdFk))]
    public AmsNeo4JNodeLabelFunctionalId? FunctionalId { get; set; }

}