using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class VehicleCategoryRepository : RepositoryBase<VehicleCategory>, IVehicleCategoryRepository
    {
        public VehicleCategoryRepository(CloudAcceleratorContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VehicleCategory>> GetAllVehiclesCategory(long accountId)
        {
            return await base.GetAll(x=> x.AccountId == accountId);
        }
        
        public async Task<VehicleCategory> GetVehicleCategoryByName(string name, long accountId)
        {
            return await SingleAsnc(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.AccountId == accountId);
        }
    }
}