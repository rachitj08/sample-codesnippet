using Sample.Admin.Service.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class RefreshTokenRepository : RepositoryBase<AdminRefreshTokens>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(CloudAcceleratorContext context) : base(context)
        {
            //TODO
        }

        public async Task<AdminRefreshTokens> AddRefreshTokens(AdminRefreshTokens refreshToken)
        {
            return base.Create(refreshToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AdminRefreshTokens> GetDetails(string token)
        {
            return await base.context.AdminRefreshTokens
                .Where(x => x.Token == token).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateDetail(AdminRefreshTokens model)
        {
            base.context.AdminRefreshTokens.Update(model);
            await base.context.SaveChangesAsync();
            return true;
        }
    }
}
