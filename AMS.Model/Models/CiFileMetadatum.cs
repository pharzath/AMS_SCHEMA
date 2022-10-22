using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class CiFileMetadatum
    {
        public int FileMetadataId { get; set; }
        public string FileLocation { get; set; } = null!;
        public string FileHash { get; set; } = null!;
    }
}
