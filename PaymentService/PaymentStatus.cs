namespace PaymentService
{
    public class PaymentStatus
    {
        public string StripePaymentId { get; set; }
        public string StripeInvoiceId { get; set; }
        public string StripeCustomerId { get; set; }
        public string Status { get; set; }
        public string CardId { get; set; }
        public string CardType { get; set; }
        public string Message { get; set; }
    }
}
