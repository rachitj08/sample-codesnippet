using Common.Model;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.IServices
{
    /// <summary>
    /// IUserVehiclePreferenceCategoryFeaturesService
    /// </summary>
    public interface IUserVehiclePreferenceCategoryFeaturesService
    {
        /// <summary>
        /// Get User Vehicle Category Features
        /// </summary> 
        /// <returns></returns>
        Task<ResponseResult<VehicleCategoryAndFeatures>> GetUserVehicleCategoryFeatures();

        /// <summary>
        /// AddUserVehicleCategoryFeatures
        /// </summary>
        /// <param name="preferenceCategoryFeaturesVM"></param>
        /// <returns></returns>
        Task<ResponseResult<bool>> SaveUserVehicleCategoryFeatures(UserVehicleCategoryFeaturesVM preferenceCategoryFeaturesVM);
    }
}
