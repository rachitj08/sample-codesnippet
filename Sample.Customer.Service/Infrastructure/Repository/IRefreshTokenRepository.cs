using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokens> AddRefreshTokens(RefreshTokens loginHistory);

        Task<RefreshTokens> GetDetails(string token);

        Task<bool> UpdateDetail(RefreshTokens model);

        /// <summary>
        /// To Verify Token
        /// </summary> 
        /// <param name="token">Token string</param>
        /// <param name="userId">User Id</param>
        /// <param name="accountId">Account Id</param>
        /// <returns></returns>
        Task<bool> VerifyToken(string token, long userId, long accountId);
    }
}
