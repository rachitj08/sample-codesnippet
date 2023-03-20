using FluentValidation;

namespace Payment.API.RequestValidators
{
    public class DisputeUpdateRequestValidator : AbstractValidator<PaymentService.DisputeModel.DisputeUpdateRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public DisputeUpdateRequestValidator()
        {
            RuleFor(x => x.DisputeId).NotEmpty();
            RuleFor(x => x.OrderId).NotEmpty();
           
        }
    }
}
