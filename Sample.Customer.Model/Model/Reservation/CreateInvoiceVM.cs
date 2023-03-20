using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class CreateInvoiceVM
    {
        public long ReservationId { get; set; }
        public long AccountId { get; set; }
        public long UserId { get; set; }
        public bool IsCheckOut { get; set; }
        public bool IsCustomRate { get; set; }
    }
}
