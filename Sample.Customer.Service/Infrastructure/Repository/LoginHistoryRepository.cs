using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{

    public class LoginHistoryRepository : RepositoryBase<LoginHistories>, ILoginHistoryRepository
    {
        public LoginHistoryRepository(CloudAcceleratorContext context) : base(context)
        {
            //TODO
        }

        public async Task<LoginHistories> AddLoginHistory(LoginHistories loginHistory)
        {
            return base.Create(loginHistory);
        }

        public async Task<LoginHistories> GetLoginHistory(string token)
        {
            return await base.context.LoginHistories.Where(x => x.Token == token).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateLoginHistory(LoginHistories model)
        {
            base.context.LoginHistories.Update(model);
            await base.context.SaveChangesAsync();
            return true;
        }
    }
}
