using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface ILoginHistoryRepository
    {
        Task<LoginHistories> AddLoginHistory(LoginHistories loginHistory);
        Task<LoginHistories> GetLoginHistory(string token);
        Task<bool> UpdateLoginHistory(LoginHistories model);
    }
}
