using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model.Model
{
    public class UpdateIsParkedVM
    {
        public string ActivityCode { get; set; }
        public long ReservationId { get; set; }
        public bool IsParked { get; set; }
    }
}
