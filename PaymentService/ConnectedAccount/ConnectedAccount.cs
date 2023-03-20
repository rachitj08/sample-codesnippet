using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.ConnectedAccount
{
    /// <summary>
    /// Connected Account Request
    /// </summary>
    public class ConnectedAccount
    {

        
        [SwaggerSchema("Stripe auto-generated Id that is stored in restaurant table after successful registertaion of restaurant on stripe")]
        [Required]
        public string ConnectedAccountId { get; set; }

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        //[Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; } = false;

        [SwaggerSchema("IP Address of the client accessing abitnow")]
        [DefaultValue("")]
        public string ClientIPAddress { get; set; } = string.Empty;

    }
}
