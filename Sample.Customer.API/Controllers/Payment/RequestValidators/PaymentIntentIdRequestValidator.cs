using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentIntentIdRequestValidator : AbstractValidator<PaymentService.PaymentIntentModel.PaymentIntentIdRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public PaymentIntentIdRequestValidator()
        {
            RuleFor(x => x.PaymentIntentId).NotEmpty();
           
        }
    }
}
