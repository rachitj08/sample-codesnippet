using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.ServiceWorker;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : BaseApiController
    {
        private readonly IReservationService reservationService;

        /// <summary>
        /// ReservationController Constructor
        /// </summary>
        /// <param name="reservationService"></param>
        /// <param name="logger"></param>
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
                var userGroups = this.reservationService.GetAllReservations( ordering,  offset,  pageSize,  pageNumber,  all, loggedInAccountId);
                return Ok(userGroups);
            });

        }
        /// <summary>
        /// GetReservationById
        /// </summary>
        /// <param name="reservationId"></param>
        /// <returns></returns>
        [Route("GetReservationById/{reservationId}")]
        [HttpGet]
        public async Task<IActionResult> GetReservationById(long reservationId)
        {
            return await Execute(async () =>
            {
                var userGroups =  this.reservationService.GetReservationById(reservationId, loggedInAccountId);
                return Ok(userGroups);
            });

        }
        
        ///// <summary>
        ///// CreateReservation
        ///// </summary>
        ///// <param name="objReservationVM"></param>
        ///// <returns></returns>
        //[Route("CreateFlightAndParkingReservation")]
        //[HttpPost]
        //public async Task<IActionResult> CreateFlightAndParkingReservation(AddUpdateFlightAndParkingReservationVM objReservationVM)
        //{
        //    return await Execute(async () =>
        //    {
        //        var result = await this.reservationService.CreateReservation(objReservationVM);
        //        return Ok(result);
        //    });

        //}
        /// <summary>
        /// CreateReservation
        /// </summary>
        /// <param name="objReservationVM"></param>
        /// <returns></returns>
        [Route("CalcuatePriceAndAddressDataByAirportID")]
        [HttpPost]
        public async Task<IActionResult> CalcuatePriceAndAddressDataByAirportID(AddUpdateFlightAndParkingReservationVM objReservationVM)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetReservationPriceAndAddressData(objReservationVM, loggedInAccountId);
                return Ok(result);
            });

        }
        /// <summary>
        /// UpdateReservation
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateFlightAndParkingReservation/{reservationid}")]
        [HttpPut]
        public async Task<IActionResult> UpdateReservation([FromRoute]long reservationId, [FromBody]AddUpdateFlightAndParkingReservationVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.UpdateReservation(model, reservationId, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// DeleteReservation
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("DeleteReservation/{reservationid}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteReservation(long reservationid)
        {
            return await Execute(async () =>
            {
                var userGroups = await this.reservationService.DeleteReservation(reservationid, loggedInAccountId);
                return Ok(userGroups);
            });
        }

        [Route("CancelReservation/{reservationid}")]
        [HttpPut]
        public async Task<IActionResult> CancelReservation(long reservationId)
        {   
            return await Execute(async () =>
            {
                var result = await this.reservationService.CancelReservation(reservationId, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get Email Itinerary Reservation For Review
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getEmailItineraryReservationForReview")]
        public async Task<IActionResult> GetEmailItineraryReservationForReview()
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetEmailItineraryReservationForReview(loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Confirm Email Itinerary Reservation
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("confirmEmailItineraryReservation/{flightReservationId}")]
        public async Task<IActionResult> ConfirmEmailItineraryReservation([FromRoute] long flightReservationId)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.ConfirmEmailItineraryReservation(flightReservationId, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get All upcoming Or ongoing Trip
        /// </summary>
        /// <returns></returns>
        [Route("getallongoingupcomingtrips")]
        [HttpGet]
        public async Task<IActionResult> GetAllOngoingUpcomingTrips()
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetAllOngoingUpcomingTrips(loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get All Trip
        /// </summary>
        /// <returns></returns>
        [Route("getalltrips/{flag}")]
        [HttpGet]
        public async Task<IActionResult> GetAllTrips([FromRoute]string flag)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetAllTrips(flag, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        [Route("GetCurrentActivityCode")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentActivityCode(long reservationId)
        {
            return await Execute(async () =>
            {
                var result = this.reservationService.GetCurrentActivityCode(reservationId,loggedInAccountId);
                return Ok(result);
            });
        }
        /// <summary>
        /// Generate Reservation Invoice By Id
        /// </summary>
        /// <param name="reservation id"></param>
        /// <returns></returns>
        [Route("GenerateReservationInvoiceById/{reservationid}")]
        [HttpPost]
        public async Task<IActionResult> GenerateReservationInvoiceById([FromRoute]long reservationid)
        {
            return await Execute(async () =>
            {
                var model = new CreateInvoiceVM() {ReservationId = reservationid, AccountId = loggedInAccountId, UserId = loggedInUserId };                
                var result = await this.reservationService.GenerateReservationInvoiceById(model);
                return Ok(result);
            });
        }

        /// <summary>
        /// Generate Reservation By Id
        /// </summary>
        /// <param name="reservation id"></param>
        /// <returns></returns>
        [Route("GenerateReservationById/{reservationid}")]
        [HttpPost]
        public async Task<IActionResult> GenerateReservationById([FromRoute] long reservationid)
        {
            return await Execute(async () =>
            {
                var model = new CreateInvoiceVM() { ReservationId = reservationid, AccountId = loggedInAccountId, UserId = loggedInUserId };
                var result = await this.reservationService.GenerateReservationById(model);
                return Ok(result);
            });
        }

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        [Route("ShuttleETA")]
        [HttpGet]
        public async Task<IActionResult> ShuttleETA()
        {
            return await Execute(async () =>
            {
                var result = this.reservationService.ShuttleETA();
                return Ok(result);
            });
        }
        

        [Route("updatecreatereservationactivtycode")]
        [HttpPost]
        public async Task<IActionResult> UpdateCreateReservationActivtyCode(ScannedVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.UpdateCreateReservationActivtyCode(model, loggedInAccountId, loggedInUserId);
                return Ok(result);
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
                var result = this.reservationService.GetReservationVehicleDetailsByVinNumber(reservationId, vinNumber,loggedInAccountId);
                return Ok(result);
            });
        }
        [Route("updateparkingreservation")]
        [HttpPost]
        public async Task<IActionResult> UpdateParkingReservation(UpdateIsParkedVM model)
        {   
            return await Execute(async () =>
            {
                var result = await this.reservationService.UpdateParkingReservation(model, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }
        [Route("getjourneycompletedlist")]
        [HttpGet]
        public async Task<IActionResult> GetJourneyCompletedlist()
        {   
            return await Execute(async () =>
            {
                var result = await this.reservationService.GetJourneyCompletedlist(loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }
        [Route("getanalyticdetail")]
        [HttpGet]
        public async Task<IActionResult> GetAnalyticDetail(long parkingProviderLocationId)
        {   
            return await Execute(async () =>
            {
                var result = this.reservationService.GetAnalyticDetail(loggedInAccountId, parkingProviderLocationId);
                return Ok(result);
            });
        }
        [Route("getshuttleboardedlist/{terminalId}")]
        [HttpGet]
        public async Task<IActionResult> GetShuttleBoardedList([FromRoute] int terminalId)
        {
            return await Execute(async () =>
            {
                var result =  this.reservationService.GetShuttleBoardedList(terminalId);
                return Ok(result);
            });
        }
        [Route("deboardfromshuttle")]
        [HttpPost]
        public async Task<IActionResult> DeBoardFromShuttle(DeBoardFromShuttleVM model)
        {
            return await Execute(async () =>
            {
                var result =  this.reservationService.DeBoardFromShuttle(model, loggedInAccountId, loggedInUserId);
                return Ok(result);
            });
        }

        [Route("getreservationsbyparkinglocationid")]
        [HttpPost]
        public async Task<IActionResult> GetReservationsByParkingLocationId( ReservationSearchVM model)
        {
            return await Execute(async () =>
            {
                var result =await this.reservationService.GetReservationsByParkingLocationId(model,loggedInAccountId,loggedInUserId);
                return Ok(result);
            });
        }
        [Route("getreservationsbyuserid/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetReservationsByUserId( long userId)
        {
            return await Execute(async () =>
            {
                var result =await this.reservationService.GetReservationsByUserId(loggedInAccountId, userId);
                return Ok(result);
            });
        }

        [Route("createreservationnew")]
        [HttpPost]
        public async Task<IActionResult> CreateReservationNew([FromBody] AddUpdateFlightReservationVM model)
        {
            return await Execute(async () =>
            {
                var result = await this.reservationService.CreateReservationNew(model,loggedInAccountId);
                return Ok(result);
            });

        }

    }
}
