using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.PaymentIntentModel
{
    public class PaymentIntentRequest
    {
        [SwaggerSchema("Stripe auto- generated Id that gets generated after user registeration on stripe and saved in Table [ASPNetUser]")]
        [Required]
        public string CustomerId { get; set; }

        [SwaggerSchema("Stripe auto generated Id from CreatePaymentMethod response")]
        [Required]
        public string PaymentMethodId { get; set; }

        [SwaggerSchema("Stripe auto-generated Id that is stored in restaurant table after successful registertaion of restaurant on stripe")]
        [Required]
        public string ConnectedAccountId { get; set; }

        [SwaggerSchema("Restaurant SEO Name from table [Restaurants]")]
        [Required]
        public string RestaurantSeoFriendlyName { get; set; }

        [SwaggerSchema("Stripe fee for processing this payment ")]
        [Required]
        public long RestaurantApplicationFeeAmount { get; set; }

        [SwaggerSchema("Total amount of the order")]
        [Required]
        public long Amount { get; set; }

        [SwaggerSchema("User registered email")]
        public string ReceiptEmail { get; set; }

        [SwaggerSchema("Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]")]
        [Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; }
    }
}
