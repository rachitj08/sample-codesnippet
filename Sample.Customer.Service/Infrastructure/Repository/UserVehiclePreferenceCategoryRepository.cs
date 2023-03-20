using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class UserVehiclePreferenceCategoryRepository : RepositoryBase<UserVehiclePreferenceCategory>, IUserVehiclePreferenceCategoryRepository
    {

        private readonly IMapper mapper;

        public UserVehiclePreferenceCategoryRepository(CloudAcceleratorContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Get User Vehicle Preference Category for User Id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        public async Task<List<UserVehiclePreferenceCategory>> GetUserVehiclePreferenceCategoryList(long accountId, long userId)
        {
            return await base.GetAll(x=> x.LoggedInUserId == userId && x.AccountId == accountId);
        }

        /// <summary>
        /// Add User Vehicle Preference Category Repo
        /// </summary>
        /// <param name="userVehiclePreferenceCategories"></param>
        /// <returns></returns>
        public async Task AddUserVehiclePreferenceCategory(List<UserVehiclePreferenceCategory> userVehiclePreferenceCategories)
        {
            await AddRange(userVehiclePreferenceCategories);
        }

        /// <summary>
        /// Delete User Vehicle Preference Category Repo
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public void DeleteUserVehiclePreferenceCategory(long accountId, long userId)
        {
            DeleteRange(x => x.LoggedInUserId == userId && x.AccountId == accountId);
        }
    }
}
