using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Payment.API.RequestValidators;
using PaymentService;
using PaymentService.Card;
using PaymentService.Customer;
using PaymentService.PaymentMethodModel;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;

namespace PaymentAPI.Controllers
{
    /// <summary>
    ///Controller contains Api for Stripe Payment Method(Card) 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : BaseController
    {
        #region VARIABLES       
        private readonly IPaymentServices _paymentServices;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentServices"></param>
        public PaymentMethodController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }
        #endregion

        #region GET


        /// <summary>
        ///  Get deatils of added payment method(cards) of stripe customer using Stripe payment method Id
        /// </summary>
        /// <param name="paymentMethodRequest">Payment method request</param>
        /// <returns></returns>
        [HttpGet(), Route("GetPaymentMethod")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentMethod))]
        public IActionResult GetPaymentMethod([FromQuery] PaymentMethodRequest paymentMethodRequest)
        {

            var result = _paymentServices.GetPaymentMethod(paymentMethodRequest.PaymentMethodId, paymentMethodRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });

        }



        /// <summary>
        /// Get list of added payment methods(cards) of stripe customer using Stripe Customer Id
        /// </summary>
        /// <param name="CustomerId">Stripe geneated customer id of user and mapped with table [AspNetUser]</param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant] </param>
        /// <returns></returns>
        [HttpGet(), Route("GetPaymentMethodList")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.StripeList<Stripe.PaymentMethod>))]
        public IActionResult GetPaymentMethodList([SwaggerParameter(Required = true)]string CustomerId, [SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {

            var result = _paymentServices.GetPaymentMethodList(CustomerId, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });

        }


        #endregion

        #region POST

        /// <summary>
        /// Register new card on stripe 
        /// </summary>
        /// <param name="cardRequest"></param>
        /// <param name="IsTestRestaurant"></param>
        /// <returns></returns>
        [HttpPost(), Route("CreatePaymentMethod")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentMethod))]
        public IActionResult CreatePaymentMethod([FromBody]CardRequest cardRequest, bool IsTestRestaurant = false)
        {
            var results = new CardRequestValidator().Validate(cardRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.CreatePaymentMethod(cardRequest, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }


        /// <summary>
        /// Attach payment method to customer using stripe generated  customerId 
        /// </summary>
        /// <param name="paymentMethodAttachRequest"></param>
        /// <returns></returns>
        [HttpPost(), Route("AttachPaymentMethod")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentMethod))]
        public IActionResult AttachPaymentMethod([FromBody]PaymentMethodAttachRequest paymentMethodAttachRequest)
        {

            var results = new PaymentMethodAttachRequestValidator().Validate(paymentMethodAttachRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.AttachPaymentMethod(paymentMethodAttachRequest.CustomerId, paymentMethodAttachRequest.PaymentMethodId, paymentMethodAttachRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }
        
        /// <summary>
        /// Register new card on stripe and attach it with customer
        /// </summary>
        /// <param name="paymentMethodCreateAttachRequest"></param>
        /// <returns></returns>
        [HttpPost(), Route("CreateAndAttachPaymentMethod")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentMethod))]
        public IActionResult CreateAndAttachPaymentMethod([FromBody]PaymentMethodCreateAttachRequest paymentMethodCreateAttachRequest)
        {
            var cardRequest = new CardRequest
            {
                CardHolderUserId = paymentMethodCreateAttachRequest.CardHolderUserId,
                CardHolderName = paymentMethodCreateAttachRequest.CardHolderName,
                CardHolderEmail = paymentMethodCreateAttachRequest.CardHolderEmail,
                CardPersonalName = paymentMethodCreateAttachRequest.CardPersonalName,
                CardNumber = paymentMethodCreateAttachRequest.CardNumber,
                ExpirationYear = paymentMethodCreateAttachRequest.ExpirationYear,
                ExpirationMonth = paymentMethodCreateAttachRequest.ExpirationMonth,
                CVVNumber = paymentMethodCreateAttachRequest.CVVNumber

            };
            var results = new CardRequestValidator().Validate(cardRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var createResult = _paymentServices.CreatePaymentMethod(cardRequest, paymentMethodCreateAttachRequest.IsTestRestaurant);

            if (createResult != null)
            {
                if (!string.IsNullOrEmpty(createResult.Id))
                {
                    var paymentMethodAttachRequest = new PaymentMethodAttachRequest
                    {
                        CustomerId = paymentMethodCreateAttachRequest.CustomerId,
                        PaymentMethodId = createResult.Id,
                        IsTestRestaurant = paymentMethodCreateAttachRequest.IsTestRestaurant
                    };
                    var attachResults = new PaymentMethodAttachRequestValidator().Validate(paymentMethodAttachRequest);

                    if (!attachResults.IsValid)
                    {
                        attachResults.AddToModelState(ModelState, null);
                        return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = attachResults });
                    }

                    var result = _paymentServices.AttachPaymentMethod(paymentMethodAttachRequest.CustomerId, paymentMethodAttachRequest.PaymentMethodId, paymentMethodAttachRequest.IsTestRestaurant);

                    if (result != null)
                        return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = createResult });
                    else
                        return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });
                }
                else
                    return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = createResult });
            }
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = createResult });


        }

        /// <summary>
        /// Detach previously attached payment using stripe generated  paymentmethodId 
        /// </summary>
        /// <param name="paymentMethodRequest"></param>
        /// <returns></returns>
        [HttpPost(), Route("DetachPaymentMethod")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentMethod))]
        public IActionResult DetachPaymentMethod([FromBody]PaymentMethodRequest paymentMethodRequest)
        {

            var results = new PaymentMethodRequestValidator().Validate(paymentMethodRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.DetachPaymentMethod(paymentMethodRequest.PaymentMethodId, paymentMethodRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }
        [HttpPost(), Route("CreatePaymentMethodNew")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentMethod))]
        public IActionResult CreatePaymentMethodNew([FromBody] CardRequest cardRequest, bool IsTestRestaurant = false)
        {
            //var data = this._userService.GetUserById(2, Convert.ToInt64(cardRequest.CardHolderUserId));
            Stripe.Customer customerResult = null;
            //if (data.Result != null && data.Result.StripeCustomerId == null)
            //{
                var customerController = new CustomerController(_paymentServices);
                CustomerRequest customerRequest = new CustomerRequest()
                {
                    Name = "Shishant",//data.Result.FirstName,
                    Phone = "9999999999",//data.Result.Mobile,
                    Email = "abc@gmail.com"//data.Result.EmailAddress
                };
                customerResult = _paymentServices.CreateCustomer(customerRequest, IsTestRestaurant);
                //----------Update Customer Id-----------------\\
                //customerController.CreateCustomer()
            //}

            var results = new CardRequestValidator().Validate(cardRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.CreatePaymentMethod(cardRequest, IsTestRestaurant);
            //-------------------------Insert Payment Method----------------\\
            if (result != null)
            {

                _paymentServices.AttachPaymentMethod(customerResult.Id, result.Id, IsTestRestaurant);

                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            }
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        #endregion

        #region PUT

        /// <summary>
        /// Update details of a payment method 
        /// </summary>
        /// <param name="updatePaymentMethod">Update payment method request class</param>
        /// <returns></returns>
        [HttpPut(), Route("UpdatePaymentMethod")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.PaymentMethod))]
        public IActionResult UpdatePaymentMethod([FromBody]UpdatePaymentMethod updatePaymentMethod)
        {
            var paymentMethodResult = new PaymentMethodRequestValidator().Validate(updatePaymentMethod.PaymentMethodInfo);
            var cardResult = new CardRequestValidator().Validate(updatePaymentMethod.CardInfo);


            if (!paymentMethodResult.IsValid)
            {
                paymentMethodResult.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = paymentMethodResult });
            }

            if (!cardResult.IsValid)
            {
                cardResult.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = cardResult });
            }

            var result = _paymentServices.UpdatePaymentMethod(updatePaymentMethod.CardInfo, updatePaymentMethod.PaymentMethodInfo.PaymentMethodId, updatePaymentMethod.PaymentMethodInfo.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }
        #endregion

    }
}