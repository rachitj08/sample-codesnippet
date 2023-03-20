using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentIntentRequestValidator : AbstractValidator<PaymentService.PaymentIntentModel.PaymentIntentRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public PaymentIntentRequestValidator()
        {
            RuleFor(x => x.ConnectedAccountId).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
            RuleFor(x => x.ConnectedAccountId).NotEmpty();
            RuleFor(x => x.PaymentMethodId).NotEmpty();
            RuleFor(x => x.RestaurantApplicationFeeAmount).NotEmpty();
            RuleFor(x => x.RestaurantSeoFriendlyName).NotEmpty();
           
        }
    }
}
