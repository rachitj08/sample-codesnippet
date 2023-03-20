using Common.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Model;
using User = Sample.Customer.Service.Infrastructure.DataModels.Users;
using VerifyTokenModel = Sample.Customer.Model.VerifyTokenModel;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IUserVehiclePreferenceFeaturesRepository
    {

        /// <summary>
        /// Get User Vehicle Preference Features for user id
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserVehiclePreferenceFeatures>> GetUserVehiclePreferenceFeatures(long accountId, long userId);

        /// <summary>
        /// Add User Vehicle Preference Features for repo
        /// </summary>
        /// <param name="userVehiclePreferenceCategory"></param>
        /// <returns></returns>
        Task AddUserVehiclePreferenceFeatures(List<UserVehiclePreferenceFeatures> userVehiclePreferenceCategories);

        /// <summary>
        /// Delte User Vehicle Preference Features for repo
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        void DeletUserVehiclePreferenceFeatures(long accountId, long userId);
    }
}
