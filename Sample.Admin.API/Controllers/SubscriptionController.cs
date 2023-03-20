using System.Threading.Tasks;
using Sample.Admin.Service.ServiceWorker;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/subscriptions")]
    [ApiController]
    public class SubscriptionController : BaseApiController
    {
        private readonly ISubscriptionService subscriptionsService;

        /// <summary>
        /// Subscription Controller constructor to Inject dependency
        /// </summary>
        /// <param name="subscriptionsService">subscriptionservice for user</param>
        public SubscriptionController(ISubscriptionService subscriptionsService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(subscriptionsService), subscriptionsService);
            this.subscriptionsService = subscriptionsService;
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
        public async Task<IActionResult> GetAllSubscriptions(string ordering, int offset, int pageSize, int pageNumber, bool all = false)
        {
            return await Execute(async () =>
            {
                var result = await this.subscriptionsService.GetAllSubscriptions(ordering, offset, pageSize, pageNumber, all);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        ///  Information for subscription by id
        /// </summary>
        /// <param name="subscriptionId">A unique integer value identifying this Subscription.</param>
        /// <returns></returns>
        [Route("{subscriptionId}")]
        [HttpGet]
        public async Task<IActionResult> GetSubscriptionById([FromRoute] int subscriptionId)
        {
            return await Execute(async () =>
            {
                var result = await this.subscriptionsService.GetSubscriptionById(subscriptionId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Subscription
        /// </summary>
        /// <param name="subscription">The new subscription object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> AddSubscription([FromBody] SubscriptionsModel subscription)
        {

            return await Execute(async () =>
            {
                var result = await this.subscriptionsService.AddSubscription(subscription, loggedInUserId);
                return Ok(result);
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
        public async Task<IActionResult> UpdateSubscription([FromRoute] long subscriptionId, [FromBody] SubscriptionsModel subscription)
        {
            return await Execute(async () =>
            {
                var result = await this.subscriptionsService.UpdateSubscription(subscriptionId, subscription, loggedInUserId);
                return Ok(result);
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
        public async Task<IActionResult> UpdatePartialSubscription([FromRoute] long subscriptionId, [FromBody] SubscriptionsModel subscription)
        {
            return await Execute(async () =>
            {
                var result = await this.subscriptionsService.UpdatePartialSubscription(subscriptionId, subscription, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// This api is used for deleing  AccountService
        /// </summary>
        /// <param name="subscriptionId">subscription identifier</param>
        /// <returns></returns>
        [Route("{subscriptionId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAccountService([FromRoute] long subscriptionId)
        {
            return await Execute(async () =>
            {
                var result = await this.subscriptionsService.DeleteSubscription(subscriptionId);
                return Ok(result);
            });
        }

        //// GET: api/<SubscriptionController>
        ///// <summary>
        /////  Get All existing Subscriptions in the database
        ///// </summary>
        ///// <returns></returns>
        //[Route("getallsubscriptions")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllSubscriptions()
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.subscriptionsService.GetAllSubscriptions();
        //        return Ok(result);
        //    });

        //}

        ///// <summary>
        ///// Get Subscription By their SubscriptionsId
        ///// </summary>
        ///// <param name="subscriptionId">subscription identifier</param>
        ///// <returns></returns>
        //[Route("getsubscriptionbysubscriptionsid")]
        //[HttpGet]
        //public async Task<IActionResult> GetSubscriptionBySubscriptionsId(int subscriptionId)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.subscriptionsService.GetSubscriptionBySubscriptionsId(subscriptionId);
        //        return Ok(result);
        //    });


        //}

        ///// <summary>
        ///// This api is used for Creating new Subscription
        ///// </summary>
        ///// <param name="subscriptions">subscription model information for subscription</param>
        ///// <returns></returns>
        //[Route("createsubscription")]
        //[HttpPost]
        //public async Task<IActionResult> CreateSubscription([FromBody] Subscriptions subscriptions)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.subscriptionsService.CreateSubscription(subscriptions);
        //        return Ok(result);
        //    });

        //}


        ///// <summary>
        ///// This api is used for Updating existing Subscription
        ///// </summary>
        ///// <param name="subscriptions">subscription model for subscription information</param>
        ///// <returns></returns>
        //[Route("UpdateSubscription")]
        //[HttpPut]
        //public async Task<IActionResult> UpdateSubscription([FromBody] Subscriptions subscriptions)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.subscriptionsService.UpdateSubscription(subscriptions);
        //        return Ok(result);
        //    });


        //}


        ///// <summary>
        ///// This api is used for deleting subscriptions
        ///// </summary>
        ///// <param name="subscriptionsId">subscription identifier</param>
        ///// <returns></returns>
        //[Route("deletesubscriptions")]
        //[HttpDelete]
        //public async Task<IActionResult> DeleteSubscriptions(int subscriptionsId)
        //{

        //    return await Execute(async () =>
        //    {
        //        var result = await this.subscriptionsService.DeleteSubscriptions(subscriptionsId);
        //        return Ok(result);
        //    });

        //}
    }
}
