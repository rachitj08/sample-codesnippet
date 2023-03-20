namespace PaymentService.Card
{
    public class CardValidationResponse
    {
        public string CardStripeVerificationToken { get; set; }
        public string CardStripeId { get; set; }
        public string CustomerId { get; set; }
        public string Brand { get; set; }
        public int Exp_Month { get; set; }
        public int Exp_Year { get; set; }
        public string Country { get; set; }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
