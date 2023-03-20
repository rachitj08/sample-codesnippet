using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// Validate customer request 
    /// </summary>
    public class CustomerRequestValidator : AbstractValidator<PaymentService.Customer.CustomerRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomerRequestValidator()
        {
            //RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
