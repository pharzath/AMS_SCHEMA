using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ComGiftCardCouponCode
    {
        public int GiftCardCouponCodeId { get; set; }
        public string GiftCardCouponCodeCode { get; set; } = null!;
        public decimal GiftCardCouponCodeRemainingValue { get; set; }
        public int GiftCardCouponCodeGiftCardId { get; set; }
        public Guid GiftCardCouponCodeGuid { get; set; }
        public DateTime GiftCardCouponCodeLastModified { get; set; }

        public virtual ComGiftCard GiftCardCouponCodeGiftCard { get; set; } = null!;
    }
}
