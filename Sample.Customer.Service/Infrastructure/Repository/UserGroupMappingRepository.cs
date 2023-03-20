using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    /// <summary>
    /// UserGroupMappingRepository
    /// </summary>
    public class UserGroupMappingRepository : RepositoryBase<UserGroupMappings>, IUserGroupMappingRepository
    {
        /// <summary>
        /// UserGroupMappingRepository
        /// </summary>
        /// <param name="context"></param>
        public UserGroupMappingRepository(CloudAcceleratorContext context) : base(context)
        {
        }
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="userGroupMapping"></param>
        /// <returns></returns>
        public async Task<UserGroupMappings> Add(UserGroupMappings userGroupMapping)
        {
            base.context.UserGroupMappings.Add(userGroupMapping);
            await base.context.SaveChangesAsync();
            return userGroupMapping;
        }
        /// <summary>
        /// AddGroupMapping
        /// </summary>
        /// <param name="userGroupMapping"></param>
        /// <returns></returns>
        public async Task<UserGroupMappings> AddGroupMapping(UserGroupMappings userGroupMapping)
        {
            base.context.UserGroupMappings.Add(userGroupMapping);
            return userGroupMapping;
        }
        /// <summary>
        /// AddList
        /// </summary>
        /// <param name="userGroups"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserGroupMappings>> AddList(List<UserGroupMappings> userGroups, long userId)
        {
            if (userId > 0 && userGroups.Count> 0)
            {
                var existingGroups = await base.context.UserGroupMappings.Where(x => x.UserId == userId).ToListAsync();

                if (existingGroups != null && existingGroups.Count() > 0)
                    base.context.UserGroupMappings.RemoveRange(existingGroups);
            }

            if (userGroups != null && userGroups.Count() > 0)
                await base.context.UserGroupMappings.AddRangeAsync(userGroups);

            return userGroups;
        }

        /// <summary>
        /// Delete by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> Delete(long userId)
        {
            var res = base.context.UserGroupMappings.Where(x => x.UserId == userId).ToList();
            if (res != null && res.Count > 0)
            {
                base.context.UserGroupMappings.RemoveRange(res);
                base.context.SaveChanges();
            }
            return true;
        }
    }
}
