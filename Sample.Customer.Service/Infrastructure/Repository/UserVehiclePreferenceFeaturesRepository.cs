using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class UserVehiclePreferenceFeaturesRepository : RepositoryBase<UserVehiclePreferenceFeatures>, IUserVehiclePreferenceFeaturesRepository
    {
        private readonly IMapper mapper;

        public UserVehiclePreferenceFeaturesRepository(CloudAcceleratorContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Get User Vehicle Preference Features for user id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserVehiclePreferenceFeatures>> GetUserVehiclePreferenceFeatures(long accountId, long userId)
        {
            return await GetAll(x=> x.LoggedInUserId == userId && x.AccountId == accountId);
        }

        /// <summary>
        /// Add User Vehicle Preference Features for repo
        /// </summary>
        /// <param name="userVehiclePreferenceCategory"></param>
        /// <returns></returns>
        public async Task AddUserVehiclePreferenceFeatures(List<UserVehiclePreferenceFeatures> userVehiclePreferenceCategories)
        {
            await base.AddRange(userVehiclePreferenceCategories); 
        }

        /// <summary>
        /// Delte User Vehicle Preference Features for repo
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        public void DeletUserVehiclePreferenceFeatures(long accountId, long userId)
        { 
            DeleteRange(x=> x.LoggedInUserId == userId && x.AccountId == accountId); 
        }
    }
}
