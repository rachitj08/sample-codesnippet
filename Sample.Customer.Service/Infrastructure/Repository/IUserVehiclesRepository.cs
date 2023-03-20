using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IUserVehiclesRepository
    {
        /// <summary>
        /// GetUserVehiclesRepositoryQuery
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IQueryable<UserVehicles> GetUserVehiclesRepositoryQuery(long accountId);
        List<CarDetailVM> GetAllOutgoing<T>(string flag, long accountId, long parkingProviderLocationId);
        List<CarDetailVM> GetAllInComing<T>(string flag, long accountId, long parkingProviderLocationId);
        CarDetailCountVM GetAllOutgoingCount<T>(DateTime inputDateValue, long accountId, long parkingProviderLocationId);
        CarDetailCountVM GetAllInComingCount<T>(DateTime inputDateValue, long accountId, long parkingProviderLocationId);
        List<DayWiseIncomingCount> GetIncomingDayWiseCount<T>(int dayCount, long accountId, long parkingProviderLocationId);
        List<DayWiseOutgoingCount> GetOutgoingDayWiseCount<T>(int dayCount, long accountId, long parkingProviderLocationId);
        CarDetailCountVM GetAllOutgoingCountByDate<T>(DateTime inputDateValue, long accountId, long parkingProviderLocationId);
        CarDetailCountVM GetAllInComingCountByDate<T>(DateTime inputDateValue, long accountId, long parkingProviderLocationId);
        /// <summary>
        /// GetUserVehiclesDataByvehicleIdAndUserID
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserVehicles GetUserVehiclesDataByVehicleIdAndUserID(long vehicleId, long accountId, long userId);

        /// <summary>
        /// Create User Vehicle Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UserVehicles> CreateUserVehicleDetails(UserVehicles model);

        /// <summary>
        /// Get Journey Completed list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flag"></param>
        /// <param name="accountId"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        List<CarDetailVM> GetJourneyCompletedlist<T>(string flag, long accountId, long parkingProviderLocationId);
        /// <summary>
        /// CreateUserVehicleDetailsNew
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        Task<UserVehicles> CreateUserVehicleDetailsNew(UserVehicles model);
    }
}
