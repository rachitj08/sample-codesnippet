using System;
using System.Collections.Generic;

namespace Sample.Customer.Service.Infrastructure.DataModels
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetails>();
        }

        public long InvoiceId { get; set; }
        public int InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public long AccountId { get; set; }
        public long ParkingReservationId { get; set; }
        public decimal TotalAmount { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoicePath { get; set; }

        public virtual ParkingReservation ParkingReservation { get; set; }
        public virtual ICollection<InvoiceDetails> InvoiceDetails { get; set; }
    }
}
