using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.ConnectedAccount
{
    public class AuthorizeConnectedAccountRequest
    {
        [SwaggerSchema("Code fetched from Create Connected Account response")]
        [Required]
        [DefaultValue("")]
        public string Code { get; set; } = string.Empty;

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        [Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; } = false;


    }
}
