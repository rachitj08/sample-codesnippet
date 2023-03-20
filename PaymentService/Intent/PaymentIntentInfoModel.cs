using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.Intent
{
    public class PaymentIntentInfoModel
    {

        [SwaggerSchema("Stripe auto- generated Id that gets generated after user registeration on stripe and saved in Table [ASPNetUser]")]
        [Required]
        public string customerId { get; set; }


        [SwaggerSchema("Stripe auto generated Id from CreatePaymentMethod response")]
        [Required]
        public string paymentMethodId { get; set; }

        [SwaggerSchema("Stripe auto-generated Id that is stored in restaurant table after successful registertaion of restaurant on stripe")]
        public string ConnectedAccountId { get; set; }

        [SwaggerSchema("Restaurant SEO Name from table [Restaurants]")]
        public string restaurantSeoFriendlyName { get; set; }

        [SwaggerSchema("Stripe fee for processing this payment ")]
        public string restaurantApplicationFeeAmount { get; set; }

        [SwaggerSchema("User registered email")]
        public string receiptEmail { get; set; }

        [SwaggerSchema("Total amount of the order")]
        [Required]
        public long amount { get; set; }
       
    }
}
