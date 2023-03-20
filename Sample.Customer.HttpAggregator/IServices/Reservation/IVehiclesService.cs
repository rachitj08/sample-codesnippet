using Common.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.IServices.Reservation
{
    /// <summary>
    /// IVehiclesService
    /// </summary>
    public interface IVehiclesService
    {
        /// <summary>
        /// GetAllCars
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        Task<ResponseResult<InComingOutGoingCarsVM>> GetAllCars(string flag, long parkingProviderLocationId,int searchType);
        /// <summary>
        /// GetCarCount
        /// </summary>
        /// <param name="inputDateValue"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        Task<ResponseResult<CarDetailCountVM>> GetCarCount(DateTime inputDateValue, long parkingProviderLocationId);
        /// <summary>
        /// Get Day Wise Count
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        Task<ResponseResult<DayWiseCarCountVM>> GetDayWiseCount(long parkingProviderLocationId);
        /// <summary>
        /// GetVehicleDetailByTagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task<ResponseResult<VehicleDetailVM>> GetVehicleDetailByTagId(string tagId);
        /// <summary>
        /// CreateReservationVehicle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> CreateReservationVehicle(CreateReservationVehicleReqVM model);
        /// <summary>
        /// CreateVehicle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> CreateVehicle(CreateVehicleReqVM model);
       
    }
}
