using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IActivityCodeRepository
    {
        /// <summary>
        /// Get All ActivityCodes
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        List<ActivityCode> GetAllActivityCodes();

        /// <summary>
        ///  Get ActivityCode By ActivityCodeID
        /// </summary>
        /// <param name="ActivityCodeId"></param>
        /// <returns></returns>
        public ActivityCode GetActivityCodeById(long activityCodeId);

        /// <summary>
        /// To Create New ActivityCode
        /// </summary>
        /// <param name="objActivityCode">new ActivityCode object</param>
        /// <returns> ActivityCode object</returns>
        Task<int> CreateActivityCode(ActivityCode objActivityCode, long accountId);

        /// <summary>
        /// To Update ActivityCode
        /// </summary>
        /// <param name="objActivityCode">New ActivityCode object</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<int> UpdateActivityCode(long activityCodeid, long accountId, ActivityCode objActivityCode);


        /// <summary>
        /// DeleteActivityCode
        /// </summary>
        /// <param name="objActivityCode"></param>
        /// <returns></returns>
        Task<int> DeleteActivityCode(ActivityCode objActivityCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedAccountId"></param>
        /// <returns></returns>
        IQueryable<ActivityCode> GetAllActivity(long loggedAccountId);

        /// <summary>
        /// Get Activity Id By Code
        /// </summary>
        /// <param name="activityCode"></param>
        /// <returns></returns>
        ActivityCode GetActivityIdByCode(string activityCode);
    }
}
