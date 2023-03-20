using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.Payment;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.Controllers.Payment
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentMethodService paymentMethodservice;
        /// <summary>
        /// Payment Controller
        /// </summary>
        /// <param name="paymentMethodservice"></param>
        /// <param name="logger"></param>
        public PaymentController(IPaymentMethodService paymentMethodservice, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(paymentMethodservice), paymentMethodservice);
            this.paymentMethodservice = paymentMethodservice;
        }
        /// <summary>
        /// CreatePaymentMethod
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreatePaymentMethod")]
        [ProducesResponseType(typeof(ResponseResult<PaymentMethod>), 200)]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] CardRequestVM model)
        {
            return await Execute(async () =>
            {
                //model.CardHolderUserId = Convert.ToString(loggedInUserId);
                var result = await this.paymentMethodservice.CreatePaymentMethod(model);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                return Ok(result);
            });

        }
        /// <summary>
        /// Check Valid Payment
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("FetchValidPaymentMethod")]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> FetchValidPaymentMethod()
        {
            return await Execute(async () =>
            {
                var result = await this.paymentMethodservice.FetchValidPaymentMethod();
                return Ok(result);
            });

        }
    }
}
