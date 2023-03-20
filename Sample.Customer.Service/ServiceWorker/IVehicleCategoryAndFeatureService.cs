using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.Service.ServiceWorker
{
    /// <summary>
    /// vehicle category and feature service interface
    /// </summary>
    public interface IVehicleCategoryAndFeatureService
    {
        Task<ResponseResult<VehicleCategoryAndFeatures>> GetVehicleCategoryAndFeatures(long accountId);
    }
}