//using Sample.Admin.Service.Infrastructure.DataModels;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//// Remove Table from Database
//namespace Sample.Admin.Service.Infrastructure.Repository
//{
//    public class AuthenticationTypeRepository : RepositoryBase<AuthenticationTypes>, IAuthenticationTypeRepository
//    {
//        public AuthenticationTypeRepository(CloudAcceleratorContext context) : base(context)
//        {
//            //TODO
//        }
//        /// <summary>
//        /// Get All Authentication Types
//        /// </summary>
//        /// <returns></returns>
//        public async Task<IEnumerable<AuthenticationTypes>> GetAllAuthenticationTypes()
//        {
//            var result = base.context.AuthenticationTypes.ToList();
//            return result;
//        }

//    }
//}
