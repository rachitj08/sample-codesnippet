using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public short? Age { get; set; }
        public decimal? AnnualIncome { get; set; }
        public string Status { get; set; }
        public int? Country { get; set; }
    }
}
