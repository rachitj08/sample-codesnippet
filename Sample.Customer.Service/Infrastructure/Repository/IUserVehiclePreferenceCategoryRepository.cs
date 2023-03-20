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
    public interface IUserVehiclePreferenceCategoryRepository
    {

        /// <summary>
        /// Get User Vehicle Preference Category for User Id
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        Task<List<UserVehiclePreferenceCategory>> GetUserVehiclePreferenceCategoryList(long accountId, long userId);

        /// <summary>
        /// Add User Vehicle Preference Category Repo
        /// </summary>
        /// <param name="userVehiclePreferenceCategories"></param>
        /// <returns></returns>
        Task AddUserVehiclePreferenceCategory(List<UserVehiclePreferenceCategory> userVehiclePreferenceCategories);

        /// <summary>
        /// Delete User Vehicle Preference Category Repo
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        void DeleteUserVehiclePreferenceCategory(long accountId, long userId);
    }
}
