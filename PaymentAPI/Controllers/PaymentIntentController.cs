using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Payment.API.RequestValidators;
using PaymentService;
using PaymentService.PaymentIntentModel;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{
    /// <summary>
    /// Controller contains Api's for Stripe Payment Intent(Payment Request)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentIntentController : BaseController
    {

        #region VARIABLES       
        private readonly IPaymentServices _paymentServices;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentServices"></param>
        public PaymentIntentController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }
        #endregion

        #region GET


        /// <summary>
        /// Get Details of payment  using PaymentIntentId
        /// </summary>
        /// <param name="PaymentIntentId">Stripe payment Intent Id</param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]</param>
        /// <returns></returns>
        [HttpGet, Route("GetPaymentIntent")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentIntent))]
        public IActionResult GetPaymentIntent([SwaggerParameter(Required = true)]string PaymentIntentId, [SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {
            var result = _paymentServices.GetPaymentIntent(PaymentIntentId, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });

        }

        /// <summary>
        /// Get list of all payments made via stripe on abitnow
        /// </summary>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]</param>
        /// <returns></returns>
        [HttpGet, Route("GetPaymentIntentList")]      
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<Stripe.PaymentIntent>))]
        public IActionResult GetListOfPaymentIntents([SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {

            var result = _paymentServices.GetListOfPaymentIntents();

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });

        }


        #endregion

        #region POST

        /// <summary>
        /// Create a pre-authorize payment for placed order Amount should Grand Total + 10% of Grand total
        /// </summary>
        /// <param name="paymentIntentRequest">Note: Amount passed should be multiplied with 100 before passing into any method (Grand Total *100) </param>
        /// <returns></returns>
        [HttpPost, Route("CreatePaymentIntent")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentIntent))]
        public IActionResult CreatePaymentIntent([FromBody]PaymentIntentRequest paymentIntentRequest)
        {
            var results = new PaymentIntentRequestValidator().Validate(paymentIntentRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.CreatePaymentIntent(paymentIntentRequest.Amount, paymentIntentRequest.ReceiptEmail, paymentIntentRequest.CustomerId, UserId, new string[0], paymentIntentRequest.PaymentMethodId, paymentIntentRequest.ConnectedAccountId, paymentIntentRequest.RestaurantSeoFriendlyName, paymentIntentRequest.RestaurantApplicationFeeAmount, paymentIntentRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }


        /// <summary>
        /// Create a pre-authorize payment for placed order via asynchronous request
        /// </summary>
        /// <param name="paymentIntentRequest">Note: Amount passed should be multiplied with 100 before passing into any method (Grand Total *100) </param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost, Route("CreatePaymentIntentAsync")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentIntent))]
        public async Task<IActionResult> CreatePaymentIntentAsync([FromBody]PaymentIntentRequest paymentIntentRequest)
        {
            var results = new PaymentIntentRequestValidator().Validate(paymentIntentRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = await _paymentServices.CreatePaymentIntentAsync(paymentIntentRequest.Amount, paymentIntentRequest.ReceiptEmail, paymentIntentRequest.CustomerId, UserId, new string[0], paymentIntentRequest.PaymentMethodId, paymentIntentRequest.ConnectedAccountId, paymentIntentRequest.RestaurantSeoFriendlyName, paymentIntentRequest.RestaurantApplicationFeeAmount, paymentIntentRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        /// <summary>
        /// Confirm a pre-Authrize payment for placed order
        /// </summary>
        /// <param name="paymentIntentIdRequest">Note: Amount passed should be multiplied with 100 before passing into any method (Amount *100)</param>
        /// <returns></returns>
        [HttpPost(), Route("ConfirmPaymentIntent")]
        //[Produces(typeof(Stripe.PaymentIntent))]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentIntent))]
        public IActionResult ConfirmPaymentIntent([FromBody]PaymentIntentIdRequest paymentIntentIdRequest)
        {

            var results = new PaymentIntentIdRequestValidator().Validate(paymentIntentIdRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.ConfirmPaymentIntent(paymentIntentIdRequest.PaymentIntentId, paymentIntentIdRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }


        /// <summary>
        /// Capture final amount for the placed order
        /// </summary>
        /// <param name="paymentIntentCapture"> Note: Amount passed should be multiplied with 100 before passing into any method (Amount *100) </param>
        /// <returns></returns>
        [HttpPost(), Route("CapturePaymentIntent")]
 
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentIntent))]
        public IActionResult CapturePaymentIntent([FromBody]PaymentIntentCaptureRequest paymentIntentCapture)
        {

            var results = new PaymentIntentCaptureRequestValidator().Validate(paymentIntentCapture);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.CapturePaymentIntent(paymentIntentCapture.PaymentIntentId, paymentIntentCapture.Amount, paymentIntentCapture.ApplicationFeeAmount, paymentIntentCapture.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        /// <summary>
        /// Confirm and Capture final amount for the placed order
        /// </summary>
        /// <param name="paymentIntentCapture"> Note: Amount passed should be multiplied with 100 before passing into any method (Amount *100) </param>
        /// <returns></returns>
        [HttpPost(), Route("ConfirmAndCapturePaymentIntent")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentIntent))]
        public IActionResult ConfirmAndCapturePaymentIntent([FromBody]PaymentIntentCaptureRequest paymentIntentCapture)
        {
            var paymentIntentIdRequest = new PaymentIntentIdRequest
            {
                PaymentIntentId = paymentIntentCapture.PaymentIntentId,
                IsTestRestaurant = paymentIntentCapture.IsTestRestaurant
            };


            var results = new PaymentIntentIdRequestValidator().Validate(paymentIntentIdRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.ConfirmPaymentIntent(paymentIntentIdRequest.PaymentIntentId, paymentIntentIdRequest.IsTestRestaurant);

            if (result != null)
            {

                var captureResults = new PaymentIntentCaptureRequestValidator().Validate(paymentIntentCapture);

                if (!captureResults.IsValid)
                {
                    captureResults.AddToModelState(ModelState, null);
                    return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = captureResults });
                }

                var resultCapture = _paymentServices.CapturePaymentIntent(paymentIntentCapture.PaymentIntentId, paymentIntentCapture.Amount, paymentIntentCapture.ApplicationFeeAmount, paymentIntentCapture.IsTestRestaurant);

                if (resultCapture != null)
                    return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = resultCapture });
                else
                    return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = resultCapture });

            }
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        /// <summary>
        /// Update pre-authrize payment intent Amount using payment intent Id
        /// </summary>
        /// <param name="updatePaymentIntentRequest">Note: Amount passed should be multiplied with 100 before passing into any method (Amount *100)</param>
        /// <returns></returns>
        [HttpPost(), Route("UpdatePaymentIntent")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentIntent))]
        public IActionResult UpdatePaymentIntent([FromBody]UpdatePaymentIntentRequest updatePaymentIntentRequest)
        {

            var results = new UpdatePaymentIntentRequestValidator().Validate(updatePaymentIntentRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.UpdatePaymentIntent(updatePaymentIntentRequest.Amount, updatePaymentIntentRequest.ApplicationFeeAmount, updatePaymentIntentRequest.ReceiptEmail, updatePaymentIntentRequest.PaymentIntentId, updatePaymentIntentRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        /// <summary>
        /// Cancel payment intent using payment intent Id
        /// </summary>
        /// <param name="paymentIntentIdRequest">payment intent id  request class</param>
        /// <returns></returns>
        [HttpPost(), Route("CancelPaymentIntent")]
 
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentIntent))]
        public IActionResult CancelPaymentIntent([FromBody]PaymentIntentIdRequest paymentIntentIdRequest)
        {

            var results = new PaymentIntentIdRequestValidator().Validate(paymentIntentIdRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.CancelPaymentIntent(paymentIntentIdRequest.PaymentIntentId, paymentIntentIdRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        /// <summary>
        /// Direct payment is used when we need to collect whole amount from user without generating any pre authrize intnet
        /// </summary>
        /// <param name="stripeInfoModel">Note: Amount passed should be multiplied with 100 before passing into any method (Amount *100) </param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost(), Route("DirectPayment")]

        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaymentStatus))]
        public IActionResult DirectPayment([FromBody]StripeInfoModel stripeInfoModel)
        {

            var results = new StripeInfoModelRequestValidator().Validate(stripeInfoModel);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.DirectPayment(stripeInfoModel, stripeInfoModel.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        #endregion


    }
}