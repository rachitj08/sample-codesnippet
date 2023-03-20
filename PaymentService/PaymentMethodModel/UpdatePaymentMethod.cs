using PaymentService.Card;

namespace PaymentService.PaymentMethodModel
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdatePaymentMethod
    {
        public CardRequest CardInfo { get; set; }

        public PaymentMethodRequest PaymentMethodInfo { get; set; }
    }
}
