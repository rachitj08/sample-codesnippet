using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class CarDetailCountVM
    {
        public int OutgoingCarCount { get; set; }
        public int IncomingCarCount { get; set; }
        public decimal OutgoingIncreasePer { get; set; }
        public decimal OutgoingDecreasePer { get; set; }
        public decimal IncomingIncreasePer { get; set; }
        public decimal IncomingDecreasePer { get; set; }
    }
}
