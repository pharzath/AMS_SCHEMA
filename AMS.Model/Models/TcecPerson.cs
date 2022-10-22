using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class TcecPerson
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Gender { get; set; }
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? JobTitle { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public string? Job { get; set; }
        public string? MizanTahsilat { get; set; }
        public string? MadrakTahsili { get; set; }
    }
}
