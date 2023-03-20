using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentIntentCaptureRequestValidator : AbstractValidator<PaymentService.PaymentIntentModel.PaymentIntentCaptureRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public PaymentIntentCaptureRequestValidator()
        {
            RuleFor(x => x.PaymentIntentId).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
           

        }
    }
}
