using System.ComponentModel.DataAnnotations.Schema;
using QOQNOS.Core;

namespace AMS.Model.Models;

public partial class AmsNeo4JDepartment : IHaveId<long>
{
    [ForeignKey(nameof(ProjectFk))]
    public AmsNeo4JProject Project { get; set; }

    public override string ToString()
    {
        return Title;
    }

    [NotMapped]
    public long Id
    {
        get => DepartmentId;
        set => DepartmentId = value;
    }
}