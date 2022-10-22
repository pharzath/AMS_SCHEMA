﻿using System;
using System.Collections.Generic;

namespace AMS.Model.Models
{
    public partial class ComTaxClassCountry
    {
        public int TaxClassCountryId { get; set; }
        public int TaxClassId { get; set; }
        public int CountryId { get; set; }
        public decimal TaxValue { get; set; }

        public virtual CmsCountry Country { get; set; } = null!;
        public virtual ComTaxClass TaxClass { get; set; } = null!;
    }
}
