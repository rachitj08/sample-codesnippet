using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.PaymentIntentModel
{
    public class PaymentIntentIdRequest
    {
        [SwaggerSchema("Stripe auto generated Id from CreatePaymentIntent response and Saved in table [Orders]")]
        [Required]
        public string PaymentIntentId { get; set; }

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        [Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; } = false;
    }
}
