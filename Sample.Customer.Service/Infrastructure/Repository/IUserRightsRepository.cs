using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IUserRightsRepository
    {
        Task<UserRights> Add(UserRights userRight);
        Task<List<UserRights>> AddList(List<UserRights> userRights, long userId);
        Task<bool> Delete(long userId);
    }
}
