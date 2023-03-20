using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Payment.API.RequestValidators;
using PaymentService;
using PaymentService.Customer;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace PaymentAPI.Controllers
{
    /// <summary>
    /// Controller contains Api's for registering user on stripe to be a customer on stripe
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {

        #region VARIABLES       
        private readonly IPaymentServices _paymentServices;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentServices"></param>
        public CustomerController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }
        #endregion

        #region GET

        /// <summary>
        /// Fetch details of stripe customer using Stripe Customer Id
        /// </summary>
        /// <param name="CustomerId">Stripe geneated customer id of user and mapped with table [AspNetUser]</param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant] </param>
        /// <returns></returns>
        [HttpGet(), Route("GetCustomer")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Customer))]
        public IActionResult GetCustomer([SwaggerParameter(Required = true)]string CustomerId, [SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {

            var result = _paymentServices.GetCustomer(CustomerId, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });

        }

        #endregion

        #region POST

        /// <summary>
        /// Create customer on stripe 
        /// </summary>
        /// <param name="customerRequest">Customer request object</param>        
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant] </param>
        /// <returns></returns>
        [HttpPost(), Route("CreateCustomer")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Customer))]
        public IActionResult CreateCustomer([FromBody] CustomerRequest customerRequest, [SwaggerParameter("Field defines that restaurant is accepting live/test payment and Linked with table [Restaurant] ", Required = true)]bool IsTestRestaurant = false)
        {
            var results = new CustomerRequestValidator().Validate(customerRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.CreateCustomer(customerRequest, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        /// <summary>
        ///  Set a default payment method for transactions
        /// </summary>
        /// <param name="CustomerId">stripe customer Id got generated after user gets registered on Stripe</param>
        /// <param name="PaymentMethodId">Stripe payment method Id(Card Id)  generated after registering Payment method on stripe</param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant] </param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi =true)]
        [HttpPost(), Route("SetCustomersDefaultPaymentMethod")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Customer))]
        public IActionResult SetCustomersDefaultPaymentMethod([SwaggerParameter(Required = true)]string CustomerId, [SwaggerParameter(Required = true)]string PaymentMethodId, [SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {
            var result = _paymentServices.SetCustomersDefaultPaymentMethod(CustomerId, PaymentMethodId, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        #endregion

        #region PUT

        /// <summary>
        /// Update details of customer on stripe using customer Id
        /// </summary>
        /// <param name="CustomerId">Stripe geneated customer id of user and mapped with table [AspNetUser]</param>
        /// <param name="customerRequest">Customer request object</param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant] </param>        
        /// <returns></returns>
        [HttpPut, Route("UpdateCustomer")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Customer))]
        public IActionResult UpdateCustomer([SwaggerParameter("Stripe geneated customer id of user", Required = true)]string CustomerId, [FromBody]CustomerRequest customerRequest, [SwaggerParameter("Field defines that restaurant is accepting live/test payment and Linked with table [Restaurant] ", Required = true)]bool IsTestRestaurant = false)
        {

            var results = new CustomerRequestValidator().Validate(customerRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.UpdateCustomer(CustomerId, customerRequest, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }
        #endregion

        #region DELETE
        /// <summary>
        /// Delete customer from Stripe using Customer Id 
        /// </summary>
        /// <param name="CustomerId">Stripe geneated customer id of user and mapped with table [AspNetUser]</param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant] </param> 
        /// <returns></returns>
        [HttpDelete, Route("DeleteCustomer")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(bool))]
        public IActionResult DeleteCustomer([SwaggerParameter(Required = true)]string CustomerId, [SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {
            if(string.IsNullOrEmpty(CustomerId))
            {
                ModelState.AddModelError(CustomerId, "Customer Id is a required field"); 
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = ModelState });
            }

            var result = _paymentServices.DeleteCustomer(CustomerId, IsTestRestaurant);
            if (result)
                return new OkObjectResult(result);
            else
                return new BadRequestObjectResult(result);

        }
        #endregion
    }
}