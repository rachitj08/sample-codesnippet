using Common.Model;
using FluentValidation.AspNetCore;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Payment.API.RequestValidators;
using PaymentService;
using PaymentService.Card;
using PaymentService.Customer;
using PaymentService.PaymentMethodModel;
using Stripe;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using Utility;
using Sample.Customer.API.Controllers;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    /// <summary>
    ///Controller contains Api for Stripe Payment Method(Card) 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : BaseApiController
    {
        #region VARIABLES       
        private readonly IPaymentServices _paymentServices;
        private readonly IUserService _userService;
        private readonly ICommonHelper _commonHelper;
        private readonly CommonConfig commonConfig;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentServices"></param>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        public PaymentMethodController(IPaymentServices paymentServices, IUserService userService, ICommonHelper commonHelper, IFileLogger logger, IOptions<CommonConfig> commonConfig) : base(logger: logger)
        {
            _paymentServices = paymentServices;
            _userService = userService;
            _commonHelper = commonHelper;
            this.commonConfig = commonConfig.Value;
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
        public IActionResult GetPaymentMethodList([SwaggerParameter(Required = true)] string CustomerId, [SwaggerParameter(Required = true)] bool IsTestRestaurant = false)
        {

            var result = _paymentServices.GetPaymentMethodList(CustomerId, IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });

        }
        [HttpGet(), Route("FetchValidPaymentMethod")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.StripeList<Stripe.PaymentMethod>))]
        public async Task<IActionResult> FetchValidPaymentMethod([SwaggerParameter(Required = true)] string CustomerId, [SwaggerParameter(Required = true)] bool IsTestRestaurant = false)
        {
            return await Execute(async () =>
            {
                bool isValidPayment = false;
                ResponseResult<bool> responseResult = new ResponseResult<bool>();
                var user = await this._userService.GetUserById(loggedInAccountId, loggedInUserId);
                if (user != null && user.StripeCustomerId == null)
                {
                    isValidPayment = false;
                }
                try
                {
                    var result = _paymentServices.GetPaymentMethodList(user.StripeCustomerId, IsTestRestaurant);
                    if (result != null && result.Data != null)
                    {
                        foreach (var item in result.Data)
                        {
                            DateTime date = new DateTime(Convert.ToInt32(item.Card.ExpYear), Convert.ToInt32(item.Card.ExpMonth), 1);
                            if (item.Card != null && date.Date >= DateTime.Now.Date)
                            {
                                isValidPayment = true;
                                break;
                            }
                        }
                    }
                    
                }
                catch (StripeException ex)
                {
                    isValidPayment=false;
                    //throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);

                }
                catch (Exception ex)
                {
                    isValidPayment = false;
                }
                responseResult = new ResponseResult<bool>()
                {
                    Data = isValidPayment,
                    ResponseCode=ResponseCode.RecordFetched,
                    Message=ResponseMessage.RecordFetched
                };
                return Ok(responseResult);
                //return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });
            });
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
        public IActionResult CreatePaymentMethod([FromBody] CardRequest cardRequest, bool IsTestRestaurant = false)
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
        public IActionResult AttachPaymentMethod([FromBody] PaymentMethodAttachRequest paymentMethodAttachRequest)
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
        public IActionResult CreateAndAttachPaymentMethod([FromBody] PaymentMethodCreateAttachRequest paymentMethodCreateAttachRequest)
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
        public IActionResult DetachPaymentMethod([FromBody] PaymentMethodRequest paymentMethodRequest)
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


        [HttpPost(), Route("CreateCustomerPayment")]
        public async Task<IActionResult> CreateCustomerPayment([FromBody] CardRequestVM cardRequestVM, bool IsTestRestaurant = false)
        {
            ResponseResult<PaymentResponseVM> responseResult = new ResponseResult<PaymentResponseVM>();
            if (string.IsNullOrEmpty(cardRequestVM.PaymentData))
            {
                responseResult = new ResponseResult<PaymentResponseVM>()
                {
                    ResponseCode = ResponseCode.PaymentMethodNotCreated,
                    Message = "Invalid request.",
                    Error = new ErrorResponseResult()
                    {
                        Message = "Invalid request."
                    }
                };
                return Ok(responseResult);
            }
            var dycryptData = _commonHelper.DecryptStringForFronEnd(cardRequestVM.PaymentData, commonConfig.PaymentDecryptKey);
            if (string.IsNullOrEmpty(dycryptData))
            {
                responseResult = new ResponseResult<PaymentResponseVM>()
                {
                    ResponseCode = ResponseCode.PaymentMethodNotCreated,
                    Message = "Invalid request.",
                    Error = new ErrorResponseResult()
                    {
                        Message = "Invalid request."
                    }
                };
                return Ok(responseResult);
            }
            CardRequest cardRequest = JsonConvert.DeserializeObject<CardRequest>(dycryptData);
            if (cardRequest==null)
            {
                responseResult = new ResponseResult<PaymentResponseVM>()
                {
                    ResponseCode = ResponseCode.PaymentMethodNotCreated,
                    Message = "Invalid request.",
                    Error = new ErrorResponseResult()
                    {
                        Message = "Invalid request."
                    }
                };
                return Ok(responseResult);
            }

            return await Execute(async () =>
            {
               
                var results = new CardRequestValidator().Validate(cardRequest);
                string customerId = await UpdateStripeCustomerId(IsTestRestaurant);
                
                if (!results.IsValid)
                {
                    results.AddToModelState(ModelState, null);
                    responseResult = new ResponseResult<PaymentResponseVM>()
                    {
                        ResponseCode = ResponseCode.PaymentMethodNotCreated,
                        Message = ResponseMessage.PaymentMethodNotCreated,
                        Error = new ErrorResponseResult()
                        {
                            Message = "Validation Error"
                        }
                    };
                    //return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
                }
                PaymentMethod createResult = null;
                try
                {
                    createResult = _paymentServices.CreatePaymentMethod(cardRequest, IsTestRestaurant);
                }
                catch (StripeException ex)
                {
                    responseResult = new ResponseResult<PaymentResponseVM>()
                    {
                        ResponseCode = ResponseCode.PaymentMethodNotCreated,
                        Message = "Card detail is Invalid."
                    };
                    return Ok(responseResult);
                }
                catch (Exception)
                {
                    responseResult = new ResponseResult<PaymentResponseVM>()
                    {
                        ResponseCode = ResponseCode.PaymentMethodNotCreated,
                        Message = "Card detail is Invalid."
                    };
                    return Ok(responseResult);
                }

                if (createResult != null)
                {
                    if (!string.IsNullOrEmpty(createResult.Id))
                    {
                        var paymentMethodAttachRequest = new PaymentMethodAttachRequest
                        {
                            CustomerId = customerId,
                            PaymentMethodId = createResult.Id,
                            IsTestRestaurant = IsTestRestaurant
                        };
                        var attachResults = new PaymentMethodAttachRequestValidator().Validate(paymentMethodAttachRequest);

                        if (!attachResults.IsValid)
                        {
                            attachResults.AddToModelState(ModelState, null);
                            responseResult = new ResponseResult<PaymentResponseVM>()
                            {
                                ResponseCode = ResponseCode.PaymentMethodNotCreated,
                                Message = ResponseMessage.PaymentMethodNotCreated,
                                Error = new ErrorResponseResult()
                                {
                                    Message = "Validation Error"
                                }
                            };
                            return Ok(responseResult);
                            //return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = attachResults });
                        }
                        try
                        {

                            var result = _paymentServices.AttachPaymentMethod(paymentMethodAttachRequest.CustomerId, paymentMethodAttachRequest.PaymentMethodId, paymentMethodAttachRequest.IsTestRestaurant);

                            if (result != null)
                            {
                                PaymentResponseVM model = new PaymentResponseVM()
                                {
                                    PaymentMethodId = result.Id,
                                };
                                responseResult = new ResponseResult<PaymentResponseVM>()
                                {
                                    Data = model,
                                    Message = ResponseMessage.PaymentMethodCreated,
                                    ResponseCode = ResponseCode.PaymentMethodCreated

                                };
                                return Ok(responseResult);
                                //return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = createResult });
                            }
                            else
                            {
                                responseResult = new ResponseResult<PaymentResponseVM>()
                                {
                                    ResponseCode = ResponseCode.PaymentMethodNotCreated,
                                    Message = ResponseMessage.PaymentMethodNotCreated,
                                    Error = new ErrorResponseResult()
                                    {
                                        Message = "Error"
                                    }
                                };
                                return Ok(responseResult);
                                //return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });
                            }
                        }
                        catch (StripeException ex)
                        {
                            responseResult = new ResponseResult<PaymentResponseVM>()
                            {
                                ResponseCode = ResponseCode.PaymentMethodNotCreated,
                                Message = ResponseMessage.PaymentMethodNotCreated,
                            };
                            return Ok(responseResult);
                            //throw PaymentService.Error.HandleStripeError.HandleError(ex, isTestBusiness);
                        }
                        catch (Exception ex)
                        {
                            responseResult = new ResponseResult<PaymentResponseVM>()
                            {
                                ResponseCode = ResponseCode.PaymentMethodNotCreated,
                                Message = ResponseMessage.PaymentMethodNotCreated
                            };
                            return Ok(responseResult);
                        }
                    }
                    else
                    {
                        responseResult = new ResponseResult<PaymentResponseVM>()
                        {
                            ResponseCode = ResponseCode.PaymentMethodNotCreated,
                            Message = ResponseMessage.PaymentMethodNotCreated,
                            Error = new ErrorResponseResult()
                            {
                                Message = "Error"
                            }
                        };
                        return Ok(responseResult);
                        //return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = createResult });
                    }
                }
                else
                {
                    responseResult = new ResponseResult<PaymentResponseVM>()
                    {
                        ResponseCode = ResponseCode.PaymentMethodNotCreated,
                        Message = ResponseMessage.PaymentMethodNotCreated,
                        Error = new ErrorResponseResult()
                        {
                            Message = "Error"
                        }
                    };
                    return Ok(responseResult);
                    //return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = createResult });
                }
            });

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
        public IActionResult UpdatePaymentMethod([FromBody] UpdatePaymentMethod updatePaymentMethod)
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
        public async Task<string> UpdateStripeCustomerId(bool IsTestRestaurant)
        {
            string customerId=string.Empty;
            var user = await this._userService.GetUserById(loggedInAccountId, loggedInUserId);
            CustomerRequest customerRequest = new CustomerRequest()
            {
                Name = user.FirstName + " " + user.LastName,
                Phone = user.Mobile,
                Email = user.EmailAddress
            };
            if (user != null && user.StripeCustomerId == null)
            {
               
                Stripe.Customer customerResult = _paymentServices.CreateCustomer(customerRequest, IsTestRestaurant);//Null Check
                if (customerResult != null && !string.IsNullOrEmpty(customerResult.Id))
                {
                    customerId = customerResult.Id;
                    user.StripeCustomerId = customerId;
                    await this._userService.UpdateStripeCustomerId(loggedInAccountId, loggedInUserId, customerId);
                }
            }
            else {
              var customer=  _paymentServices.GetCustomer(user.StripeCustomerId);
                if (customer == null || string.IsNullOrEmpty(customer.Name))
                {
                    Stripe.Customer customerResult = _paymentServices.CreateCustomer(customerRequest, IsTestRestaurant);//Null Check
                    if (customerResult != null && !string.IsNullOrEmpty(customerResult.Id))
                    {
                        customerId = customerResult.Id;
                        user.StripeCustomerId = customerId;
                        await this._userService.UpdateStripeCustomerId(loggedInAccountId, loggedInUserId, customerId);
                    }
                }
                customerId = user.StripeCustomerId;
            }
                
            return customerId;
        }
    }
}