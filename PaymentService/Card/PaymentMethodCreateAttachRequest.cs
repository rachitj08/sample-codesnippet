using PaymentService.PaymentMethodModel;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.Card
{
    public class PaymentMethodCreateAttachRequest
    {
        [SwaggerSchema("Abitnow User Id of the customer ")]
        public string CardHolderUserId { get; set; }

        [SwaggerSchema("Customer Name")]
        public string CardHolderName { get; set; }

        [SwaggerSchema("Email of the card owner")]
        [Required]
        public string CardHolderEmail { get; set; }

        [SwaggerSchema("Personal name for the card with which user want's to register on stripe.")]
        public string CardPersonalName { get; set; }

        [SwaggerSchema("16 digit card number")]
        [Required]
        public string CardNumber { get; set; }

        [SwaggerSchema("Expiration year of card")]
        [Required]
        public int ExpirationYear { get; set; }

        [SwaggerSchema("Expiration month of card")]
        [Required]
        public int ExpirationMonth { get; set; }

        [SwaggerSchema("Cvv Number of card")]
        [Required]
        public string CVVNumber { get; set; }
       

        [SwaggerSchema("Stripe auto generated Id from CreateCustomer response")]
        [Required]
        public string CustomerId { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsTestRestaurant { get; set; }
    }
}
