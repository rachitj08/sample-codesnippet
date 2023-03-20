using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{

    [Route("api/vehiclefeatures")]
    [ApiController]
    public class VehicleFeaturesController : BaseApiController
    {
        private readonly IVehicleCategoryAndFeatureService vehicleCategoryAndFeatureService;
        public VehicleFeaturesController(IVehicleCategoryAndFeatureService vehicleCategoryAndFeatureService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(vehicleCategoryAndFeatureService), vehicleCategoryAndFeatureService);
            this.vehicleCategoryAndFeatureService = vehicleCategoryAndFeatureService;
        }

        /// <summary>
        /// Vehicle category and feature controller
        /// </summary>
        /// <returns></returns>
      
        [Route("getvehiclecategoryandfeatures")]
        [HttpGet]
        public async Task<IActionResult> GetVehicleCategoryAndFeatures()
        {
            return await Execute(async () =>
            {
                var result = await this.vehicleCategoryAndFeatureService.GetVehicleCategoryAndFeatures(loggedInAccountId);
                if (result == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError
                    });
                }
                return Ok(result);
            });
        }
    }
}
