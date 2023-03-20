using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model.Reservation
{
    public class ReservationSearchVM
    {
        public string ReservaionLocationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobNo { get; set; }
        public string EmailId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SourceName { get; set; }

    }
}
