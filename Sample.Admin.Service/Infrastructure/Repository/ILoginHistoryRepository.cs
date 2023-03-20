using Sample.Admin.Service.Infrastructure.DataModels;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface ILoginHistoryRepository
    {
        Task<LoginHistories> AddLoginHistory(LoginHistories loginHistory);
        Task<LoginHistories> GetLoginHistory(string token);
        Task<bool> UpdateLoginHistory(LoginHistories model);
    }
}
