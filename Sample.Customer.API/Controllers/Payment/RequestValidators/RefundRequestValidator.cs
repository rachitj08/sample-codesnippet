using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class RefundRequestValidator : AbstractValidator<PaymentService.RefundModel.RefundRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public RefundRequestValidator()
        {
            RuleFor(x => x.RefundId).NotEmpty();
           
        }
    }
}
