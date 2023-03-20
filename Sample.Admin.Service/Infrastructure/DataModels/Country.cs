using System;
using System.Collections.Generic;

namespace Sample.Admin.Service.Infrastructure.DataModels
{
    public partial class Country
    {
        public short CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
