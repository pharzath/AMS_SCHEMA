using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class AmsGenericContact
    {
        public int GenericContactId { get; set; }
        public int ContactTypeId { get; set; }
        public string ContactTypeTitle { get; set; } = null!;
        public string? ContactValue { get; set; }
        public string? ContactValueEx { get; set; }
        public string? ContactValueDesc { get; set; }
        public bool? ContactIsDefault { get; set; }
        public int ContactParameterId { get; set; }
        public int? PersonId { get; set; }
        public int? ContractorId { get; set; }

        public virtual AmsParameter ContactType { get; set; } = null!;
    }
}
