using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Payment.API.RequestValidators;
using PaymentService;
using PaymentService.RefundModel;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;

namespace PaymentAPI.Controllers
{
    /// <summary>
    /// Controller contains Api's for  Refund management
    /// </summary>
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : BaseController
    {
        #region VARIABLES       
        private readonly IPaymentServices _paymentServices;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentServices"></param>
        public RefundController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }
        #endregion

        #region GET


        /// <summary>
        /// Get Details of refund  using refund Id
        /// </summary>
        /// <param name="RefundId">Stripe refund  Id</param>
        /// <param name="IsTestRestaurant">Field defines that restaurant is accepting live/test payment and mapped with table [Restaurant]</param>
        /// <returns></returns>
        [HttpGet, Route("GetRefundDetails")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Refund))]
        public IActionResult GetRefundDetails([SwaggerParameter(Required = true)]string RefundId, [SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {
            var result = _paymentServices.GetRefundDetails(RefundId, IsTestRestaurant);

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
        [HttpGet, Route("GetRefundList")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<Stripe.Refund>))]
        public IActionResult GetRefundList([SwaggerParameter(Required = true)]bool IsTestRestaurant = false)
        {

            var result = _paymentServices.GetRefundList(IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });

        }


        #endregion

        #region POST

        /// <summary>
        /// Create a refund corresponding to a successful payment
        /// </summary>
        /// <param name="createRefundRequest">Create refund request class</param>
        /// <returns></returns>
        [HttpPost, Route("CreateRefund")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Refund))]
        public IActionResult CreateRefund([FromBody]CreateRefundRequest createRefundRequest)
        {
            var results = new CreateRefundRequestValidator().Validate(createRefundRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.CreateRefundForIntent(createRefundRequest);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        /// <summary>
        /// Update refund using refund Id
        /// </summary>
        /// <param name="refundRequest"></param>
        /// <returns></returns>
        [HttpPost(), Route("UpdateRefund")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Stripe.Refund))]
        public IActionResult UpdateRefund([FromBody]RefundRequest refundRequest)
        {

            var results = new RefundRequestValidator().Validate(refundRequest);

            if (!results.IsValid)
            {
                results.AddToModelState(ModelState, null);
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Validation Error", Entity = results });
            }

            var result = _paymentServices.UpdateRefund(refundRequest.RefundId, refundRequest.IsTestRestaurant);

            if (result != null)
                return new OkObjectResult(new { IsSuccess = true, StatusCode = HttpStatusCode.OK, Message = "Success", Entity = result });
            else
                return new BadRequestObjectResult(new { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = "Error", Entity = result });


        }

        #endregion

    }
}