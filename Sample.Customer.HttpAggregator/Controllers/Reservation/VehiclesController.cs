using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.Reservation;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.Controllers.Reservation
{
    /// <summary>
    /// VehiclesController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiclesController : BaseApiController
    {
        private readonly IVehiclesService _vehiclesService;

        /// <summary>
        /// VehiclesController
        /// </summary>
        /// <param name="vehiclesService"></param>
        /// <param name="logger"></param>
        public VehiclesController(IVehiclesService vehiclesService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(vehiclesService), vehiclesService);
            this._vehiclesService = vehiclesService;
        }

        /// <summary>
        /// Vehicle category and feature controller
        /// </summary>
        /// <returns></returns>
        [Route("GetAllCars/{flag}/{parkingProviderLocationId}/{searchType}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<InComingOutGoingCarsVM>), 200)]
        public async Task<IActionResult> GetAllCars(string flag, long parkingProviderLocationId,int searchType)
        {
            return await Execute(async () =>
            {
                var result = await this._vehiclesService.GetAllCars(flag, parkingProviderLocationId,searchType);
                if (result.ResponseCode != ResponseCode.RecordFetched)
                    return BadRequest(result);
                return Ok(result);
            });
        }

        /// <summary>
        /// GetCarCount
        /// </summary>
        /// <param name="parkingReservationDate"></param>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        [Route("GetCarCount/{parkingReservationDate}/{parkingProviderLocationId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<CarDetailCountVM>), 200)]
        public async Task<IActionResult> GetCarCount(DateTime parkingReservationDate, long parkingProviderLocationId)
        {
            return await Execute(async () =>
            {
                var result = await this._vehiclesService.GetCarCount(parkingReservationDate, parkingProviderLocationId);
                if (result.ResponseCode != ResponseCode.RecordFetched)
                    return BadRequest(result);
                return Ok(result);
            });
        }
        /// <summary>
        /// Get Day Wise Count
        /// </summary>
        /// <param name="parkingProviderLocationId"></param>
        /// <returns></returns>
        [Route("GetDayWiseCount/{parkingProviderLocationId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<CarDetailCountVM>), 200)]
        public async Task<IActionResult> GetDayWiseCount(long parkingProviderLocationId)
        {
            return await Execute(async () =>
            {
                var result = await _vehiclesService.GetDayWiseCount(parkingProviderLocationId);
                if (result.ResponseCode != ResponseCode.RecordFetched)
                    return BadRequest(result);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get Vehicle Detail By Tag Id
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetVehicleDetailByTagId/{tagId}/")]
        [ProducesResponseType(typeof(ResponseResult<VehicleDetailVM>), 200)]
        public async Task<IActionResult> GetVehicleDetailByTagId([FromRoute]string tagId)
        {
            return await Execute(async () =>
            {
                var result = await _vehiclesService.GetVehicleDetailByTagId(tagId);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Create Reservation Vehicle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateReservationVehicle")]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> CreateReservationVehicle(CreateReservationVehicleReqVM model)
        {
            return await Execute(async () =>
            {
                var result = await _vehiclesService.CreateReservationVehicle(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Create Vehicle and Reservation Vehicle details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateVehicle")]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> CreateVehicle(CreateVehicleReqVM model)
        {
            return await Execute(async () =>
            {
                var result = await _vehiclesService.CreateVehicle(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }
       
    }
}
