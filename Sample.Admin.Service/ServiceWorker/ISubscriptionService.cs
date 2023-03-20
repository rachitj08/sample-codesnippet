using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface ISubscriptionService
    {
        Task<ResponseResultList<SubscriptionsListVM>> GetAllSubscriptions(string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get subscription By Id
        /// </summary>
        /// <returns></returns>
        Task<SubscriptionsVM> GetSubscriptionById(long subscriptionId);

        /// <summary>
        /// To Create New subscription
        /// </summary>
        /// <param name="subscriptionModel">subscription model</param>
        /// <returns></returns>
        Task<ResponseResult<SubscriptionsVM>> AddSubscription(SubscriptionsModel subscriptionModel, int loggedInUserId);

        /// <summary>
        /// To Update existing Subscription
        /// </summary>
        /// <param name="subscription">subscription object</param>
        /// <returns></returns>

        Task<ResponseResult<SubscriptionsVM>> UpdateSubscription(long subscriptionId, SubscriptionsModel subscription, int loggedInUserId);

        /// <summary>
        /// To Update Subscription Partially
        /// </summary>
        /// /// <param name="subscriptionId">Subscription ID</param>
        /// <param name="subscription">New subscription object</param>
        /// <returns></returns>
        Task<ResponseResult<SubscriptionsVM>> UpdatePartialSubscription(long subscriptionId, SubscriptionsModel subscription, int loggedInUserId);

        /// <summary>
        /// To Delete existing Subscription
        /// </summary>
        /// <param name="subscriptionId">subscription identifier</param>
        /// <returns></returns>
        Task<long> DeleteSubscription(long subscriptionId);

        ///// <summary>
        ///// Get All Subscriptions info
        ///// </summary>
        ///// <returns></returns>
        //Task<IEnumerable<Subscriptions>> GetAllSubscriptions();
        ///// <summary>
        ///// Get Subscription By SubscriptionsId
        ///// </summary>
        ///// <param name="subscriptionId">subscription identifier</param>
        ///// <returns></returns>
        //Task<IEnumerable<Subscriptions>> GetSubscriptionBySubscriptionsId(int subscriptionId);
        ///// <summary>
        ///// To Create New Subscription
        ///// </summary>
        ///// <param name="subscriptions"> subscription object</param>
        ///// <returns></returns>
        //Task<Subscriptions> CreateSubscription(Subscriptions subscriptions);
        ///// <summary>
        ///// To Update existing Subscriptions
        ///// </summary>
        ///// <param name="subscriptions">subscription object</param>
        ///// <returns></returns>
        //Task<Subscriptions> UpdateSubscription(Subscriptions subscriptions);
        ///// <summary>
        ///// To Delete existing subscriptions
        ///// </summary>
        ///// <param name="subscriptionId">subscription identifier</param>
        ///// <returns></returns>
        //Task<int> DeleteSubscriptions(int subscriptionsId);
    }
}
