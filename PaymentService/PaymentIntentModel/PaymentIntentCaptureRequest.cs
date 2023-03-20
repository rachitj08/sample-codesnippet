using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.PaymentIntentModel
{
    public class PaymentIntentCaptureRequest
    {
        [SwaggerSchema("Stripe auto generated Id from CreatePaymentMethod response")]
        [Required]
        public string PaymentIntentId { get; set; }

        [SwaggerSchema("Final amount that needs to be captured")]
        [Required]
        public long Amount { get; set; }

        [SwaggerSchema("Stripe fee for processing this payment ")]
        [Required]
        public long ApplicationFeeAmount { get; set; }
        
        [Required]
        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; } = false;
    }
}
