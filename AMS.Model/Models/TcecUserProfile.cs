using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class TcecUserProfile
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int? MemberId { get; set; }
        public int UserId { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? LicecnceType { get; set; }
        public bool? IsStudent { get; set; }
        public string? GraduatedAtUniversity { get; set; }
        public string? LatestGraduatedLevel { get; set; }
    }
}
