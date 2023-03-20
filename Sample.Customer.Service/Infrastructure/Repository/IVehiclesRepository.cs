using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    /// <summary>
    /// IVehiclesRepository
    /// </summary>
    public interface IVehiclesRepository
    {
        /// <summary>
        /// GetAllVehicles
        /// </summary>
        /// <returns></returns>
        public IQueryable<Vehicles> GetAllVehicles(long accountId);

        /// <summary>
        /// GetVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="vinNumber"></param>
        /// <returns></returns>
        public Vehicles GetVehicleDetailsByVinNumber(string vinNumber, long accountId);
        /// <summary>
        /// SaveVehicleDetails
        /// </summary>
        /// <param name="model">Vehicles</param>
        /// <returns></returns>
        public Task<Vehicles> CreateVehicleDetails(Vehicles model);
        /// <summary>
        /// GetVehicleById
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<List<Vehicles>> GetUserVehicleById(long userId, long accountId);

        /// <summary>
        /// GetVehicleDetailsById
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Vehicles GetVehicleDetailsById(long vehicleId, long accountId);
        /// <summary>
        /// UpdateVehicleDetails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Vehicles UpdateVehicleDetails(Vehicles model);
    }
}
