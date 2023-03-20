using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IVehicleFeaturesMappingRepository
    {
        /// <summary>
        /// GetAllVehicles
        /// </summary>
        /// <returns></returns>
        public IQueryable<VehicleFeaturesMapping> GetAllVehicleFeaturesMapping(long accountId);

        /// <summary>
        /// GetVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="vinNumber"></param>
        /// <returns></returns>
        public VehicleFeaturesMapping GetVehicleFeaturesMappingById(long vehicleFeaturesMappingId);
        /// <summary>
        /// SaveVehicleDetails
        /// </summary>
        /// <param name="objVehicles"></param>
        /// <returns></returns>
        public Task<int> SaveVehicleFeaturesMappingDetails(VehicleFeaturesMapping objVehicleFeaturesMapping);
    }
}
