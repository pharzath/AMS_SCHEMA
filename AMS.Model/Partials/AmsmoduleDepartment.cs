using System.ComponentModel.DataAnnotations.Schema;
using QOQNOS.Core;

namespace AMS.Model.Models;

public partial class AmsmoduleDepartment : IHaveId<int>
{
    public override string ToString()
    {
        return Title;
    }

    [NotMapped]
    public int Id
    {
        get => DepartmentId;
        set => DepartmentId = value;
    }
}