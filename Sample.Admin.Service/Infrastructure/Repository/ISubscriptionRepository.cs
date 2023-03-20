using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface ISubscriptionRepository
    {
        /// <summary>
        /// Get All Subscription
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<SubscriptionsListVM>> GetAllSubscription(string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Subscription By ID
        /// </summary>
        /// <returns></returns>
        Task<Subscriptions> GetSubscriptionById(long subscriptionId);

        /// <summary>
        /// To Create Subscriptions
        /// </summary>
        /// <param name="subscription">New Subscription Object</param>
        /// <returns></returns>
        Task<long> AddSubscription(Subscriptions subscription);

        /// <summary>
        /// To Update Subscription
        /// </summary>
        /// <param name="subscription">New subscription object</param>
        /// <returns></returns>
        Task<int> UpdateSubscription(Subscriptions subscription);

        /// <summary>
        /// To Delete Subscription
        /// </summary>
        /// <param name="subscriptionId">The subscriptionId to delete</param>
        /// <returns></returns>
        Task<long> DeleteSubscription(long subscriptionId);

        /// <summary>
        /// To Create Subscription
        /// </summary>
        /// <param name="subscriptions">new subscription object</param>
        /// <returns></returns>
        Task<Subscriptions> CreateSubscription(Subscriptions subscriptions);

        ///// <summary>
        ///// To Update Subscription Partially
        ///// </summary>
        ///// /// <param name="subscriptionId">Subscription ID</param>
        ///// <param name="subscription">New subscription object</param>
        ///// <returns></returns>
        //Task<SubscriptionsVM> UpdatePartialSubscription(long subscriptionId, SubscriptionsModel subscription);

        ///// <summary>
        ///// Get All Subscriptions
        ///// </summary>
        ///// <returns></returns>
        //Task<IEnumerable<Subscriptions>> GetAllSubscriptions();

        ///// <summary>
        ///// Get Subscription By SubscriptionsId
        ///// </summary>
        ///// <param name="subscriptionId">The subscriptionsId to get subscription</param>
        ///// <returns></returns>
        //Task<IEnumerable<Subscriptions>> GetSubscriptionBySubscriptionsId(int subscriptionId);

        ///// <summary>
        ///// To Update Subscriptions
        ///// </summary>
        ///// <param name="subscriptions">new subscription object/param>
        ///// <returns></returns>
        //Task<Subscriptions> UpdateSubscription(Subscriptions subscriptions);

        ///// <summary>
        ///// To Delete subscriptions
        ///// </summary>
        ///// <param name="subscriptionsId">The subscriptionsId to delete subscriptions</param>
        ///// <returns></returns>
        //Task<int> DeleteSubscriptions(int subscriptionsId);
    }
}
