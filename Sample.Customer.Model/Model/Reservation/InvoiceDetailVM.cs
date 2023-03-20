using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Customer.Model
{
    public class InvoiceVM 
    {
        public string InvoiceNo { get; set; }
        public int InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public long ParkingReservationId { get; set; }
        public string InvoicePath { get; set; }
        public List<InvoicePriceDetailVM> InvoiceDetails { get; set; }
    }

    public class InvoicePriceDetailVM
    {
        public long ParkingHeadId { get; set; }
        public long ParkingHeadRateId { get; set; }
        public decimal Amount { get; set; }
        public short Qty { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public short SeqNo { get; set; }
    }

    public class ParkingRateReqVM
    {
        public long ParkingProviderLocationId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsCustomRate { get; set; }
        public decimal CustomRate { get; set; }
        public long ReservationId { get; set; }
    }
    public enum ParkingHeadRateEnum
    {
        ParkingFee=1,
        GovernmentRate=7,
        CustomRateType=8
    }
}
