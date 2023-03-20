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
using Sample.Customer.Model.Model.Reservation;
using System;

namespace Sample.Customer.HttpAggregator.Controllers
{
    /// <summary>
    /// 
    /// </summary>
   
    [Route("api/ParkingProvider")]
    [ApiController]
    [Authorize]
    public class ParkingProviderController : BaseApiController
    {
       
        private readonly IParkingProviderService parkingProviderService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parkingProviderService"></param>
        /// <param name="logger"></param>
        public ParkingProviderController(IParkingProviderService parkingProviderService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(parkingProviderService), parkingProviderService);
            this.parkingProviderService = parkingProviderService;
        }
        /// <summary>
        /// Get Parking Provider
        /// </summary>
        /// <returns></returns>
        [Route("getsource")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetSource()
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetSource();
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// GetCountry
        /// </summary>
        /// <returns></returns>
        [Route("getcountry")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<CountryVM>>), 200)]
        public async Task<IActionResult> GetCountry()
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetCountry();
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// GetState
        /// </summary>
        /// <returns></returns>
        [Route("getstate/{countryId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<StateVM>>), 200)]
        public async Task<IActionResult> GetState(long countryId)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetState(countryId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// GetCity
        /// </summary>
        /// <returns></returns>
        [Route("getcity/{stateId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<CityVM>>), 200)]
        public async Task<IActionResult> GetCity(long stateId)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetCity(stateId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// GetProviderReservation
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        [Route("getproviderreservation/{reservationId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetProviderReservation(long reservationId)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetProviderReservation(reservationId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// GetProviderReservation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetAllProviderReservation/{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetAllProviderReservation(long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetAllProviderReservation(userId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }


        /// <summary>
        /// Create Reservation
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("createreservation")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> CreateReservation([FromBody] UpsertProviderReservationVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.CreateReservation(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }
        /// <summary>
        /// Create Reservation
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updatereservation")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> UpdateReservation([FromBody] UpsertProviderReservationVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.UpdateReservation(model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        [Route("getparkingpricedetail")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetParkingPriceDetail(ParkingRateReqVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetParkingPriceDetail(model);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        [Route("getparkingspotbylocationandspottype/{providerLocationId}/{spotType}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetParkingSpotByLocationandSpotType(long providerLocationId,long spotType)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetParkingSpotByLocationandSpotType(providerLocationId,spotType);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        [Route("getparkingspottype")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<DropDownMaster>>), 200)]
        public async Task<IActionResult> GetParkingSpotType()
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.GetParkingSpotType();
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        [Route("checkinvehicle")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> CheckInVehicle(CheckInCheckOutVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.CheckInVehicle(model);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        [Route("checkoutvehicle")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> CheckOutVehicle(CheckInCheckOutVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.parkingProviderService.CheckOutVehicle(model);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }

    }
}
