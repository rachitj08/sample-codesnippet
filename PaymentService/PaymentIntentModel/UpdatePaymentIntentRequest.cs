using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.PaymentIntentModel
{
    public class UpdatePaymentIntentRequest
    {
        [SwaggerSchema("Stripe auto generated Id from CreatePaymentIntent response and Saved in table [Orders]")]
        [Required]
        public string PaymentIntentId { get; set; }

        [SwaggerSchema("Updated amount for current order")]
        [Required]
        public long Amount { get; set; }

        [SwaggerSchema("Updated Stripe fee for processing this payment ")]
        [Required]
        public long ApplicationFeeAmount { get; set; }

        [SwaggerSchema("User registered email")]
        public string ReceiptEmail { get; set; }

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        [Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; }
    }
}
