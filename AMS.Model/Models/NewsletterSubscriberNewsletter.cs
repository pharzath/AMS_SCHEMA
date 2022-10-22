﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class NewsletterSubscriberNewsletter
    {
        public int SubscriberId { get; set; }
        public int NewsletterId { get; set; }
        public DateTime SubscribedWhen { get; set; }
        public bool? SubscriptionApproved { get; set; }
        public DateTime? SubscriptionApprovedWhen { get; set; }
        public string? SubscriptionApprovalHash { get; set; }
        public int SubscriberNewsletterId { get; set; }

        public virtual NewsletterNewsletter Newsletter { get; set; } = null!;
        public virtual NewsletterSubscriber Subscriber { get; set; } = null!;
    }
}
