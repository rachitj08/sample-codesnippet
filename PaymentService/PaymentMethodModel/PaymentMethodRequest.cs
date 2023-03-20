using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.PaymentMethodModel
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentMethodRequest
    {
        [SwaggerSchema("Stripe auto generated Id from CreatePaymentMethod response")]
        [Required]
        public string PaymentMethodId { get; set; }

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        [Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; }
    }
}
