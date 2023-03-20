using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.DisputeModel
{
    public class DisputeUpdateRequest
    {
        [SwaggerSchema("Stripe auto- generated Id that gets generated when refund didn't get processed and moves into dispute")]
        [Required]
        public string DisputeId { get; set; }

        [SwaggerSchema("Abitnow order id from [Orders] table")]
        public string OrderId { get; set; }

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        public bool IsTestRestaurant { get; set; }
      
    }
}
