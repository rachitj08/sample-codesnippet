using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IVehicleCategoryRepository
    {
        Task<IEnumerable<VehicleCategory>> GetAllVehiclesCategory(long accountId);

        Task<VehicleCategory> GetVehicleCategoryByName(string name, long accountId);
    }
}