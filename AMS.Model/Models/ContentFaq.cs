using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ContentFaq
    {
        public int Faqid { get; set; }
        public string Faqquestion { get; set; } = null!;
        public string Faqanswer { get; set; } = null!;
    }
}
