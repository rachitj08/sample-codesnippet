using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IReservationVehicleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IQueryable<ReservationVehicle> GetAllReservationVehicle(long accountId);

        /// <summary>
        /// Create Reservation Vehicle Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ReservationVehicle> CreateReservationVehicleDetails(ReservationVehicle model);

        /// <summary>
        /// GetReservationVehicleByIs
        /// </summary>
        /// <param name="reservationVehicleId"></param>
        /// <returns></returns>
        ReservationVehicle GetReservationVehicleByIs(long reservationVehicleId);
        /// <summary>
        /// GetReservationVehicleByReservationId
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        ReservationVehicle GetReservationVehicleByReservationId(long reservationId);
        /// <summary>
        /// GetReservationVehicleByVehicleId
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        ReservationVehicle GetReservationVehicleByVehicleId(long vehicleId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ReservationVehicle> CreateReservationVehicleDetailsNew(ReservationVehicle model);
    }
}
