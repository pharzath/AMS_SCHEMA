using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ContentImageGallery
    {
        public int ImageGalleryId { get; set; }
        public string GalleryName { get; set; } = null!;
        public string? GalleryDescription { get; set; }
        public Guid? GalleryTeaserImage { get; set; }
    }
}
