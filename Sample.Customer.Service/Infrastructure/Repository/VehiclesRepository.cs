using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class VehiclesRepository :RepositoryBase<Vehicles>, IVehiclesRepository 
    {
        public VehiclesRepository(CloudAcceleratorContext context) : base(context)
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
        public IQueryable<Vehicles> GetAllVehicles(long accountId)
        {
            //return (List<Vehicles>)base.GetAll();
            return GetQuery(x => x.AccountId == accountId);
            
        }

        public Task<Vehicles> CreateVehicleDetails(Vehicles model)
        {
            base.context.Vehicles.Add(model);
            base.context.SaveChanges();
            return Task.FromResult(model);
        }

        public Vehicles GetVehicleDetailsByVinNumber(string vinNumber, long accountId)
        {
            return base.context.Vehicles.Where(x => x.Vinnumber == vinNumber && x.AccountId == accountId)
                   .Include(x => x.VehicleCategory)
                   .Include(x => x.ReservationVehicle)
                   .Include(x => x.Shuttle)
                   .Include(x => x.UserVehicles)
                   .Include(x => x.VehicleFeaturesMapping)
                   .Include(x => x.VehiclesEventLog)
                   .Include(x => x.VehiclesMediaStorage).FirstOrDefault();
        }
        public async Task<List<Vehicles>> GetUserVehicleById(long userId, long accountId)
        {

            try
            {
               
                var userNavigations = from userVehicle in base.context.UserVehicles
                                      join vehicle in base.context.Vehicles on userVehicle.VehicleId equals vehicle.VehicleId
                                      where userVehicle.UserId == userId && userVehicle.AccountId == accountId
                                      select vehicle;
                return await userNavigations.ToListAsync();
            }
            catch (Exception ex)
            {
                var userVehicleData = base.context.UserVehicles.Where(x => x.UserId == userId).FirstOrDefault();
                var veh = base.context.Vehicles.Where(x => x.VehicleId == userVehicleData.VehicleId).FirstOrDefault();
                return null;
            }
        }

        public Vehicles GetVehicleDetailsById(long vehicleId, long accountId)
        {
            return base.context.Vehicles.Where(x => x.VehicleId == vehicleId && x.AccountId == accountId)
                   .Include(x => x.VehicleCategory)
                   .Include(x => x.ReservationVehicle)
                   .Include(x => x.Shuttle)
                   .Include(x => x.UserVehicles)
                   .Include(x => x.VehicleFeaturesMapping)
                   .Include(x => x.VehiclesEventLog)
                   .Include(x => x.VehiclesMediaStorage).FirstOrDefault();
        }

        public Vehicles UpdateVehicleDetails(Vehicles model)
        {
            var vehicle=Update(model);
            context.SaveChanges();
            return vehicle;
        }
    }
}
