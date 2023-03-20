using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class StripeInfoModelRequestValidator : AbstractValidator<PaymentService.StripeInfoModel>
    {
        /// <summary>
        /// 
        /// </summary>
        public StripeInfoModelRequestValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty();
            RuleFor(x => x.CVVNumber).NotEmpty();
            RuleFor(x => x.ExpirationYear).NotEmpty();
            RuleFor(x => x.ExpirationMonth).NotEmpty();
           
        }
    }
}
