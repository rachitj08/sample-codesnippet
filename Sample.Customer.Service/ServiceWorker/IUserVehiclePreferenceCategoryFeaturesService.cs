using Sample.Customer.Service.Infrastructure.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Common.Model;
using User = Sample.Customer.Service.Infrastructure.DataModels.Users;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IUserVehiclePreferenceCategoryFeaturesService
    {
        Task<ResponseResult<UserVehicleCategoryFeaturesVM>> GetUserVehicleCategoryFeatures(long loggedAccountId, long loggedInUserId);

        Task<ResponseResult<bool>> SaveUserVehicleCategoryFeatures(UserVehicleCategoryFeaturesVM model, long loggedAccountId, long loggedInUserId);
    }
}
