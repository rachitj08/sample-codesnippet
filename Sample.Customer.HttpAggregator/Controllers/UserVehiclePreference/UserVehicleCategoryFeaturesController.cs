using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Controllers.UserManagement
{
    /// <summary>
    /// Users Controller
    /// </summary>
    [Route("api/uservehiclecategoryfeatures")]
    [ApiController]
    [Authorize]
    public class UserVehicleCategoryFeaturesController : BaseApiController
    {
        private readonly IUserVehiclePreferenceCategoryFeaturesService userVehiclePreferenceCategoryFeaturesService;

        /// <summary>
        /// UserVehiclePreferenceCategoryFeaturesController
        /// </summary>
        /// <param name="userVehiclePreferenceCategoryFeaturesService"></param>
        /// <param name="logger"></param>
        public UserVehicleCategoryFeaturesController(IUserVehiclePreferenceCategoryFeaturesService userVehiclePreferenceCategoryFeaturesService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(userVehiclePreferenceCategoryFeaturesService), userVehiclePreferenceCategoryFeaturesService);

            this.userVehiclePreferenceCategoryFeaturesService = userVehiclePreferenceCategoryFeaturesService;
        }

        /// <summary>
        /// Get User Vehicle Category Features
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("getuservehiclecategoryfeatures")]
        [ProducesResponseType(typeof(ResponseResultList<VehicleCategoryAndFeatures>), 200)]
        public async Task<IActionResult> GetUserVehicleCategoryFeatures()
        { 

            return await Execute(async () =>
            {
                var result = await userVehiclePreferenceCategoryFeaturesService.GetUserVehicleCategoryFeatures();
                if (result.ResponseCode == ResponseCode.RecordFetched)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            });
        }

        /// <summary>
        /// AddUserVehicleCategoryFeatures
        /// </summary>
        /// <param name="userVehiclePreferenceCategory"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("saveUservehiclecategoryfeatures")]
        [ProducesResponseType(typeof(ResponseResultList<bool>), 200)]
        public async Task<IActionResult> SaveUserVehicleCategoryFeatures([FromBody] UserVehicleCategoryFeaturesVM userVehiclePreferenceCategory)
        {
            Check.Argument.IsNotNull(nameof(userVehiclePreferenceCategory), userVehiclePreferenceCategory);

            return await Execute(async () =>
            {
                var result = await userVehiclePreferenceCategoryFeaturesService.SaveUserVehicleCategoryFeatures(userVehiclePreferenceCategory);
                if(result.ResponseCode == ResponseCode.RecordSaved)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }); 
        }

    }
}
