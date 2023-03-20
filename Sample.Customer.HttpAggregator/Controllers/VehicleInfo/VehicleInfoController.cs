using Common.Model;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.VehicleInfo;
using Sample.Customer.Model.VinDecoder;

namespace Sample.Customer.HttpAggregator.Controllers.VehicleInfo
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleInfoController : BaseApiController
    {
        private readonly IVehicleInfoService vehicleInfoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleInfoService"></param>
        /// <param name="logger"></param>
        public VehicleInfoController(IVehicleInfoService vehicleInfoService,IFileLogger logger) : base(logger)
        {
            Check.Argument.IsNotNull(nameof(vehicleInfoService), vehicleInfoService);
            Check.Argument.IsNotNull(nameof(logger), logger);
            this.vehicleInfoService = vehicleInfoService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVehicleInfo")]
        public async Task<IActionResult> GetVehicleInfo([FromBody] VINDecoderVM model)
        {
            return await Execute(async () =>
            {
                var userGroups = await this.vehicleInfoService.GetVehicleInfo(model);
                return Ok(userGroups);
            });
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("GetVehicleInfo")]
        //public async Task<IActionResult> GetVehicleInfo([FromBody] VINDecoderVM model)
        //{
           
        //    return await Execute(async () =>
        //    {
        //        var result = vehicleInfoService.GetVehicleInfo(model);
        //        if (result.ResponseCode != ResponseCode.RecordFetched)
        //        {
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return BadRequest(result);
        //        }
        //    });
        //}
    }
}
