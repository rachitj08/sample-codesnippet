namespace Sample.Customer.Model
{
    public class ParkingHeadsRateDetailVM
    {
        public long ParkingHeadId { get; set; }
        public long ParkingHeadsRateId { get; set; }
        public decimal Rate { get; set; }
        public decimal MaxDiscountPercentage { get; set; }
        public decimal MaxDiscountDollars { get; set; }
        public short SeqNo { get; set; }        
        public string Type { get; set; }
        public long? BasisOn { get; set; }
        public string Description { get; set; }

    }
}
