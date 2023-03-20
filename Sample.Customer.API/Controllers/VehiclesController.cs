using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : BaseApiController
    {
        private readonly IVehiclesService _vehiclesService;
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
        public async Task<IActionResult> GetAllCars(string flag, long parkingProviderLocationId, int searchType)
        {
            return await Execute(async () =>
            {
                var result =  this._vehiclesService.GetAllCars(flag, parkingProviderLocationId, loggedInAccountId, searchType);
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
        
        [Route("GetCarCount/{parkingReservationDate}/{parkingProviderLocationId}")]
        [HttpGet]
        public async Task<IActionResult> GetCarCount(DateTime parkingReservationDate, long parkingProviderLocationId)
        {
            return await Execute(async () =>
            {
                var result =  this._vehiclesService.GetCarCount(parkingReservationDate, parkingProviderLocationId, loggedInAccountId);
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
        [Route("GetDayWiseCount/{parkingProviderLocationId}")]
        [HttpGet]
        public async Task<IActionResult> GetDayWiseCount(long parkingProviderLocationId)
        {
            return await Execute(async () =>
            {
                var result =  this._vehiclesService.GetDayWiseCount(parkingProviderLocationId, loggedInAccountId);
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


        [Route("GetVehicleDetailByTagId/{tagId}/")]
        [HttpGet]
        public async Task<IActionResult> GetVehicleDetailByTagId([FromRoute]string tagId)
        {
            return await Execute(async () =>
            {
                var result = await this._vehiclesService.GetVehicleDetailByTagId(tagId, loggedInAccountId);               
                return Ok(result);
            });
        }

        [Route("CreateReservationVehicle")]
        [HttpPost]
        public async Task<IActionResult> CreateReservationVehicle(CreateReservationVehicleReqVM model)
        {
            return await Execute(async () =>
            {
                var result = await this._vehiclesService.CreateReservationVehicle(model, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        [Route("CreateVehicle")]
        [HttpPost]
        public async Task<IActionResult> CreateVehicle(CreateVehicleReqVM model)
        {
            return await Execute(async () =>
            {
                var result = await this._vehiclesService.CreateVehicle(model, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }
    }
}

