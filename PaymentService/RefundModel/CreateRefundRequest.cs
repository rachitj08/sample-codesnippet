using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.RefundModel
{
    public class CreateRefundRequest
    {

        [SwaggerSchema("Stripe auto generated Id from CreatePaymentIntent response and Saved in table [Orders]")]
        [Required]
        public string PaymentIntentId { get; set; }

        [Required]
        public string OrderId { get; set; }

        [SwaggerSchema("Reason for generating a refund request")]
        [Required]
        public string Reason { get; set; }

        [SwaggerSchema("Amount for which need to generate a refund request ")]
        [Required]
        public long Amount { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool RefundApplicationFee { get; set; } = true;

        [Required]
        [DefaultValue(true)]
        public bool ReverseTransfer { get; set; } = true;

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        [Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; } = false;
    }
}
