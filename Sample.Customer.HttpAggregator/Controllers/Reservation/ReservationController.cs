using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.HttpAggregator.IServices.Reservation;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.Reservation;

namespace Sample.Customer.HttpAggregator.Controllers.Reservation
{
    /// <summary>
    /// ReservationController
    /// </summary>
    [Route("api/Reservation")]
    [ApiController]
    [Authorize]
    public class ReservationController : BaseApiController
    {
        private readonly IReservationService reservationService;

        /// <summary>
        /// Users Controller Constructor to inject services
        /// </summary>
        /// <param name="reservationService">The user service</param>
        /// <param name="logger">The file logger</param>
        public ReservationController(IReservationService reservationService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(reservationService), reservationService);
            this.reservationService = reservationService;
        }

        /// <summary>
        /// GetAllReservations 
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        [Route("GetAllReservations")]
        [HttpGet]
        public async Task<IActionResult> GetAllReservations(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            return await Execute(async () =>
            {
                var userGroups = await this.reservationService.GetAllReservations(ordering, offset, pageSize, pageNumber, all, headerAccountId);
                return Ok(userGroups);
            });
        }

        /// <summary>
        /// GetReservationById
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetReservationById/{reservationid}")]
        public async Task<IActionResult> GetReservationById(long reservationid)
        {
            return await Execute(async () =>
            {
                var userGroups = await this.reservationService.GetReservationById(reservationid);
                return Ok(userGroups);
            });
        }

        /// <summary>
        /// Create Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateFlightAndParkingReservation")]
        [ProducesResponseType(typeof(ResponseResult<ParkingReserAmountAndLocationVM>), 200)]
        public async Task<IActionResult> CreateFlightAndParkingReservation([FromBody] AddUpdateFlightAndParkingReservationVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.CreateReservation(model, loggedInUserId, headerAccountId);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        /// Update Reservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateFlightAndParkingReservation/{reservationid}")]
        [ProducesResponseType(typeof(ResponseResult<ParkingReserAmountAndLocationVM>), 200)]
        public async Task<IActionResult> UpdateReservation([FromRoute] long reservationid, [FromBody] AddUpdateFlightAndParkingReservationVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.UpdateReservation(reservationid, model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        /// Delete Reservation
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        [Route("DeleteReservation/{reservationId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteReservation([FromRoute] long reservationId)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.DeleteReservation(reservationId);
                if (result.ResponseCode == ResponseCode.RecordDeleted)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Cancel Reservation by id
        /// </summary>
        /// <param name="reservationid"></param>
        /// <returns></returns>
        [Route("CancelReservation/{reservationid}")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> CancelReservation(long reservationid)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.CancelReservation(reservationid);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        /// Confirm Email Itinerary Reservation
        /// </summary>
        /// <param name="flightReservationId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("confirmEmailItineraryReservation/{flightReservationId}")]
        [ProducesResponseType(typeof(ResponseResult<bool>), 200)]
        public async Task<IActionResult> ConfirmEmailItineraryReservation([FromRoute] long flightReservationId)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.ConfirmEmailItineraryReservation(flightReservationId);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Get Email Itinerary Reservation For Review
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getEmailItineraryReservationForReview")]
        [ProducesResponseType(typeof(ResponseResult<List<FlightReservationVM>>), 200)]
        public async Task<IActionResult> GetEmailItineraryReservationForReview()
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetEmailItineraryReservationForReview();
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Create Reservation Activty Code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CreateReservationActivtyCode")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<ScannedResponseVM>), 200)]
        public async Task<IActionResult> CreateReservationActivtyCode(ScannedVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.CreateReservationActivtyCode(model, headerAccountId, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// Get All upcoming Or ongoing Trip
        /// </summary>
        /// <returns></returns>
        [Route("getallongoingupcomingtrips")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<OngoingUpcomingTripVM>), 200)]
        public async Task<IActionResult> GetAllOngoingUpcomingTrips()
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetAllOngoingUpcomingTrips();
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Get All Trips
        /// </summary>
        /// <param name="flag">ongoing|upcoming|completed</param>
        /// <returns></returns>
        [Route("getalltrips/{flag}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<List<TripDetailVM>>), 200)]
        public async Task<IActionResult> GetAllTrips([FromRoute]string flag)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetAllTrips(flag);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Get current activity code
        /// </summary>
        /// <returns></returns>
        [Route("getcurrentactivitycode/{reservationId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList<CurrentActivityVM>), 200)]
        public async Task<IActionResult> GetCurrentActivityCode(long reservationId)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetCurrentActivityCode(reservationId);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Shuttle ETA
        /// </summary>
        /// <returns></returns>
        [Route("ShuttleETA")]
        [HttpGet]
        public async Task<IActionResult> ShuttleETA()
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.ShuttleETA();
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Generate Reservation Invoice By Id
        /// </summary>
        /// <returns></returns>
        [Route("GenerateReservationInvoiceById/{reservationid}")]
        [HttpPost]
        public async Task<IActionResult> GenerateReservationInvoiceById([FromRoute]long reservationid)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GenerateReservationInvoiceById(reservationid);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// Generate Reservation Id
        /// </summary>
        /// <returns></returns>
        [Route("GenerateReservationById/{reservationid}")]
        [HttpPost]
        public async Task<IActionResult> GenerateReservationById([FromRoute] long reservationid)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GenerateReservationInvoiceById(reservationid);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// GetReservationVehicleDetailsByVinNumber
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="vinNumber"></param>
        /// <returns></returns>
        [Route("GetReservationVehicleDetailsByVinNumber/{reservationId}/{vinNumber}")]
        [HttpGet]
        public async Task<IActionResult> GetReservationVehicleDetailsByVinNumber(long reservationId, string vinNumber)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetReservationVehicleDetailsByVinNumber(reservationId, vinNumber);
                return Ok(result);
            });
        }
        /// <summary>
        /// Update Parking Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateParkingReservation")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResultList<int>), 200)]
        public async Task<IActionResult> UpdateParkingReservation(UpdateIsParkedVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.UpdateParkingReservation(model);
                if (result.ResponseCode == ResponseCode.ParkingStatus)
                    return Ok(result);
                return BadRequest(result);
            });
        }
        /// <summary>
        /// GetJourneyCompletedlist
        /// </summary>
        /// <returns></returns>
        [Route("getjourneycompletedlist")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<OngoingUpcomingTripVM>), 200)]
        public async Task<IActionResult> GetJourneyCompletedlist()
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetJourneyCompletedlist();
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }
        /// <summary>
        /// GetShuttleBoardedList
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        [Route("getshuttleboardedlist")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<ShuttleBoardedListVM>), 200)]
        public async Task<IActionResult> GetShuttleBoardedList(long terminalId)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetShuttleBoardedList(terminalId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        /// <summary>
        /// De-Board From Shuttle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("deboardfromshuttle")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<int>), 200)]
        public async Task<IActionResult> DeBoardFromShuttle(DeBoardFromShuttleVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.DeBoardFromShuttle(model);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }
        [Route("getreservationsbyparkinglocationid")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<ReservationHistoryVM>), 200)]
        public async Task<IActionResult> GetReservationsByParkingLocationId(ReservationSearchVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetReservationsByParkingLocationId(model);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }

        [Route("getreservationsbyuserid")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<ShuttleBoardedListVM>), 200)]
        public async Task<IActionResult> GetReservationsByUserId(long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetReservationsByUserId(userId);
                if (result.ResponseCode == ResponseCode.InternalServerError)
                    return BadRequest(result);
                else
                    return Ok(result);

            });
        }

        /// <summary>
        /// Create Reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createreservationnew")]
        [ProducesResponseType(typeof(ResponseResult<ParkingReservationAmountAndLocationVM>), 200)]
        public async Task<IActionResult> CreateReservationNew([FromBody] AddUpdateFlightAndParkingReservationVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.CreateReservationNew(model, loggedInUserId, headerAccountId);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }


    }
}
