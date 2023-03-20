using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentMethodAttachRequestValidator : AbstractValidator<PaymentService.PaymentMethodModel.PaymentMethodAttachRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public PaymentMethodAttachRequestValidator()
        {
            RuleFor(x => x.PaymentMethodId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
           
        }
    }
}
