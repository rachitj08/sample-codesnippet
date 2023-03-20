using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdatePaymentIntentRequestValidator : AbstractValidator<PaymentService.PaymentIntentModel.UpdatePaymentIntentRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public UpdatePaymentIntentRequestValidator()
        {
            RuleFor(x => x.PaymentIntentId).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
            
        }
    }
}
