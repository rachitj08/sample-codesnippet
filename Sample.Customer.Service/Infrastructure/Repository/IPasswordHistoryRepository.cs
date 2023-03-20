using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IPasswordHistoryRepository
    {
        /// <summary>
        /// Get PasswordHistory By UserId
        /// </summary>
        /// <param name="userId">The UserId to get Password History</param>
        /// <param name="count">Total number of record to get Password History order by last update desc</param>
        /// <returns></returns>
        Task<List<PasswordHistories>> GetPasswordHistoryByUserId(long userId, int count);

        /// <summary>
        /// Create PasswordHistory
        /// </summary>
        /// <param name="PasswordHistories">Password History details</param>
        /// <returns></returns>
        Task<PasswordHistories> CreatePasswordHistory(PasswordHistories passwordHistory);
    }
}
