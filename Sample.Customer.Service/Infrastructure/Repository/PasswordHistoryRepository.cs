using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class PasswordHistoryRepository : RepositoryBase<PasswordHistories>, IPasswordHistoryRepository
    {
        public PasswordHistoryRepository(CloudAcceleratorContext context) : base(context)
        {
            //TODO
        }

        /// <summary>
        /// Get PasswordHistory By UserId
        /// </summary>
        /// <param name="userId">The UserId to get Password History</param>
        /// <param name="count">Total number of record to get Password History order by last update desc</param>
        /// <returns></returns>
        public async Task<List<PasswordHistories>> GetPasswordHistoryByUserId(long userId, int count)
        {
            return await base.context.PasswordHistories.Where(x => x.UserId == userId)
                .OrderByDescending(x=>x.LastUsedOn)
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// Create PasswordHistory
        /// </summary>
        /// <param name="PasswordHistory">Password History details</param>
        /// <returns></returns>
        public async Task<PasswordHistories> CreatePasswordHistory(PasswordHistories passwordHistory)
        {
            return base.Create(passwordHistory);
        }
    }
}
