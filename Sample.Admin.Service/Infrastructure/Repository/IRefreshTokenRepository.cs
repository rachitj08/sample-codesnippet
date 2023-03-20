using Sample.Admin.Service.Infrastructure.DataModels;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<AdminRefreshTokens> AddRefreshTokens(AdminRefreshTokens refreshToken);

        Task<AdminRefreshTokens> GetDetails(string token);

        Task<bool> UpdateDetail(AdminRefreshTokens model);
    }
}
