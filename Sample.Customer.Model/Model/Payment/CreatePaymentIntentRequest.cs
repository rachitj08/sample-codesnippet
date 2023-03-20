namespace Sample.Customer.Model
{
    public class CreatePaymentIntentRequest
    {
        public long AccountId { get; set; }
        public long ReservationId { get; set; }
        public long UserId { get; set; }
        public long Amount { get; set; }
        public string ReservationCode { get; set; }
    }
}
