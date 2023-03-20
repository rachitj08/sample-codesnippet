using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class ShuttleBoardedListVM
    {
        public string ImagePath { get; set; }
        public string UserName { get; set; }
        public int NoOfPassengers { get; set; }
        public int NoOfBags { get; set; }
        public string BoardStatus { get; set; }
        public string Airlines { get; set; }
        public DateTime AirlineTime { get; set; }
        public string Status { get; set; }
        public string ShuttleAction { get; set; }
        public string Code { get; set; }
        public long ReservationActivityCodeId { get; set; }

    }
}
