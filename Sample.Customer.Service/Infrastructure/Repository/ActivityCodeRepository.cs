using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ActivityCodeRepository : RepositoryBase<ActivityCode>, IActivityCodeRepository 
    {
        public ActivityCodeRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        /// <summary>
        /// GetAllActivityCodes
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public List<ActivityCode> GetAllActivityCodes()
        {
            return (List<ActivityCode>)base.GetAll();
        }

        /// <summary>
        /// GetActivityCodeById
        /// </summary>
        /// <param name="ActivityCodeId"></param>
        /// <returns></returns>
        public ActivityCode GetActivityCodeById(long activityCodeId)
        {
            return base.GetById(activityCodeId);
        }
        /// <summary>
        /// CreateActivityCode
        /// </summary>
        /// <param name="objActivityCode"></param>
        /// <returns></returns>
        public async Task<int> CreateActivityCode(ActivityCode objActivityCode, long accountId)
        {
            base.context.ActivityCode.Add(objActivityCode);
            return await base.context.SaveChangesAsync();
        }
        /// <summary>
        /// UpdateActivityCode
        /// </summary>
        /// <param name="ActivityCodeId"></param>
        /// <param name="objActivityCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> UpdateActivityCode(long activityCodeid, long accountId, ActivityCode objActivityCode)
        {
            base.context.ActivityCode.Update(objActivityCode);
            return await base.context.SaveChangesAsync();

        }
        /// <summary>
        /// DeleteActivityCode
        /// </summary>
        /// <param name="ActivityCodeId"></param>
        /// <returns></returns>
        public async Task<int> DeleteActivityCode(ActivityCode objActivityCode)
        {
            base.context.ActivityCode.Remove(objActivityCode);
            return await base.context.SaveChangesAsync();
        }

        public IQueryable<ActivityCode> GetAllActivity(long loggedAccountId)
        {
            return GetQuery(x => x.AccountId == loggedAccountId);
        }

        public ActivityCode GetActivityIdByCode(string activityCode)
        {
            return GetQuery(x => x.Code == activityCode).FirstOrDefault();
        }
        
    }
}
