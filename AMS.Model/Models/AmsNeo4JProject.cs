using System;
using System.Collections.Generic;

namespace AMS.Model.Models;

public partial class AmsNeo4JProject
{
    public int Id { get; set; }

    public string? Name { get; set; } // QOQNOS EMS

    public string? DisplayName { get; set; } // Some Persian Text

    public required string RootPath { get; set; } // E:\\QOQNOS
    
    public required string Namespace { get; set; } // QOQNOS

    public string? Description { get; set; }
}
