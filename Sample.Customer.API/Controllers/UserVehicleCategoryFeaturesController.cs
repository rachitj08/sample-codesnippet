using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVehicleCategoryFeaturesController : BaseApiController
    {
        private readonly IUserVehiclePreferenceCategoryFeaturesService userVehiclePreferenceCategoryFeaturesService;

        /// <summary>
        /// UserVehiclePreference Controller constructor to Inject dependency
        /// </summary>
        /// <param name="userService">user service class</param>
        public UserVehicleCategoryFeaturesController(IUserVehiclePreferenceCategoryFeaturesService userVehiclePreferenceCategoryFeaturesService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(userVehiclePreferenceCategoryFeaturesService), userVehiclePreferenceCategoryFeaturesService);
            
            this.userVehiclePreferenceCategoryFeaturesService = userVehiclePreferenceCategoryFeaturesService;
        }


        /// <summary>
        /// Save User Vehicle Category Features
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getUserVehicleCategoryFeatures")]
        public async Task<IActionResult> GetUserVehicleCategoryFeatures()
        { 
            return await Execute(async () =>
            {
                var result = await this.userVehiclePreferenceCategoryFeaturesService.GetUserVehicleCategoryFeatures(loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Save User Vehicle Category Features
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveUserVehicleCategoryFeatures")]
        public async Task<IActionResult> SaveUserVehicleCategoryFeatures([FromBody] UserVehicleCategoryFeaturesVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.userVehiclePreferenceCategoryFeaturesService.SaveUserVehicleCategoryFeatures(model, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

    }
}
