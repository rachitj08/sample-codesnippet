using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IActivityCodeService
    {
        /// <summary>
        /// Get All ActivityCodeVMs
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        ResponseResultList<ActivityCodeVM> GetAllActivityCodes(string ordering, int offset, int pageSize, int pageNumber, bool all, long accountId);

        /// <summary>
        ///  Get ActivityCodeVM By ActivityCodeid
        /// </summary>
        /// <param name="activityCodeid"></param>
        /// <returns></returns>
        ResponseResult<ActivityCodeVM> GetActivityCodeById(long activityCodeid);


        /// <summary>
        /// To Create New ActivityCodeVM
        /// </summary>
        /// <param name="objActivityCodeVM">new ActivityCodeVM object</param>
        /// <returns> ActivityCodeVM object</returns>
        Task<ResponseResult<int>> CreateActivityCode(ActivityCodeVM objActivityCodeVM);

       
        /// <summary>
        /// To Update ActivityCodeVM
        /// </summary>
        /// <param name="objActivityCodeVM">New ActivityCodeVM object</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<ResponseResult<int>> UpdateActivityCode(ActivityCodeVM objActivityCodeVM, long activityCodeid, long accountId, long userId);


        /// <summary>
        /// To Delete ActivityCodeVM
        /// </summary>
        /// <param name="ActivityCodeid">The ActivityCodeid to delete</param>
        /// <returns></returns>
        Task<ResponseResult<ActivityCodeVM>> DeleteActivityCode(long activityCodeid);

        /// <summary>
        /// Get All Activity
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <param name="loggedAccountId"></param>
        /// <returns></returns>
        ResponseResult<List<ActivityCodeVM>> GetAllActivity(long parkingProviderLocationId, long loggedAccountId);

    }
}
