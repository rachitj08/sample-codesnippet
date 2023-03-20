using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentService.CardScanner
{
    public class LocationCreateRequest
    {
        public string DisplayName { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
    }
}
