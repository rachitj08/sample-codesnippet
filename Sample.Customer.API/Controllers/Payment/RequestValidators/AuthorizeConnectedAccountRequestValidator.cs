using FluentValidation;

namespace Payment.API.RequestValidators
{
    
    /// <summary>
    /// PaymentServices.ConnectedAccount.AuthorizeConnectedAccountRequest
    /// </summary>
    public class AuthorizeConnectedAccountRequestValidator : AbstractValidator<PaymentService.ConnectedAccount.AuthorizeConnectedAccountRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthorizeConnectedAccountRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();           
        }
    }
}
