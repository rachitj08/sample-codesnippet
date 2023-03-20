using FluentValidation;

namespace Payment.API.RequestValidators
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateRefundRequestValidator : AbstractValidator<PaymentService.RefundModel.CreateRefundRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateRefundRequestValidator()
        {
            RuleFor(x => x.PaymentIntentId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
           
        }
    }
}
