using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService
{
    public class StripeInfoModel
    {
        [SwaggerSchema("Abitnow userId of Card Holder, if user is registered with abitnow")]
        public string CardHolderUserId { get; set; }
      
        [SwaggerSchema("Card owner name")]
        public string CardHolderName { get; set; }

        [SwaggerSchema("Email of card owner")]
        public string CardHolderEmail { get; set; }

        [SwaggerSchema("16 digit number on card")]
        [Required]
        public string CardNumber { get; set; }

        [SwaggerSchema("Card expiration year")]
        [Required]
        public int ExpirationYear { get; set; }

        [SwaggerSchema("Card expiration month")]
        [Required]
        public int ExpirationMonth { get; set; }

        [SwaggerSchema("Card cvv number")]
        [Required]
        //[Required(ErrorMessage = "Please enter CVV number")]
        //[Display(Description = "CVVNumber details of saved card")]
        //[StringLength(3, MinimumLength = 3, ErrorMessage = "Please enter a valid CVV number")]
        public string CVVNumber { get; set; }

        [SwaggerSchema("Final amount corresponding to current order")]
        [Required]
        //[Required(ErrorMessage = "Please enter amount")]
        //[Display(Description = "Amount for current payment")]
        //[Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int Amount { get; set; }

        [SwaggerSchema("Description for current payment")]
        //[Required(ErrorMessage = "Please enter description")]
        //[Display(Description = "Description for the Payment")]
        public string PaymentDescription { get; set; }

        [SwaggerSchema("SEO friendly name of restaurant, can be fetched from table [Restaurant->SeoFriendlyName]")]
        [Required]
        public string RestaurantSeoFriendlyName { get; set; }

        [SwaggerSchema("Stripe ConnectedAccountId of restaurant, can be fetched from table [Restaurant->StripeConnectedAccountId]")]
        [Required]
        public string RestaurantConnectedAccountId { get; set; } ///= "acct_1Gdd7RFIp97ed87r";

        [SwaggerSchema("Stripe collects amount[2.9% + 30¢] per successful transaction as application fee and currently abitnow is charging a fix value for it. This value can be fetched from table [Restaurant->ConvenienceFee]")]
        [Required]
        public int RestaurantApplicationFeeAmount { get; set; } 

        [SwaggerSchema("Paramter describe whether current is a test restaurant of live , can be fetched from table [Restaurant->IsTestRestaurant]")]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; } = false;
    }
}
