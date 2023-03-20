using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
   
    public class ReservationVehicleRepository : RepositoryBase<ReservationVehicle>, IReservationVehicleRepository
    {
        public ReservationVehicleRepository(CloudAcceleratorContext context) : base(context)
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
        public IQueryable<ReservationVehicle> GetAllReservationVehicle(long accountId)
        {
            //return (List<Vehicles>)base.GetAll();
            return GetQuery(x => x.AccountId == accountId);

        }
        /// <summary>
        /// Create Reservation Vehicle Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReservationVehicle> CreateReservationVehicleDetails(ReservationVehicle model)
        { 
            return await CreateAsync(model);
        }

        /// <summary>
        /// GetVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="reservationVehicleId"></param>
        /// <returns></returns>
        public ReservationVehicle GetReservationVehicleByIs(long reservationVehicleId)
        {
            return base.context.ReservationVehicle.Where(x => x.ReservationVehicleId == reservationVehicleId)
                   .Include(x => x.ParkingProvidersLocation)
                   .Include(x => x.Reservation)
                   .Include(x => x.Vehicle)
                   .Include(x => x.VehicleAvailablity).FirstOrDefault();
        }
        /// <summary>
        /// GetVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="reservationVehicleId"></param>
        /// <returns></returns>
        public ReservationVehicle GetReservationVehicleByReservationId(long reservationId)
        {
            return base.context.ReservationVehicle.Where(x => x.ReservationId == reservationId)
                   .Include(x => x.ParkingProvidersLocation)
                   .Include(x => x.Reservation)
                   .Include(x => x.Vehicle)
                   .Include(x => x.VehicleAvailablity).FirstOrDefault();
        }
        /// <summary>
        /// GetVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="reservationVehicleId"></param>
        /// <returns></returns>
        public ReservationVehicle GetReservationVehicleByVehicleId(long vehicleId)
        {
            return base.context.ReservationVehicle.Where(x => x.VehicleId == vehicleId)
                   .Include(x => x.ParkingProvidersLocation)
                   .Include(x => x.Reservation)
                   .Include(x => x.Vehicle)
                   .Include(x => x.VehicleAvailablity).FirstOrDefault();
        }

        public async Task<ReservationVehicle> CreateReservationVehicleDetailsNew(ReservationVehicle model)
        {
                var ReservationVehicle= await CreateAsync(model);
            await context.SaveChangesAsync();
            return ReservationVehicle;
        }
    }
}
