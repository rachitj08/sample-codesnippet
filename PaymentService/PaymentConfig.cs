namespace PaymentService
{
    public class PaymentConfig
    {
        public string TestSecretKey { get; set; }
        public string TestPublishableKey { get; set; }
        public string TestClientId { get; set; }
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
        public string ClientId { get; set; }
        public bool IsTestStripe { get; set; }
        public string ApplicationName { get; set; }
        public decimal ApplicationFeePercentage { get; set; }
        public long MarginAmountForPaymentIntent { get; set; }
    }
}
