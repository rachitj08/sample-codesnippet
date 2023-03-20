using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Payment.API.RequestValidators;
using PaymentService;
using PaymentService.ConnectedAccount;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace PaymentAPI.Controllers
{
    /// <summary>
    /// Controller contains Api's for registering restaurant as connected account on stripe
    /// Note: Restaurant use Connected Account for auto payment of their share on every transaction
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectedAccountController : BaseController
    {
        #region VARIABLES       
        private readonly IPaymentServices _paymentServices;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentServices"></param>
        public ConnectedAccountController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }
        #endregion

        #region GET
        
        /// <summary>
        /// Api to get connected account details of restaurant
        /// </summary>
        /// <param name="ConnectedAccountId">Connected Id values corresponding to each restaurant mapped with table [Restaurants]</param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]</param>
        /// <returns></returns>
        [HttpGet(), Route("GetConnectedAccountDetails")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Account))]
        public IActionResult GetConnectedAccountDetails([SwaggerParameter(Required = true)]string ConnectedAccountId, [SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {
           
            var result = _paymentServices.GetConnectedAccountDetails(ConnectedAccountId, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });

        }



        /// <summary>
        /// Api to get connected account pending or incorrect details from stripe
        /// </summary>
        /// <param name="ConnectedAccountId">Connect Account Id for restaurant</param>
        /// <param name="BaseUrl">Url of your application that will handle authrization of connected account </param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]</param>
        /// <returns></returns>
        [HttpGet(), Route("GetConnectedAccountPendingInformation")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.AccountLink))]
        public IActionResult GetConnectedAccountPendingInformation([SwaggerParameter(Required = true)]string ConnectedAccountId, [SwaggerParameter(Required = false)]string BaseUrl, [SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {
            var result = _paymentServices.GetConnectedAccountPendingInformation(ConnectedAccountId, BaseUrl, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
               return new NotFoundObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = "Not found", Entity = result });

        }

        #endregion

        #region POST

        /// <summary>
        /// Api to Authorize created connected account code.
        /// </summary>
        /// <param name="connectedAccount">Authorization code that is recieved after coonected account setup on stripe </param>       
        /// <returns></returns>
        [HttpPost(), Route("AuthorizeConnectedAccount")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.OAuthToken))]
        public IActionResult AuthorizeConnectedAccount([FromBody] AuthorizeConnectedAccountRequest connectedAccount)
        {
            var results = new AuthorizeConnectedAccountRequestValidator().Validate(connectedAccount);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.AuthorizeConnectedAccount(connectedAccount.Code, connectedAccount.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }


        #endregion

        #region PUT

        /// <summary>
        /// Api to update connected account details 
        /// </summary>
        /// <param name="connectedAccount">Connected account request class</param>
        /// <returns></returns>
        [HttpPut(), Route("UpdateConnectedAccount")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Account))]
        public IActionResult UpdateConnectedAccount([FromBody]ConnectedAccount connectedAccount)
        {

            var results = new ConnectedAccountRequestValidator().Validate(connectedAccount);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.UpdateConnectedAccount(connectedAccount.ConnectedAccountId, connectedAccount.ClientIPAddress, connectedAccount.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }
        #endregion

    }
}