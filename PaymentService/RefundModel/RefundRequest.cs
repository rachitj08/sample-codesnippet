using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.RefundModel
{
    public class RefundRequest
    {
        [SwaggerSchema("Stripe auto generated Id from CreateRefund response and Saved in table [Orders]")]
        [Required]
        public string RefundId { get; set; }

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        [Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; } = false;
    }
}
