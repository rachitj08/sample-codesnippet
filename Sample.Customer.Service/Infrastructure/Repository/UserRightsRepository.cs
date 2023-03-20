using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class UserRightsRepository : RepositoryBase<UserRights>, IUserRightsRepository
    {
        public UserRightsRepository(CloudAcceleratorContext context) : base(context)
        {            
        }
        /// <summary>
        /// add
        /// </summary>
        /// <param name="userRight"></param>
        /// <returns></returns>
        public async Task<UserRights> Add(UserRights userRight)
        {           
            base.context.UserRights.Add(userRight);
            await base.context.SaveChangesAsync();
            return userRight;
        }
        /// <summary>
        /// AddList
        /// </summary>
        /// <param name="userRights"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserRights>> AddList(List<UserRights> userRights, long userId)
        {
            if (userId > 0)
            {
                var existingUserRights = await base.context.UserRights.Where(x => x.UserId == userId).ToListAsync();

                if (existingUserRights != null && existingUserRights.Count() > 0)
                    base.context.UserRights.RemoveRange(existingUserRights);
            }

            if (userRights != null && userRights.Count() > 0)
                await base.context.UserRights.AddRangeAsync(userRights);
             
            return userRights;
        }

        /// <summary>
        /// Get user by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> Delete(long userId)
        {
            var res = base.context.UserRights.Where(x => x.UserId == userId).ToList();
            if(res != null && res.Count > 0)
            {
                base.context.UserRights.RemoveRange(res);
                await base.context.SaveChangesAsync();            
            }
            return true;
        }
    }
}
