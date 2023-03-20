using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Currencies
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public bool BaseCurrency { get; set; }
        public string Description { get; set; }
    }
}
