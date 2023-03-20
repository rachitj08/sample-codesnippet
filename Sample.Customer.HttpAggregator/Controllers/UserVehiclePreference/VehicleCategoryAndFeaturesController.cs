using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Controllers
{
    /// <summary>
    /// VehicleCategoryAndFeatures Controller
    /// </summary>
    /// 
    
    public class VehicleCategoryAndFeaturesController : BaseApiController
    {
        private readonly IVehicleCategoryAndFeatureService VehicleCategoryAndFeatureService;
        /// <summary>
        /// VehicleCategoryAndFeatures controller constructor to inject dependency
        /// </summary>
        /// <returns></returns>

        public VehicleCategoryAndFeaturesController(IVehicleCategoryAndFeatureService VehicleCategoryAndFeatureService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(VehicleCategoryAndFeatureService), VehicleCategoryAndFeatureService);
            this.VehicleCategoryAndFeatureService = VehicleCategoryAndFeatureService;
        }

        /// <summary>
        /// Get Vehicle category and feature
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<VehicleCategoryAndFeatures>), 200)]
        public async Task<IActionResult> GetVehicleCategoryAndFetures()
        {
            return await Execute(async () =>
            {
                var vehicleData = await VehicleCategoryAndFeatureService.GetVehicleCategoryAndFeatures();
                if (vehicleData.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(vehicleData);
                else
                    return BadRequest(vehicleData);
            });
        }
    }
}
