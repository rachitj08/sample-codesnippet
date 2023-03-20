using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IVehiclesService
    {
        /// <summary>
        /// GetAllVehicles
        /// </summary>
        /// <returns></returns>
        ResponseResult<InComingOutGoingCarsVM> GetAllCars(string flag, long parkingProviderLocationId, long accountId, int searchType);
        ResponseResult<CarDetailCountVM> GetCarCount(DateTime inputDateValue, long parkingProviderLocationId, long accountId);
        ResponseResult<DayWiseCarCountVM> GetDayWiseCount(long parkingProviderLocationId, long accountId);
        
        /// <summary>
        /// GetVehicleDetailByTagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<ResponseResult<VehicleDetailVM>> GetVehicleDetailByTagId(string tagId, long accountId);

        /// <summary>
        /// Create Reservation Vehicle
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> CreateReservationVehicle(CreateReservationVehicleReqVM model, long accountId, long userId);

        /// <summary>
        /// Create Vehicle & Reservation Vehicle
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> CreateVehicle(CreateVehicleReqVM model, long accountId, long userId);
        /// <summary>
        /// GetJourneyCompletedlist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flag"></param>
        /// <param name="accountId"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        ResponseResult<List<CarDetailVM>> GetJourneyCompletedlist(string flag, long parkingProviderLocationId, long accountId);
    }
}
