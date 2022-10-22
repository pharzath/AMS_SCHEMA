using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsUserProfileResidentalComplexJoin
    {
        public int ItemId { get; set; }
        public int UserProfileId { get; set; }
        public int ResidentalComplexId { get; set; }
    }
}
