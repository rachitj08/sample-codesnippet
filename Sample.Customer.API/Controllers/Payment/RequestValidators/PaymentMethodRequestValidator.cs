using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentMethodRequestValidator : AbstractValidator<PaymentService.PaymentMethodModel.PaymentMethodRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public PaymentMethodRequestValidator()
        {
            RuleFor(x => x.PaymentMethodId).NotEmpty();
           
        }
    }
}
