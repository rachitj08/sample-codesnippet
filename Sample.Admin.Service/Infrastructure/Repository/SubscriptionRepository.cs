using Sample.Admin.Service.Infrastructure.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Common.Model;
using Sample.Admin.Model;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class SubscriptionRepository : RepositoryBase<Subscriptions>, ISubscriptionRepository
    {
        public IConfiguration configuration { get; }
        private readonly IMapper mapper;
        public SubscriptionRepository(CloudAcceleratorContext context, IConfiguration configuration, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get All Subscription
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<SubscriptionsListVM>> GetAllSubscription(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            IQueryable<Subscriptions> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("");
            StringBuilder sbPrevious = new StringBuilder("");

            result = (from subscriptions in base.context.Subscriptions
                      select subscriptions);

            if (!all)
            {
                listCount = result.Count();

                var rowIndex = 0;

                if (pageNumber > 0)
                {
                    rowIndex = (pageNumber - 1) * pageSize;
                    if (((pageNumber + 1) * pageSize) <= listCount)
                        sbNext.Append("pageNumber=" + (pageNumber + 1) + "&pageSize=" + pageSize);

                    if (pageNumber > 1)
                        sbPrevious.Append("pageNumber=" + (pageNumber - 1) + "&pageSize=" + pageSize);
                }
                else if (offset > 0)
                {
                    rowIndex = offset;

                    if ((offset + pageSize + 1) <= listCount)
                        sbNext.Append("offset=" + (offset + pageSize) + "&pageSize=" + pageSize);

                    if ((offset - pageSize) > 0)
                        sbPrevious.Append("offset=" + (offset - pageSize) + "&pageSize=" + pageSize);
                }
                else
                {
                    if (pageSize < listCount)
                        sbNext.Append("pageNumber=" + (rowIndex + 1) + "&pageSize=" + pageSize);
                }

                result = result.Skip(rowIndex).Take(pageSize);
            }
            else
            {
                listCount = result.Count();
                sbNext.Append("all=" + all);
                sbPrevious.Append("all=" + all);
            }

            if (!string.IsNullOrWhiteSpace(ordering))
            {
                ordering = string.Concat(ordering[0].ToString().ToUpper(), ordering.AsSpan(1));
                if (typeof(Subscriptions).GetProperty(ordering) != null)
                {
                    result = result.OrderBy(m => EF.Property<object>(m, ordering));
                    if (!string.IsNullOrEmpty(sbNext.ToString()))
                        sbNext.Append("&ordering=" + ordering);

                    if (!string.IsNullOrEmpty(sbPrevious.ToString()))
                        sbPrevious.Append("&ordering=" + ordering);
                }
            }
            else
            {
                result = result.OrderByDescending(x => x.SubscriptionId);
            }

            var subscriptionsNew = new List<SubscriptionsListVM>();
            if (result != null && result.Count() > 0)
            {
                subscriptionsNew = await result.Select(x => new SubscriptionsListVM()
                {
                    SubscriptionId = x.SubscriptionId,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Cancelled = x.Cancelled,
                    CancelledOn = x.CancelledOn,
                    CancelledReason = x.CancelledReason,
                    VersionId = x.VersionId,
                    AccountId = x.AccountId,
                    Account = new AccountsModel
                    {
                        AccountId = x.Account.AccountId,
                        CreatedOn = x.Account.CreatedOn,
                        UpdatedOn = x.Account.UpdatedOn,
                        CreatedBy = x.Account.CreatedBy,
                        UpdatedBy = x.Account.UpdatedBy,
                        AuthenticationCategory = x.Account.AuthenticationCategory,
                        OrganizationName = x.Account.OrganizationName,
                        AccountGuid = x.Account.AccountGuid,
                        AccountUrl = x.Account.AccountUrl,
                        Description = x.Account.Description,
                        ContactPerson = x.Account.ContactPerson,
                        ContactEmail = x.Account.ContactEmail,
                        BillingAddress = x.Account.BillingAddress,
                        BillingContactPerson = x.Account.BillingContactPerson,
                        BillingEmailAddress = x.Account.BillingEmailAddress,
                        Active = x.Account.Active,
                        TenantCss = x.Account.TenantCss,
                        TenantLogo = x.Account.TenantLogo,
                        Region = x.Account.Region,
                        TimeZone = x.Account.TimeZone,
                        Locale = x.Account.Locale,
                        Language = x.Account.Language,
                        IsolationType = x.Account.IsolationType,
                        CurrencyId = x.Account.CurrencyId ?? 0,
                        AuthenticationTypeId = x.Account.AuthenticationTypeId ?? 0,
                    }

                }).ToListAsync();

            }

            return new ResponseResultList<SubscriptionsListVM>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = subscriptionsNew
            };
        }

        /// <summary>
        /// Get Subscription By Id
        /// </summary>
        /// <returns></returns>
        public async Task<Subscriptions> GetSubscriptionById(long subscriptionId)
        {
            return await base.context.Subscriptions.Where(m => m.SubscriptionId == subscriptionId).FirstOrDefaultAsync();
            //return mapper.Map<SubscriptionsVM>(result);
        }

        /// <summary>
        /// To Create subscription
        /// </summary>
        /// <param name="subscription">New Subscription Object</param>
        /// <returns></returns>
        public async Task<long> AddSubscription(Subscriptions subscription)
        {
            base.context.Subscriptions.Add(subscription);
            var result = await base.context.SaveChangesAsync();
            return (result > 0 ? subscription.SubscriptionId : 0);
        }

        /// <summary>
        /// To Update Subscription
        /// </summary>
        /// <param name="subscription">new subscription object</param>
        /// <returns></returns>
        public async Task<int> UpdateSubscription(Subscriptions subscription)
        {
            base.context.Update(subscription);
            return await base.context.SaveChangesAsync();
        }
         

        /// <summary>
        /// To Delete Subscription
        /// </summary>
        /// <param name="subscriptionId">The subscriptionId to delete </param>
        /// <returns></returns>
        public async Task<long> DeleteSubscription(long subscriptionId)
        {
            long result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var subscription = await base.context.Subscriptions.FirstOrDefaultAsync(x => x.SubscriptionId == subscriptionId);

                if (subscription != null)
                {
                    //Delete that post
                    base.context.Subscriptions.Remove(subscription);

                    //Commit the transaction
                    result = await base.context.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        /// <summary>
        /// To Create Subscription
        /// </summary>
        /// <param name="subscriptions">The new subscriptions object</param>
        /// <returns></returns>
        public async Task<Subscriptions> CreateSubscription(Subscriptions subscriptions)
        {
            await base.context.Subscriptions.AddAsync(subscriptions);
            return subscriptions;
        }

        ///// <summary>
        ///// Get All Subscriptions
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IEnumerable<Subscriptions>> GetAllSubscriptions()
        //{
        //    return await base.context.Subscriptions.ToListAsync();
        //}

        ///// <summary>
        ///// Get Subscription By SubscriptionsId
        ///// </summary>
        ///// <param name="subscriptionId">The subscriptionsId to get subscription</param>
        ///// <returns></returns>
        //public async Task<IEnumerable<Subscriptions>> GetSubscriptionBySubscriptionsId(int subscriptionId)
        //{
        //    var result = from subscription in base.context.Subscriptions
        //                 where subscription.SubscriptionId == subscriptionId
        //                 select subscription;

        //    return result;
        //}


        ///// <summary>
        ///// To Update Subscriptions
        ///// </summary>
        ///// <param name="subscriptions">The new subscription object</param>
        ///// <returns></returns>
        //public async Task<Subscriptions> UpdateSubscription(Subscriptions subscriptions)
        //{
        //    base.context.Subscriptions.Update(subscriptions);
        //    return subscriptions;
        //}

        ///// <summary>
        ///// To Delete subscriptions
        ///// </summary>
        ///// <param name="subscriptionsId">The subscriptionsId to delete subscriptions</param>
        ///// <returns></returns>
        //public async Task<int> DeleteSubscriptions(int subscriptionsId)
        //{
        //    return 1;
        //}


    }
}
