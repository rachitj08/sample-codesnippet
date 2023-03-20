using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectedAccountRequestValidator : AbstractValidator<PaymentService.ConnectedAccount.ConnectedAccount>
    {
        /// <summary>
        /// 
        /// </summary>
        public ConnectedAccountRequestValidator()
        {
            RuleFor(x => x.ConnectedAccountId).NotEmpty();
           
        }
    }
}
