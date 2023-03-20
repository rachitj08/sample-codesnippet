using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.IServices.ActivityCode
{
    /// <summary>
    /// IActivityCodeService
    /// </summary>
    public interface IActivityCodeService
    {
        /// <summary>
        /// GetReservationById
        /// </summary>
        /// <param name="activityCodeId"></param>
        /// <returns></returns>
        Task<ResponseResult<ActivityCodeVM>> GetActivityCodeById(long activityCodeId);
        /// <summary>
        /// Get All Activity
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        Task<ResponseResult<List<ActivityCodeVM>>> GetAllActivity(long parkingProviderLocationId);
    }
}
