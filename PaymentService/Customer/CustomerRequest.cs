using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PaymentService.Customer
{
    [SwaggerSchema("Request class with customer details")]
    public class CustomerRequest
    {
       

        public string Id { get; set; }

        [SwaggerSchema("Name of the user that is going to register on stripe")]
        public string Name { get; set; }

        [SwaggerSchema("Email of the user that is going to register on stripe")]
        [Required]
        public string Email { get; set; }

        [SwaggerSchema("Phone of the user that is going to register on stripe")]
        public string Phone { get; set; }

        [SwaggerSchema("City of the user that is going to register on stripe")]
        public string City { get; set; }

        [SwaggerSchema("Address Line1 of the user that is going to register on stripe")]
        public string Line1 { get; set; }

        [SwaggerSchema("Address Line2 of the user that is going to register on stripe")]
        public string Line2 { get; set; }

        [SwaggerSchema("State of the user that is going to register on stripe")]
        public string State { get; set; }

        [SwaggerSchema("Postal Code of the user that is going to register on stripe")]
        public string PostalCode { get; set; }

        [SwaggerSchema("Country of the user that is going to register on stripe")]
        public string Country { get; set; }


    }
}
