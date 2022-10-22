using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ContentSimpleArticle
    {
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; } = null!;
        public string ArticleText { get; set; } = null!;
    }
}
