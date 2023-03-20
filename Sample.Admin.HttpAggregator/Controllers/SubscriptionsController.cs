using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Subscriptions Controller
    /// </summary>
    [Route("api/subscriptions")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : BaseApiController
    {

        private readonly ISubscriptionService subscriptionService;

        /// <summary>
        /// Subscription Controller constructor to Inject dependency
        /// </summary>
        /// <param name="subscriptionService">subscription service </param>
        /// <param name="logger">logger service </param>
        public SubscriptionController(ISubscriptionService subscriptionService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(subscriptionService), subscriptionService);
            this.subscriptionService = subscriptionService;
        }

        /// <summary>
        ///  Information for all existing subscriptions
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList), 200)]
        public async Task<IActionResult> GetAllSubscriptions([FromQuery]string ordering, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] bool all = false)
        {
            return await Execute(async () =>
            {
                var subscriptions = await subscriptionService.GetAllSubscriptions(HttpContext,ordering, offset,pageSize,pageNumber,all);
                if (subscriptions.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(subscriptions);
                else
                    return BadRequest(subscriptions);
            });

        }

        /// <summary>
        ///  Information for subscription by id
        /// </summary>
        /// <param name="subscriptionId">A unique integer value identifying this Subscription.</param>
        /// <returns></returns>
        [Route("{subscriptionId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<SubscriptionsModel>), 200)]
        public async Task<IActionResult> GetSubscriptionById([FromRoute] int subscriptionId)
        {
            return await Execute(async () =>
            {
                    var result = await this.subscriptionService.GetSubscriptionById(subscriptionId);
                    if (result.ResponseCode == ResponseCode.RecordFetched)
                        return Ok(result);
                    else
                        return BadRequest(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Subscription
        /// </summary>
        /// <param name="subscription">The new subscription object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<SubscriptionsVM>), 201)]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionsModel subscription)
        {
            return await Execute(async () =>
            {
                if (subscription != null)
                {
                    var result = await this.subscriptionService.AddSubscription(subscription,base.loggedInUserId);
                    if (result != null)
                    {
                        if(result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Created("api/subscriptions/", result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<SubscriptionsVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<SubscriptionsVM>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                });

            });
        }

        /// <summary>
        /// This api is used for Updating existing Subscription
        /// </summary>
        /// <param name="subscription">The existing subscription object.</param>
        /// <param name="subscriptionId">A unique integer value identifying this Subscription.</param>
        /// <returns></returns>
        [Route("{subscriptionId}")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult<SubscriptionsVM>), 200)]
        public async Task<IActionResult> UpdateSubscription([FromRoute] int subscriptionId, [FromBody] SubscriptionsModel subscription)
        {
            return await Execute(async () =>
            {
                if (subscriptionId != 0 && subscription != null)
                {
                    var result = await this.subscriptionService.UpdateSubscription(subscriptionId, subscription);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {

                        return BadRequest(new ResponseResult<SubscriptionsVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<SubscriptionsVM>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                });

            });
        }

        /// <summary>
        /// This api is used for Updating existing Subscription partially
        /// </summary>
        /// <param name="subscription">The existing subscription object.</param>
        /// <param name="subscriptionId">A unique integer value identifying this Subscription.</param>
        /// <returns></returns>
        [Route("{subscriptionId}")]
        [HttpPatch]
        [ProducesResponseType(typeof(ResponseResult<SubscriptionsVM>), 200)]
        public async Task<IActionResult> UpdatePartialSubscription([FromRoute] int subscriptionId, [FromBody] SubscriptionsModel subscription)
        {
            return await Execute(async () =>
            {
                if (subscriptionId != 0 && subscription != null)
                {
                    var result = await this.subscriptionService.UpdatePartialSubscription(subscriptionId, subscription);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<SubscriptionsVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    } 
                }

                return BadRequest(new ResponseResult<SubscriptionsVM>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });
            });
        }


        /// <summary>
        /// This api is used for deleing  Subscription
        /// </summary>
        /// <param name="subscriptionId">A unique integer value identifying this Subscription.</param>
        /// <returns></returns>
        [Route("{subscriptionId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> DeleteSubscription([FromRoute] long subscriptionId)
        {

            return await Execute(async () =>
            {
                var result = await this.subscriptionService.DeleteSubscription(subscriptionId);

                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordDeleted)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });
            });

        }
    }

}
