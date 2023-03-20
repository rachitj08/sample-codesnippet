using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class RefreshTokenRepository : RepositoryBase<RefreshTokens>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(CloudAcceleratorContext context) : base(context)
        {
            //TODO
        }

        public async Task<RefreshTokens> AddRefreshTokens(RefreshTokens loginHistory)
        {
           return base.Create(loginHistory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RefreshTokens> GetDetails(string token)
        {
            return await base.context.RefreshTokens
                .Where(x => x.Token == token).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateDetail(RefreshTokens model)
        {
            base.context.RefreshTokens.Update(model);
            await base.context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// To Verify Token
        /// </summary>
        /// <param name="token">Token string</param>
        /// <param name="userId">User Id</param>
        /// <param name="accountId">Account Id</param>
        /// <returns></returns>
        public async Task<bool> VerifyToken(string token, long userId, long accountId)
        {
            var result = await base.context.RefreshTokens
                 .Where(x => x.IsRevorked == false && x.IsUsed == false 
                    && x.UserId == userId && x.AccountId == accountId
                    && x.JwtToken == token).AnyAsync();
              
            return result;
        }
    }

}
