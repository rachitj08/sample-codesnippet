using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    /// <summary>
    /// IUserGroupMappingRepository
    /// </summary>
    public interface IUserGroupMappingRepository
    {
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="userGroupMapping"></param>
        /// <returns></returns>
        Task<UserGroupMappings> Add(UserGroupMappings userGroupMapping);
        /// <summary>
        /// AddGroupMapping
        /// </summary>
        /// <param name="userGroupMapping"></param>
        /// <returns></returns>
        Task<UserGroupMappings> AddGroupMapping(UserGroupMappings userGroupMapping);
        /// <summary>
        /// AddList
        /// </summary>
        /// <param name="userGroups"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserGroupMappings>> AddList(List<UserGroupMappings> userGroups, long userId);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> Delete(long userId);
    }
}
