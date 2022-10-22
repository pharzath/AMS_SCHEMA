using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class PersonasPersonaContactHistory
    {
        public int PersonaContactHistoryId { get; set; }
        public int? PersonaContactHistoryPersonaId { get; set; }
        public DateTime PersonaContactHistoryDate { get; set; }
        public int PersonaContactHistoryContacts { get; set; }

        public virtual PersonasPersona? PersonaContactHistoryPersona { get; set; }
    }
}
