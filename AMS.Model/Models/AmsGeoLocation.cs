using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsGeoLocation
    {
        public int ItemId { get; set; }
        public string City { get; set; } = null!;
        public string Provience { get; set; } = null!;
        public string? Region { get; set; }
        public string? Zone { get; set; }
        public string? District { get; set; }
        public string Street { get; set; } = null!;
        public string? Allay { get; set; }
        public string No { get; set; } = null!;
        public string? ZipCode { get; set; }
        public string FullAddress { get; set; } = null!;
        public DateTime? LastModified { get; set; }
        public string Title { get; set; } = null!;
        public Guid? Guid { get; set; }
    }
}
