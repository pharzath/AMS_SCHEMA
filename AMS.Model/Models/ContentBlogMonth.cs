using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ContentBlogMonth
    {
        public int BlogMonthId { get; set; }
        public string BlogMonthName { get; set; } = null!;
        public DateTime BlogMonthStartingDate { get; set; }
    }
}
