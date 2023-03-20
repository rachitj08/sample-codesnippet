using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
   
   public class AirportAddressVM
    {
        public long AirportId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Streat1 { get; set; }
        public string Streat2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        
    }
}
