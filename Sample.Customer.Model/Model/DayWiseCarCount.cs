using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class DayWiseCarCountVM
    {
        public List<DayWiseIncomingCount> DayWiseIncomingCounts { get; set; }
        public List<DayWiseOutgoingCount> DayWiseOutgoingCounts { get; set; }
    }
    public class DayWiseIncomingCount
    {
        public DateTime ReservationDate { get; set; }
        public int TotalCount { get; set; }
    }
    public class DayWiseOutgoingCount
    {
        public DateTime ReservationDate { get; set; }
        public int TotalCount { get; set; }
    }
}
