using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Service.Infrastructure.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Admin.Model;
using Common.Model;
using System;

namespace Sample.Admin.Service.ServiceWorker
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISubscriptionRepository subscriptionsRepository;

        /// <summary>
        /// Modules Service constructor to Inject dependency
        /// </summary>
        /// <param name="unitOfWork">unit of work</param>
        /// <param name="moduleRepository">module repository</param>
        public SubscriptionService(IUnitOfWork unitOfWork, ISubscriptionRepository subscriptionsRepository)
        {
            this.unitOfWork = unitOfWork;
            this.subscriptionsRepository = subscriptionsRepository;
        }

        /// <summary>
        /// Get All Subscriptions
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<SubscriptionsListVM>> GetAllSubscriptions(string ordering, int offset, int pageSize, int pageNumber, bool all) => await this.subscriptionsRepository.GetAllSubscription(ordering, offset, pageSize, pageNumber, all);

        /// <summary>
        /// Get Subscription By Id
        /// </summary>
        /// <returns></returns>
        public async Task<SubscriptionsVM> GetSubscriptionById(long subscriptionId)
        {
            var subscription = await this.subscriptionsRepository.GetSubscriptionById(subscriptionId);
            if (subscription != null)
            {
                return new SubscriptionsVM()
                {
                    Account = subscription.AccountId,
                    AccountId = subscription.AccountId,
                    Description = subscription.Description,
                    SubscriptionId = subscription.SubscriptionId,
                    Version = subscription.VersionId,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    Cancelled = subscription.Cancelled,
                    CancelledOn = subscription.CancelledOn,
                    CancelledReason = subscription.CancelledReason,
                };
            }
            return null;
        }

        /// <summary>
        /// To Create New Subscription
        /// </summary>
        /// <param name="newSubscription">Subscription Param for subscription</param>
        /// <returns></returns>
        public async Task<ResponseResult<SubscriptionsVM>> AddSubscription(SubscriptionsModel subscription, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (subscription.Account < 1)
            {
                errorDetails.Add("account", new string[] { "This field may not be blank." });
            }

            if (subscription.Version < 1)
            {
                errorDetails.Add("version", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(subscription.Description))
            {
                errorDetails.Add("description", new string[] { "This field may not be blank." });
            }
            else if (subscription.Description.Length > 500)
            {
                errorDetails.Add("description", new string[] { "Ensure this field has no more than 500 characters." });
            }

            if (subscription.StartDate == null)
            {
                errorDetails.Add("startDate", new string[] { "This field may not be blank." });
            }

            if (subscription.EndDate == null)
            {
                errorDetails.Add("endDate", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<SubscriptionsVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var subscriptionNew = new Subscriptions()
            {
                AccountId = subscription.Account,
                Description = subscription.Description,
                VersionId = subscription.Version,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUserId,
                Cancelled = subscription.Cancelled,
                CancelledOn = subscription.CancelledOn,
                CancelledReason = subscription.CancelledReason
            }; 

            var subscriptionId = await this.subscriptionsRepository.AddSubscription(subscriptionNew);
            if (subscriptionId > 0)
            {
                return new ResponseResult<SubscriptionsVM>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = new SubscriptionsVM()
                    {
                        AccountId = subscription.Account,
                        Description = subscription.Description,
                        Version = subscription.Version,
                        StartDate = subscription.StartDate,
                        EndDate = subscription.EndDate,
                        Cancelled = subscription.Cancelled,
                        CancelledOn = subscription.CancelledOn,
                        CancelledReason = subscription.CancelledReason,
                        SubscriptionId = subscriptionId,
                        Account = subscription.Account,
                    }
                };
            }
            else
            {
                return new ResponseResult<SubscriptionsVM>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
        }

        /// <summary>
        /// To Update existing Subscription
        /// </summary>
        /// <param name="subscription">subscription object</param>
        /// <returns></returns>
        public async Task<ResponseResult<SubscriptionsVM>> UpdateSubscription(long subscriptionId, SubscriptionsModel subscription, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (subscription.Account < 1)
            {
                errorDetails.Add("account", new string[] { "This field may not be blank." });
            }

            if (subscription.Version < 1)
            {
                errorDetails.Add("version", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(subscription.Description))
            {
                errorDetails.Add("description", new string[] { "This field may not be blank." });
            }
            else if (subscription.Description.Length > 500)
            {
                errorDetails.Add("description", new string[] { "Ensure this field has no more than 500 characters." });
            }

            if (subscription.StartDate == null)
            {
                errorDetails.Add("startDate", new string[] { "This field may not be blank." });
            }

            if (subscription.EndDate == null)
            {
                errorDetails.Add("endDate", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<SubscriptionsVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var subscriptionExisting = await this.subscriptionsRepository.GetSubscriptionById(subscriptionId);
            if (subscriptionExisting == null)
            {
                return new ResponseResult<SubscriptionsVM>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }

            subscriptionExisting.AccountId = subscription.Account;
            subscriptionExisting.VersionId = subscription.Version;
            subscriptionExisting.Description = subscription.Description;
            subscriptionExisting.StartDate = subscription.StartDate;
            subscriptionExisting.EndDate = subscription.EndDate;
            subscriptionExisting.Cancelled = subscription.Cancelled;
            subscriptionExisting.CancelledOn = subscription.CancelledOn;
            subscriptionExisting.CancelledReason = subscription.CancelledReason;
            subscriptionExisting.UpdatedBy = loggedInUserId;
            subscriptionExisting.UpdatedOn = DateTime.UtcNow;

            var result = await this.subscriptionsRepository.UpdateSubscription(subscriptionExisting);

            if (result > 0)
            {
                return new ResponseResult<SubscriptionsVM>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = new SubscriptionsVM()
                    {
                        AccountId = subscription.Account,
                        Description = subscription.Description,
                        Version = subscription.Version,
                        StartDate = subscription.StartDate,
                        EndDate = subscription.EndDate,
                        Cancelled = subscription.Cancelled,
                        CancelledOn = subscription.CancelledOn,
                        CancelledReason = subscription.CancelledReason,
                        SubscriptionId = subscriptionId,
                        Account = subscription.Account,
                    }
                };
            }
            else
            {
                return new ResponseResult<SubscriptionsVM>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }

        /// <summary>
        /// To Update Subscription Partially
        /// </summary>
        /// /// <param name="subscriptionId">Subscription ID</param>
        /// <param name="subscription">New subscription object</param>
        /// <returns></returns>
        public async Task<ResponseResult<SubscriptionsVM>> UpdatePartialSubscription(long subscriptionId, SubscriptionsModel subscription, int loggedInUserId)
        {
            return await UpdateSubscription(subscriptionId, subscription, loggedInUserId);
        }

        /// <summary>
        /// To delete existing Subscription
        /// </summary>
        /// <param name="subscriptionId">subscription identifier</param>
        /// <returns></returns>
        public async Task<long> DeleteSubscription(long subscriptionId) => await this.subscriptionsRepository.DeleteSubscription(subscriptionId);

        ///// <summary>
        ///// Get All Subscriptions
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IEnumerable<Subscriptions>> GetAllSubscriptions()
        //{

        //    var modulesData = await this.subscriptionsRepository.GetAllSubscriptions();
        //    return modulesData;

        //}

        ///// <summary>
        ///// Get Subscription By SubscriptionsId
        ///// </summary>
        ///// <param name="subscriptionId"> subscription identifier</param>
        ///// <returns></returns>
        //public async Task<IEnumerable<Subscriptions>> GetSubscriptionBySubscriptionsId(int subscriptionId)
        //{

        //    var result = await this.subscriptionsRepository.GetSubscriptionBySubscriptionsId(subscriptionId);

        //    return result;

        //}

        ///// <summary>
        ///// To Create new Subscription
        ///// </summary>
        ///// <param name="subscriptions">subscription object</param>
        ///// <returns></returns>
        //public async Task<Subscriptions> CreateSubscription(Subscriptions subscriptions)
        //{

        //    var result = await this.subscriptionsRepository.CreateSubscription(subscriptions);
        //    return result;

        //}

        ///// <summary>
        ///// To Update existing subscriptions
        ///// </summary>
        ///// <param name="subscriptions">subscription object</param>
        ///// <returns></returns>
        //public async Task<Subscriptions> UpdateSubscription(Subscriptions subscriptions)
        //{

        //    var result = await this.subscriptionsRepository.UpdateSubscription(subscriptions);
        //    return result;

        //}

        ///// <summary>
        ///// To delete existing subscriptions
        ///// </summary>
        ///// <param name="subscriptionsId">subscription identifier</param>
        ///// <returns></returns>
        //public async Task<int> DeleteSubscriptions(int subscriptionsId)
        //{

        //    var result = await this.subscriptionsRepository.DeleteSubscriptions(subscriptionsId);
        //    return result;

        //}
    }
}
