using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class CardRequestValidator : AbstractValidator<PaymentService.Card.CardRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public CardRequestValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty();            
            RuleFor(x => x.ExpirationYear).NotEmpty();
            RuleFor(x => x.ExpirationMonth).NotEmpty();
            RuleFor(x => x.CVVNumber).NotEmpty();

        }
    }
}
