using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    ///Subscription Service
    /// </summary>
    public interface ISubscriptionService
    {
        /// <summary>
        /// Subscription Service to get subscription list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<SubscriptionsListVM>> GetAllSubscriptions(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Subscription By Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<SubscriptionsVM>> GetSubscriptionById(long subscriptionId);

        /// <summary>
        /// To Create new Subscription
        /// </summary>
        /// <param name="subscription">subscription object</param>
        /// <param name="currentloggedInUserID">subscription object</param>
        /// <returns></returns>
        Task<ResponseResult<SubscriptionsVM>> AddSubscription(SubscriptionsModel subscription, long currentloggedInUserID);

        /// <summary>
        /// To Update existing Subscription
        /// </summary>
        /// <param name="subscription">subscription object</param>
        /// /// <param name="subscriptionId">Unique Subscription Id</param>
        /// <returns></returns>

        Task<ResponseResult<SubscriptionsVM>> UpdateSubscription(long subscriptionId, SubscriptionsModel subscription);

        /// <summary>
        /// To Update existing Subscription
        /// </summary>
        /// <param name="subscription">subscription object</param>
        /// /// <param name="subscriptionId">Unique Subscription Id</param>
        /// <returns></returns>

        Task<ResponseResult<SubscriptionsVM>> UpdatePartialSubscription(long subscriptionId, SubscriptionsModel subscription);

        /// <summary>
        /// To Delete existing Subscription
        /// </summary>
        /// <param name="subscriptionId">subscription identifier</param>
        /// <returns></returns>
        Task<ResponseResult<SubscriptionsVM>> DeleteSubscription(long subscriptionId);

    }
}
