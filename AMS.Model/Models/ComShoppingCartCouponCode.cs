﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ComShoppingCartCouponCode
    {
        public int ShoppingCartCouponCodeId { get; set; }
        public int ShoppingCartId { get; set; }
        public string CouponCode { get; set; } = null!;

        public virtual ComShoppingCart ShoppingCart { get; set; } = null!;
    }
}
