using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class AirportsVM
    { 
        public long AirportId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long AddressId { get; set; }
        public int InTimeGapInMin { get; set; }
        public int OutTimeGapInMin { get; set; }
    }
}
