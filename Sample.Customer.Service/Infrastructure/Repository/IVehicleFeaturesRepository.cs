using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IVehicleFeaturesRepository
    {
        Task<IEnumerable<VehicleFeatures>> GetAllVehiclesFeatures(long accountId);

        /// <summary>
        /// SaveVehicleFeatureDetails
        /// </summary>
        /// <param name="objVehicles"></param>
        /// <returns></returns>
        public Task<int> SaveVehicleFeatureDetails(VehicleFeatures objVehicleFeatures);
    }
}