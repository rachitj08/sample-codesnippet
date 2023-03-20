using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.ServiceWorker.ParkingProvider;
using Sample.Customer.Model;

namespace Sample.Customer.API.Controllers.ParkingProvider
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingProviderController : BaseApiController
    {
        private readonly IParkingProviderService parkingProviderService;

        public ParkingProviderController(IParkingProviderService parkingProviderService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(parkingProviderService), parkingProviderService);
            this.parkingProviderService = parkingProviderService;
        }
        [Route("getcountry")]
        [HttpGet]
        public async Task<IActionResult> GetCountry()
        {
            return await Execute(async () =>
            {
                var countries = this.parkingProviderService.GetAllCountry();
                return Ok(countries);
            });

        }
        [Route("getstate/{countryId}")]
        [HttpGet]
        public async Task<IActionResult> GetState(long countryId)
        {
            return await Execute(async () =>
            {
                var countries =await this.parkingProviderService.GetStateByCountryId(countryId);
                return Ok(countries);
            });

        }
        [Route("getcity/{stateId}")]
        [HttpGet]
        public async Task<IActionResult> GetCity(long stateId)
        {
            return await Execute(async () =>
            {
                var countries =await this.parkingProviderService.GetCityByStateId(stateId);
                return Ok(countries);
            });

        }
        [Route("getsource")]
        [HttpGet]
        public async Task<IActionResult> GetSource()
        {
            return await Execute(async () =>
            {
                var countries = this.parkingProviderService.GetSource();
                return Ok(countries);
            });

        }
        [Route("createreservation")]
        [HttpPost]
        public async Task<IActionResult> CreateReservation(UpsertProviderReservationVM model)
        {
            return await Execute(async () =>
            {
                var reservation =await this.parkingProviderService.CreateReservation(model,loggedInUserId,loggedInAccountId);
                return Ok(reservation);
            });

        }
        [Route("getproviderreservation/{reservationId}")]
        [HttpGet]
        public async Task<IActionResult> GetProviderReservation(long reservationId)
        {
            return await Execute(async () =>
            {
                var countries = await this.parkingProviderService.GetProviderReservation(reservationId, loggedInUserId,loggedInAccountId);
                return Ok(countries);
            });

        }
        [Route("getallproviderreservation/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetAllProviderReservation(long userId)
        {
            return await Execute(async () =>
            {
                var countries = await this.parkingProviderService.GetAllProviderReservation(userId, loggedInAccountId);
                return Ok(countries);
            });

        }
        [Route("updatereservation")]
        [HttpPost]
        public async Task<IActionResult> UpdateReservation(UpsertProviderReservationVM model)
        {
            return await Execute(async () =>
            {
                var reservation =await this.parkingProviderService.UpdateReservation(model,loggedInUserId,loggedInAccountId);
                return Ok(reservation);
            });

        }

        [Route("getparkingpricedetail")]
        [HttpPost]
        public async Task<IActionResult> GetParkingPriceDetail(ParkingRateReqVM model)
        {
            return await Execute(async () =>
            {
                var result = this.parkingProviderService.GetParkingPriceDetail(model, loggedInAccountId);
                return Ok(result);

            });
        }

        [Route("getparkingspotbylocationandspottype/{providerLocationId}/{spotType}")]
        [HttpGet]
        public async Task<IActionResult> GetParkingSpotByLocationandSpotType(long providerLocationId, long spotType)
        {
            return await Execute(async () =>
            {
                var result = this.parkingProviderService.GetParkingSpotByLocationandSpotType(providerLocationId, spotType,loggedInAccountId);
                return Ok(result);

            });
        }
        [Route("getparkingspottype")]
        [HttpGet]
        public async Task<IActionResult> GetParkingSpotType()
        {
            return await Execute(async () =>
            {
                var result = this.parkingProviderService.GetParkingSpotType(loggedInAccountId);
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
                var result = await this.parkingProviderService.CheckInVehicle(model,loggedInUserId,loggedInAccountId);
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
                var result = await this.parkingProviderService.CheckOutVehicle(model,loggedInUserId,loggedInAccountId);
                return Ok(result);

            });
        }
    }
}
