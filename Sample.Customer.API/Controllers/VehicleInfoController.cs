using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model.VinDecoder;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleInfoController : BaseApiController
    {
        private readonly IVehicleInfoService vehicleInfoService;
        public VehicleInfoController(IVehicleInfoService vehicleInfoService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(vehicleInfoService), vehicleInfoService);

            this.vehicleInfoService = vehicleInfoService;
        }

        [HttpPost]
        [Route("GetVehicleInfo")]
        public async Task<IActionResult> GetVehicleInfo([FromBody] VINDecoderVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.vehicleInfoService.GetVehicleInfo(model.VIN);
                return Ok(result);
            });
        }
    }
}
