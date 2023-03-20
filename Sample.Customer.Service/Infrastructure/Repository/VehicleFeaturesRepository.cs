using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class VehicleFeaturesRepository : RepositoryBase<VehicleFeatures>, IVehicleFeaturesRepository
    {
        public VehicleFeaturesRepository(CloudAcceleratorContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VehicleFeatures>> GetAllVehiclesFeatures(long accountId)
        {
            return await base.GetAll(x=> x.AccountId == accountId);
        }

        public async Task<int> SaveVehicleFeatureDetails(VehicleFeatures objVehicleFeatures)
        {
            base.context.VehicleFeatures.Add(objVehicleFeatures);
            return await base.context.SaveChangesAsync();
        }
    }
}