using Sample.Admin.Service.Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Utility;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class ApplicationConfigRepository : RepositoryBase<ApplicationConfigs>, IApplicationConfigRepository
    {
        

        public ApplicationConfigRepository(CloudAcceleratorContext context) : base(context)
        {
           
        }
         

        public async Task<ApplicationConfigs> GetApplicationConfig()
        {
            return await context.ApplicationConfigs.FirstOrDefaultAsync();
        }
    }
}
