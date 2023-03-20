using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Sample.Customer.Model.Model.StorageModel;
using System.Collections.Generic;
using Common.Enum.StorageEnum;
using Microsoft.Extensions.Options;
using Sample.Customer.Model.Model;
using Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Logger;
using Utilities;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.HttpAggregator.IServices.ParkingProvider;

namespace Sample.Customer.HttpAggregator.Controllers
{
    /// <summary>
    /// 
    /// </summary>
   
    [Route("api/QRGenerator")]
    [ApiController]
    [Authorize]
    public class QRGeneratorController  : BaseApiController
    {
       
        private readonly IParkingProviderService parkingProviderService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingProviderService"></param>
        /// <param name="logger"></param>
        public QRGeneratorController(IParkingProviderService parkingProviderService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(parkingProviderService), parkingProviderService);
            this.parkingProviderService = parkingProviderService;
        }
        /// <summary>
        /// Get Parking Provider
        /// </summary>
        /// <returns></returns>
        [Route("getparkingprovider")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetParkingProvider()
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetParkingProvider();
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// Get Activity Code
        /// </summary>
        /// <returns></returns>
        [Route("getactivitycode")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetActivityCode()
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetActivityCode();
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// Get Parking Provider Location
        /// </summary>
        /// <returns></returns>
        [Route("getparkingproviderlocation/{providerId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetParkingProviderLocation(long providerId)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetParkingProviderLocation(providerId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// Get Sub Location
        /// </summary>
        /// <returns></returns>
        [Route("getsublocation/{parkingProviderlocationId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetSubLocation(long parkingProviderlocationId)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetSubLocation(parkingProviderlocationId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingProviderlocationId"></param>
        /// <returns></returns>
        [Route("getqrlist/{parkingProviderlocationId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetQRList(long parkingProviderlocationId)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetQRList(parkingProviderlocationId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// Get Parking SpotId
        /// </summary>
        /// <returns></returns>
        [Route("getparkingspotid/{subLocationId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetParkingSpotId(long subLocationId)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetParkingSpotId(subLocationId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }

        /// <summary>
        /// Update User Profile Image
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadandsaveqr")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> UploadandSaveQR([FromBody] QRUploadVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.UploadandSaveQR(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        [Route("getsublocationtype")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetSubLocationType()
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetSubLocationType();
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
    }
}
