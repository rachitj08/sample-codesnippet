using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.IServices
{
    /// <summary>
    /// Interface for Vehicle category and features
    /// </summary>
    public interface IVehicleCategoryAndFeatureService
    {
        /// <summary>
        /// Get Vehicle category and features
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<VehicleCategoryAndFeatures>> GetVehicleCategoryAndFeatures();
    }
}
