using AutoMapper;
using Common.Enum.StorageEnum;
using Common.Model;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.EmailHelper;
using Utility;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Model.Model.StorageModel;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAirportsParkingService _airportsParkingService;
        private readonly IAirportsRepository _airportsRepo;
        private readonly IAddressRepository _addressRepo;
        private readonly IParkingHeadsService _parkingHeadsService;
        private readonly IParkingReservationRepository _parkingReservationRepo;
        private readonly IReservationActivityCodeRepository _reservationActivityCodeRepository;
        private readonly IFlightReservationRepository _flightReservationRepository;
        private readonly IActivityCodeRepository _activityCodeRepository;
        private readonly IParkingProvidersSubLocationsRepository _parkingProvidersSubLocationsRepository;
        private readonly IMapper mapper;
        private readonly SMSConfig _SMSConfig;
        private readonly IParkingSpotsRepository _parkingSpotsRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmailHelper _emailHelper;
        private readonly ISMSHelper _smsHelper;
        private readonly ICommonHelper _commonHelper;
        private readonly CommonConfig _commonConfig;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVehicleTagMappingRepository _vehicleTagMappingRepository;
        public ReservationService(IUnitOfWork unitOfWork, IReservationRepository reservationRepository,
            IAirportsParkingService airportsParkingService, IAirportsRepository airportsRepo,
            IParkingHeadsService parkingHeadsService, IFlightReservationRepository flightReservationRepository,
            IReservationActivityCodeRepository reservationActivityCodeRepository, IMapper mapper,
            IParkingReservationRepository parkingReservationRepo, IAddressRepository addressRepo,
            IActivityCodeRepository activityCodeRepository,
            IOptions<SMSConfig> smsConfig,
            IParkingProvidersSubLocationsRepository parkingProvidersSubLocationsRepository,
            IParkingSpotsRepository parkingSpotsRepository, IInvoiceRepository invoiceRepository,
            IEmailHelper emailHelper, ISMSHelper smsHelper, ICommonHelper commonHelper, IOptions<CommonConfig> commonConfig,
            IVehiclesRepository vehiclesRepository, IUserRepository userRepository,IVehicleTagMappingRepository vehicleTagMappingRepository)
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(reservationRepository), reservationRepository);
            Check.Argument.IsNotNull(nameof(parkingHeadsService), parkingHeadsService);
            Check.Argument.IsNotNull(nameof(airportsParkingService), airportsParkingService);
            Check.Argument.IsNotNull(nameof(reservationActivityCodeRepository), reservationActivityCodeRepository);
            Check.Argument.IsNotNull(nameof(flightReservationRepository), flightReservationRepository);
            Check.Argument.IsNotNull(nameof(airportsRepo), airportsRepo);
            Check.Argument.IsNotNull(nameof(addressRepo), addressRepo);
            Check.Argument.IsNotNull(nameof(parkingReservationRepo), parkingReservationRepo);
            Check.Argument.IsNotNull(nameof(activityCodeRepository), activityCodeRepository);
            Check.Argument.IsNotNull(nameof(smsConfig), smsConfig);
            Check.Argument.IsNotNull(nameof(commonConfig), commonConfig);
            Check.Argument.IsNotNull(nameof(parkingProvidersSubLocationsRepository), parkingProvidersSubLocationsRepository);
            Check.Argument.IsNotNull(nameof(parkingSpotsRepository), parkingSpotsRepository);
            Check.Argument.IsNotNull(nameof(emailHelper), emailHelper);
            Check.Argument.IsNotNull(nameof(smsHelper), smsHelper);
            Check.Argument.IsNotNull(nameof(invoiceRepository), invoiceRepository);
            Check.Argument.IsNotNull(nameof(commonHelper), commonHelper);
            Check.Argument.IsNotNull(nameof(vehiclesRepository), vehiclesRepository);
            Check.Argument.IsNotNull(nameof(userRepository), userRepository);
            Check.Argument.IsNotNull(nameof(vehicleTagMappingRepository), vehicleTagMappingRepository);

            this.unitOfWork = unitOfWork;
            this._reservationRepository = reservationRepository;
            this._parkingHeadsService = parkingHeadsService;
            this._airportsParkingService = airportsParkingService;
            this._reservationActivityCodeRepository = reservationActivityCodeRepository;
            this._flightReservationRepository = flightReservationRepository;
            this.mapper = mapper;
            this._airportsRepo = airportsRepo;
            this._parkingReservationRepo = parkingReservationRepo;
            this._addressRepo = addressRepo;
            _activityCodeRepository = activityCodeRepository;
            _SMSConfig = smsConfig.Value;
            _commonConfig = commonConfig.Value;
            _parkingProvidersSubLocationsRepository = parkingProvidersSubLocationsRepository;
            _parkingSpotsRepository = parkingSpotsRepository;
            _invoiceRepository = invoiceRepository;
            _emailHelper = emailHelper;
            _smsHelper = smsHelper;
            _commonHelper = commonHelper;
            _vehiclesRepository = vehiclesRepository;
            _userRepository = userRepository;
            _vehicleTagMappingRepository = vehicleTagMappingRepository;
        }

        public async Task<ResponseResult<bool>> CreateReservation(AddUpdateFlightReservationVM model)
        {
            try
            {
                // Validations
                var validateModelResult = IsInputFormDataIsValid(model);
                if (validateModelResult.ResponseCode == ResponseCode.ValidationFailed)
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.ValidationFailed,
                        Message = validateModelResult.Message,
                        Error = validateModelResult.Error
                    };
                }

                AddFlightReservationVM finalModel = mapper.Map<AddFlightReservationVM>(model);
                finalModel.ReservationCode = model.DepaurtureAirportId + DateTime.Now.Ticks.ToString();
                var result = await _reservationRepository.CreateReservationFromSP(finalModel);
                if (result.ReservationId > 0)
                {
                    

                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.RecordSaved,
                        Message = ResponseMessage.RecordSaved,
                        Data = true
                    };
                }
                else
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.SomethingWentWrong,
                        Message = result.ErrorMessage ?? ResponseMessage.SomethingWentWrong,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.SomethingWentWrong
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Reservation Service ==> Kafka Call (CreatePaymentIntent) ==> Kafka Config  Exception :: "+ex);
                throw;
            }
        }

        /// <summary>
        /// Get Reservation Price And Address Data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ParkingReservationAmountAndLocationVM>> GetReservationPriceAndAddressData(AddUpdateFlightAndParkingReservationVM model, long accountId, long airportsParkingId = 0)
        {
            if (model == null || model.DepaurtureAirportId < 1
                || model.ReturnDateTime == null || model.ReturnDateTime == DateTime.MinValue
                || model.DepaurtureDateTime == null || model.DepaurtureDateTime == DateTime.MinValue)
            {
                return new ResponseResult<ParkingReservationAmountAndLocationVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var airportsAndParkingData = await _airportsParkingService.SelectAirportsParkingByAirportId(model.DepaurtureAirportId, accountId, airportsParkingId);
           
            if (airportsAndParkingData == null || airportsAndParkingData.Airport == null)
            {
                return new ResponseResult<ParkingReservationAmountAndLocationVM>
                {
                    Message = "Depaurture airport details are missing.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            if (airportsAndParkingData.ParkingProvidersLocationId < 1 || airportsAndParkingData.ParkingProvidersLocations == null || airportsAndParkingData.ParkingProvidersLocationAddress == null)
            {
                return new ResponseResult<ParkingReservationAmountAndLocationVM>
                {
                    Message = "We don't have any parking providers for Depaurture airport.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            #region[Get Price Details]
            var inTimeGap = airportsAndParkingData.Airport.InTimeGapInMin + airportsAndParkingData.ParkingProvidersLocations.InTimeGapInMin;
            var outTimeGap = airportsAndParkingData.Airport.OutTimeGapInMin + airportsAndParkingData.ParkingProvidersLocations.OutTimeGapInMin;
            var startDate = inTimeGap > 0 ? model.DepaurtureDateTime.AddMinutes(-inTimeGap) : model.DepaurtureDateTime;
            var endDate = outTimeGap > 0 ? model.ReturnDateTime.AddMinutes(outTimeGap) : model.ReturnDateTime;
            
            var parkingPriceDetail = _parkingHeadsService.GetParkingPriceDetail(airportsAndParkingData.ParkingProvidersLocationId, startDate, endDate, accountId,false,0);
            if (parkingPriceDetail.ResponseCode != ResponseCode.RecordFetched)
            {
                return new ResponseResult<ParkingReservationAmountAndLocationVM>()
                {
                    ResponseCode = parkingPriceDetail.ResponseCode,
                    Message = parkingPriceDetail.Message,
                    Error = parkingPriceDetail.Error
                };
            }

            decimal totalFinalAmount = parkingPriceDetail.Data.Sum(x => x.Amount - (x.DiscountAmount ?? 0));
            #endregion


            // Address prepared
            StringBuilder parkingAddress = new StringBuilder();
            if (!string.IsNullOrEmpty(airportsAndParkingData.ParkingProvidersLocations.Name))
                parkingAddress.AppendLine(airportsAndParkingData.ParkingProvidersLocations.Name);
            if (!string.IsNullOrEmpty(airportsAndParkingData.ParkingProvidersLocationAddress.Streat1))
                parkingAddress.Append(airportsAndParkingData.ParkingProvidersLocationAddress.Streat1 + ", ");
            if (!string.IsNullOrEmpty(airportsAndParkingData.ParkingProvidersLocationAddress.Streat2))
                parkingAddress.Append(airportsAndParkingData.ParkingProvidersLocationAddress.Streat2 + ", ");
            if (!string.IsNullOrEmpty(airportsAndParkingData.ParkingProvidersLocationAddress.City))
                parkingAddress.Append(airportsAndParkingData.ParkingProvidersLocationAddress.City + ", ");
            if (!string.IsNullOrEmpty(airportsAndParkingData.ParkingProvidersLocationAddress.State))
                parkingAddress.Append(airportsAndParkingData.ParkingProvidersLocationAddress.State + ", ");
            if (!string.IsNullOrEmpty(airportsAndParkingData.ParkingProvidersLocationAddress.Country))
                parkingAddress.Append(airportsAndParkingData.ParkingProvidersLocationAddress.Country + ", ");
            if (!string.IsNullOrEmpty(airportsAndParkingData.ParkingProvidersLocationAddress.Zip))
                parkingAddress.Append(airportsAndParkingData.ParkingProvidersLocationAddress.Zip);

            return new ResponseResult<ParkingReservationAmountAndLocationVM>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Data = new ParkingReservationAmountAndLocationVM()
                {
                    ParkingTotalAmount = totalFinalAmount,
                    ParkingAddress = parkingAddress.ToString(),
                    InTimeGap = inTimeGap,
                    OutTimeGap = outTimeGap,
                    ParkingProvidersLocationId = airportsAndParkingData.ParkingProvidersLocationId,
                    AirportsParkingId = airportsAndParkingData.AirportsParkingId,
                    UserId=model.CustomUserId
                }
            };
        }
        
        public async Task<ResponseResult<ParkingReservationAmountAndLocationVM>> UpdateReservation(AddUpdateFlightAndParkingReservationVM model, long reservationId, long accountId, long userId)
        {
            // Validations
            var validateModelResult = IsInputFormDataIsValid(model);
            if (validateModelResult.ResponseCode == ResponseCode.ValidationFailed) return validateModelResult;

            var reservation = _reservationRepository.GetReservationById(reservationId, accountId);
            if (reservation == null)
            {
                return new ResponseResult<ParkingReservationAmountAndLocationVM>()
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }

            var flightReservationCollection = reservation.FlightReservation;
            var flightReservation = flightReservationCollection.FirstOrDefault();

            var parkingReservationCollection = reservation.ParkingReservation;
            var parkingReservation = parkingReservationCollection.FirstOrDefault();

            #region [Vatildate Data, Get Price And Address]
            long airportsParkingId = 0;
            if (flightReservation.DepaurtureAirportId == model.DepaurtureAirportId)
            {
                airportsParkingId = parkingReservation.AirportsParkingId;
            }
            var priceResult = await GetReservationPriceAndAddressData(model, accountId, airportsParkingId);
            
            if (priceResult.ResponseCode != ResponseCode.RecordFetched) return priceResult;
            #endregion


            // Update Reservation Table
            reservation.IsChanged = true;
            reservation.UpdatedBy = userId;
            reservation.UpdatedOn = DateTime.UtcNow;

            // _reservationRepository.UpdateReservation(reservation);
            // Update Flight Reservation
            flightReservation.DepaurtureAirportId = model.DepaurtureAirportId;
            flightReservation.IsHomeAirport = model.IsHomeAirport;
            flightReservation.DepaurtureDateTime = model.DepaurtureDateTime;
            flightReservation.ReturnDateTime = model.ReturnDateTime;
            flightReservation.FlyingToAirportld = model.FlyingToAirportld;
            flightReservation.IsBorrowingCarForRent = model.IsBorrowingCarForRent;
            flightReservation.FlyingToAirline = model.FlyingToAirline;
            flightReservation.FlyingToFlightNo = model.FlyingToFlightNo;
            flightReservation.ReturningToAirportld = model.ReturningToAirportld;
            flightReservation.ReturningToAirline = model.ReturningToAirline;
            flightReservation.ReturningToFlightNo = model.ReturningToFlightNo;
            flightReservation.UpdatedBy = userId;
            flightReservation.UpdatedOn = DateTime.UtcNow;
            flightReservation.Status = 1;
            reservation.FlightReservation = flightReservationCollection;

            // Update Parking Reservation
            var startDate = priceResult.Data.InTimeGap > 0 ? model.DepaurtureDateTime.AddMinutes(-priceResult.Data.InTimeGap) : model.DepaurtureDateTime;
            var endDate = priceResult.Data.OutTimeGap > 0 ? model.ReturnDateTime.AddMinutes(priceResult.Data.OutTimeGap) : model.ReturnDateTime;

            parkingReservation.ParkingProvidersLocationId = priceResult.Data.ParkingProvidersLocationId;
            parkingReservation.AirportsParkingId = priceResult.Data.AirportsParkingId;
            parkingReservation.StartDateTime = startDate;
            parkingReservation.EndDateTime = endDate;
            parkingReservation.IsConcentedToRent = model.IsBorrowingCarForRent; 
            parkingReservation.UpdatedOn = DateTime.UtcNow;
            parkingReservation.UpdatedBy = userId;
            parkingReservation.Comment = model.Comment;
            parkingReservation.SourceId=model.SourceId;
            parkingReservation.BookingConfirmationNo=model.BookingConfirmationNo;
            reservation.ParkingReservation = parkingReservationCollection;

            _reservationRepository.UpdateReservation(reservation);
            if (unitOfWork.CommitWithStatus() > 0)
            {
                

                priceResult.Data.ReservationId = reservationId;
                priceResult.ResponseCode = ResponseCode.RecordSaved;
                priceResult.Message = ResponseMessage.RecordSaved;
                return priceResult; 
            }
            else
            {
                return new ResponseResult<ParkingReservationAmountAndLocationVM>()
                {
                    Message = "Could not able to save details.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
        }

        public async Task<ResponseResult<FlightAndParkingReservationVM>> DeleteReservation(long reservationid, long accountId)
        {
            var responseResult = new ResponseResult<FlightAndParkingReservationVM>();
            var objReservation = this._reservationRepository.GetReservationById(reservationid, accountId);
            if (objReservation != null)
            {
                int result = await this._reservationRepository.DeleteReservation(objReservation);
                if (result > 0)
                {

                    responseResult.ResponseCode = ResponseCode.RecordDeleted;
                    responseResult.Message = ResponseMessage.RecordDeleted;
                }
                else
                {
                    responseResult.ResponseCode = ResponseCode.SomethingWentWrong;
                    responseResult.Message = ResponseMessage.SomethingWentWrong;
                    responseResult.Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.SomethingWentWrong
                    };
                }
            }
            else
            {
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
            }

            return responseResult;
        }

        public ResponseResultList<FlightAndParkingReservationVM> GetAllReservations(string ordering, int offset, int pageSize, int pageNumber, bool all, long accountId)
        {
            ResponseResultList<FlightAndParkingReservationVM> objResponseResultResevation = new ResponseResultList<FlightAndParkingReservationVM>();
            List<FlightAndParkingReservationVM> lstReservationVM = new List<FlightAndParkingReservationVM>();
            List<Reservation> result = this._reservationRepository.GetAllReservations();
            if (result != null && result.Count() > 0)
            {

                foreach (var items in result)
                {
                    FlightAndParkingReservationVM ojReservationVM = new FlightAndParkingReservationVM();
                    ojReservationVM.ReservationId = items.ReservationId;
                    ojReservationVM.ReservationCode = items.ReservationCode;
                    ojReservationVM.UserId = items.UserId;
                    lstReservationVM.Add(ojReservationVM);
                }
                objResponseResultResevation = new ResponseResultList<FlightAndParkingReservationVM>
                {
                    ResponseCode = ResponseCode.RecordFetched,
                    Message = ResponseMessage.RecordFetched,
                    //Count = listCount,
                    //Next = sbNext.ToString(),
                    //Previous = sbPrevious.ToString(),
                    Data = lstReservationVM,
                };
            }
            else
            {
                objResponseResultResevation = new ResponseResultList<FlightAndParkingReservationVM>
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    //Count = listCount,
                    //Next = sbNext.ToString(),
                    //Previous = sbPrevious.ToString(),
                    Data = lstReservationVM,
                };
            }
            return objResponseResultResevation;
        }

        public ResponseResult<FlightAndParkingReservationVM> GetReservationById(long reservationid, long accountId)
        {
            var responseResult = new ResponseResult<FlightAndParkingReservationVM>();
            var reservation = _reservationRepository.GetReservationById(reservationid, accountId);
            if (reservation != null)
            {
                var data = new FlightAndParkingReservationVM()
                {
                    ReservationId = reservation.ReservationId,
                    ReservationCode = reservation.ReservationCode,
                    UserId = reservation.UserId
                };

                data.FlightReservation = mapper.Map<FlightReservationVM>(reservation.FlightReservation.FirstOrDefault());
                data.ParkingReservation = mapper.Map<ParkingReservationVM>(reservation.ParkingReservation.FirstOrDefault());
                data.ReservationVehicle = mapper.Map<ReservationVehicleVM>(reservation.ReservationVehicle.FirstOrDefault());

                responseResult.ResponseCode = ResponseCode.RecordFetched;
                responseResult.Message = ResponseMessage.RecordFetched;
                responseResult.Data = data;
            }
            else
            {
                responseResult.ResponseCode = ResponseCode.NoRecordFound;
                responseResult.Message = ResponseMessage.NoRecordFound;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
            }
            return responseResult;
        }

        /// <summary>
        /// Get Email Itinerary Reservation For Review
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<List<FlightReservationVM>>> GetEmailItineraryReservationForReview(long accountId, long userId)
        {
            var flightReservationList = await _flightReservationRepository.GetAllFlightReservationForReview(accountId, userId);
            if (flightReservationList != null)
            {
                var flightReservationData = mapper.Map<List<FlightReservationVM>>(flightReservationList);

                return new ResponseResult<List<FlightReservationVM>>()
                {
                    ResponseCode = ResponseCode.RecordFetched,
                    Message = ResponseMessage.RecordFetched,
                    Data = flightReservationData
                };
            }
            else
            {
                return new ResponseResult<List<FlightReservationVM>>()
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }
        }

        /// <summary>
        /// Confirm Email Itinerary Reservation
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> ConfirmEmailItineraryReservation(long flightReservationId, long accountId, long userId)
        {
            var flightReservation = await _flightReservationRepository.GetFlightReservationById(flightReservationId, accountId);
            if (flightReservation != null)
            {
                flightReservation.Status = 1;
                flightReservation.UpdatedOn = DateTime.UtcNow;
                flightReservation.UpdatedBy = userId;

                _flightReservationRepository.UpdateFlightReservation(flightReservation);

                if (unitOfWork.CommitWithStatus() > 0)
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.RecordSaved,
                        Message = ResponseMessage.RecordSaved,
                        Data = true
                    };
                }
                else
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.InternalServerError,
                        Message = "Could not able to save details",
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
            }
            else
            {
                return new ResponseResult<bool>()
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }
        }

        /// <summary>
        /// Cancel Reservation
        /// </summary>
        /// <param name="reservationid"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> CancelReservation(long reservationid, long accountId, long userId)
        {
            var reservation = this._reservationRepository.GetReservationById(reservationid, accountId);
            reservation.ReservationId = reservationid;
            reservation.UpdatedBy = userId;
            reservation.UpdatedOn = DateTime.UtcNow;
            if (reservation.IsCancelled == true)
                reservation.IsCancelled = false;
            else
                reservation.IsCancelled = true;

            try
            {
                Reservation result = this._reservationRepository.UpdateReservation(reservation);
                if (unitOfWork.CommitWithStatus() < 1)
                {
                    return new ResponseResult<bool>()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                else
                {
                    

                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.RecordSaved,
                        Message = ResponseMessage.RecordSaved,
                        Data = true
                    };
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Get All Ongoing Upcoming Trips
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<OngoingUpcomingTripVM>> GetAllOngoingUpcomingTrips(long accountId, long userId)
        {
            var data = new OngoingUpcomingTripVM();

            var flightReservationQuery = this._flightReservationRepository.GetAllFlightReservation(accountId, userId);
            var airportQuery = this._airportsRepo.GetAllAiportsWithAddress(accountId);
            var parkingLocationQuery = this._parkingReservationRepo.GetParkingReservationQuery(accountId);
            var addressQuery = this._addressRepo.GetAddressQuery(accountId);
            var activeCodeQuery = this._reservationActivityCodeRepository.GetReservationActivityCodeQuery(accountId);

            var resultQuery = from f in flightReservationQuery
                          join da in airportQuery on f.DepaurtureAirportId equals da.AirportId
                          join ra in airportQuery on f.ReturningToAirportld equals ra.AirportId
                          join pr in parkingLocationQuery on f.ReservationId equals pr.ReservationId
                          join ad in addressQuery on pr.ParkingProvidersLocation.AddressId equals ad.AddressId
                          select new TripDetailVM()
                          {
                              ReservationId = f.ReservationId,
                              FlightReservationId = f.FlightReservationId,
                              DepaurtureDateTime = f.DepaurtureDateTime,
                              FlightNo = f.FlyingToFlightNo,
                              AirlineCode = f.FlyingToAirline,
                              ArrivalAirportCode = ra.Code,
                              ArrivalAirportName = ra.Name,
                              DepaurtureAirportAddress = (da.Address.Streat1 ?? "" + " " + da.Address.Streat2 ?? "" + " " + da.Address.City ?? "" + " " + da.Address.State ?? ""),
                              DepaurtureAirportCode = da.Code,
                              DepaurtureAirportName = da.Name,
                              DepaurtureCity = da.Address.City ?? "",
                              ParkingLocationAddress = (ad.Streat1 ?? "" + " " + ad.Streat2 ?? "" + " " + ad.City ?? "" + " " + ad.State ?? ""),
                              ActivityCode = activeCodeQuery.Where(x=> x.ReservationId == f.ReservationId).Select(x=> x.ActivityCode).OrderByDescending(x => x.Odering).FirstOrDefault().Code,
                              Latitude = ad.Latitude,
                              Longitude = ad.Longitude,
                              ParkingProvidersLocationId = pr.ParkingProvidersLocationId ?? 0
                          };
            
            data.OngoingTrips = await resultQuery.Where(x => !string.IsNullOrWhiteSpace(x.ActivityCode) && x.ActivityCode != "VLP").OrderBy(x => x.DepaurtureDateTime).ToListAsync();
            data.UpcomingTrips = await resultQuery.Where(x => string.IsNullOrWhiteSpace(x.ActivityCode) && x.DepaurtureDateTime > DateTime.UtcNow).OrderBy(x => x.DepaurtureDateTime).ToListAsync();

            return new ResponseResult<OngoingUpcomingTripVM>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        /// <summary>
        /// Get All Trips
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<List<TripDetailVM>>> GetAllTrips(string flag, long accountId, long userId)
        { 
            if(string.IsNullOrWhiteSpace(flag) || accountId < 1 || userId < 1)
            {
                return new ResponseResult<List<TripDetailVM>>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var data = await this._flightReservationRepository.GetAllTrips(flag, accountId, userId); 
            return new ResponseResult<List<TripDetailVM>> ()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<IEnumerable<CurrentActivityVM>> GetCurrentActivityCode(long reservationId,long accountId)
        {
            var resActvt = _reservationActivityCodeRepository.GetCurrentActivityCode(reservationId, accountId);
            var activityCode = _activityCodeRepository.GetAllActivity(accountId);
            var result = resActvt.Join(activityCode, res => res.ActivityCodeId, act => act.ActivityCodeId, (res, act) => new CurrentActivityVM()
            {
                ActivityCodeId = res.ActivityCodeId,
                ReservationId = res.ReservationId,
                ParkingProvidersLocationSubLocationId = res.ParkingProvidersLocationSubLocationId,
                ActivityCode = act.Code
            }).ToList();

            if (result == null || result.Count == 0)
            {
                return new ResponseResult<IEnumerable<CurrentActivityVM>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            }

            return new ResponseResult<IEnumerable<CurrentActivityVM>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = result
            };
        }

        public async Task<ResponseResult<bool>> GenerateReservationInvoiceById(CreateInvoiceVM model)
        {
            try
            {
                //Console.WriteLine("Called::GenerateReservationInvoiceById- " + model.ReservationId);
                #region[Validation]
                if (model == null || model.ReservationId < 1 || model.AccountId < 1)
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.ValidationFailed,
                        Message = ResponseMessage.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.ValidationFailed
                        }
                    };
                }

                #endregion

                #region[Get Reservation Detail]
                //Console.WriteLine("Called::_reservationRepository.GetReservationDetailById-Get Reservation Detail");
                var reservation = await this._reservationRepository.GetReservationDetailById(model.ReservationId, model.AccountId);

                if (reservation == null)
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.NoRecordFound,
                        Message = ResponseMessage.NoRecordFound,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.NoRecordFound
                        }
                    };
                }
                #endregion

                #region[Get Price Details]
                //Console.WriteLine("Called::_reservationRepository.GetParkingPriceDetail-Get Price Details");
                DateTime startDateTime = reservation.StartDateTime.ToUniversalTime();
                DateTime endDateTime=reservation.EndDateTime.ToUniversalTime();
                ResponseResult<List<InvoicePriceDetailVM>> parkingPriceDetail;
                if (model.IsCheckOut)
                {
                    var parkingReservation = await _parkingReservationRepo.GetParkingReservationByReservationId(model.ReservationId, model.AccountId);
                    startDateTime = (parkingReservation.CheckInDateTime == null || parkingReservation.CheckInDateTime==DateTime.MinValue) ? reservation.StartDateTime.ToUniversalTime() : Convert.ToDateTime(parkingReservation.CheckInDateTime).ToUniversalTime();
                    endDateTime = (parkingReservation.CheckOutDateTime == null || parkingReservation.CheckInDateTime == DateTime.MinValue) ? reservation.EndDateTime.ToUniversalTime() : Convert.ToDateTime(parkingReservation.CheckOutDateTime).ToUniversalTime();
                }
                if (model.IsCustomRate)
                {
                    parkingPriceDetail = _parkingHeadsService.GetParkingPriceDetail(reservation.ParkingProvidersLocationId, startDateTime, endDateTime, model.AccountId,model.IsCustomRate,model.ReservationId);
                }
                else
                {
                    parkingPriceDetail = _parkingHeadsService.GetParkingPriceDetail(reservation.ParkingProvidersLocationId, startDateTime, endDateTime, model.AccountId,model.IsCustomRate,0);
                }
                if (parkingPriceDetail.ResponseCode != ResponseCode.RecordFetched)
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = parkingPriceDetail.ResponseCode,
                        Message = parkingPriceDetail.Message,
                        Error = parkingPriceDetail.Error
                    };
                }

                decimal totalFinalAmount = parkingPriceDetail.Data.Sum(x => x.Amount - (x.DiscountAmount ?? 0));
                #endregion

                #region [Save Invoice Detail]
                //Console.WriteLine("Save Invoice Detail");
                string bodyHTML = "", pdfHtml = "";
                var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
                var pdfHtmlFilePath = Path.Combine("Helpers", "Docs", "EmailTemplate", "InvoiceDetailPdf.html");
                //Console.WriteLine("pdfHtmlFilePath::pdfHtmlFilePath- "+ pdfHtmlFilePath);
                var fileInfo = provider.GetFileInfo(pdfHtmlFilePath);
                if (!fileInfo.Exists)
                {
                    return new ResponseResult<bool>()
                    {
                        Message = "Invoice Detail Pdf HTML file is missing.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                using (var fs = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(fs))
                    {
                        pdfHtml = sr.ReadToEnd();
                    }
                }

                if (string.IsNullOrWhiteSpace(pdfHtml))
                {
                    return new ResponseResult<bool>()
                    {
                        Message = "Invoice Detail Pdf HTML file is blank.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                string invNo = GetInvoiceName(reservation.DepaurtureAirportCode, reservation.ParkingProvidersLocationName, model.ReservationId);
                var pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "Docs", "Temp", $"InvoiceDetail_{Guid.NewGuid()}.pdf");
                pdfHtml = pdfHtml.Replace("##UserName##", reservation.UserFirstName).Replace("##InvoiceNo##", invNo)
                   .Replace("##StartDate##", reservation.StartDateTime.ToUniversalTime().ToString("dd-MMM-yyyy HH:mm"))
                   .Replace("##EndDate##", reservation.EndDateTime.ToUniversalTime().ToString("dd-MMM-yyyy HH:mm"));
                pdfHtml = pdfHtml.Replace("##InvoiceDetails##", CreateEmailSendContentTable(parkingPriceDetail.Data.OrderBy(x => x.SeqNo).ToList(), totalFinalAmount));
                //Console.WriteLine("Read and Update PDF invNo- " + invNo);
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "Docs", "Temp")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "Docs", "Temp"));
                }
                //Console.WriteLine("GetPDFBytes UserId- " + model.UserId+" "+ reservation.Mobile);
                var pdfBytes = PDFHelper.GetPDFBytes(pdfHtml, pdfFilePath);
                
                string path = SavePDF(pdfBytes, model.UserId, reservation.Mobile, "Invoice", (int)MediaReferenceEnum.SavePDF);
                if (string.IsNullOrEmpty(path))
                {
                    return new ResponseResult<bool>()
                    {
                        Message = "Failed to Save Invoice in to the Server.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                var invoiceDetails = parkingPriceDetail.Data.ToList();

                var invoice = new InvoiceVM()
                {
                    InvoiceNo = invNo,
                    InvoiceDate = DateTime.UtcNow,
                    ParkingReservationId = reservation.ParkingReservationId,
                    TotalAmount = totalFinalAmount,
                    InvoiceType = 1,
                    InvoiceDetails = invoiceDetails,
                    InvoicePath=path
                };

                //Console.WriteLine("CreateInvoice");

                if (await _invoiceRepository.CreateInvoice(invoice, model.AccountId, model.UserId) < 1)
                {
                    return new ResponseResult<bool>()
                    {
                        Message = "Could not able to save details.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                #endregion
                //Console.WriteLine("Kafka Call to Capture Final Payment");
                
                //Console.WriteLine("Send SMS");

                #region [Send SMS] 
                string smsContent = "";
                provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
                var smsfilePath = Path.Combine("Helpers", "Docs", "SMSTemplate", "InvoiceDetails.txt");
                fileInfo = provider.GetFileInfo(smsfilePath);
                if (!fileInfo.Exists)
                {
                    return new ResponseResult<bool>()
                    {
                        Message = "Invoice sms template file is missing.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }

                using (var fs = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(fs))
                    {
                        smsContent = sr.ReadToEnd();
                    }
                }

                if (string.IsNullOrWhiteSpace(smsContent))
                {
                    return new ResponseResult<bool>()
                    {
                        Message = "Invoice sms template file is blank.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                bool isSmsSended = false;
                try
                {
                    var mobileNo = (!string.IsNullOrWhiteSpace(reservation.MobileCode) ? reservation.MobileCode : "+1") + reservation.Mobile;
                    smsContent = smsContent.Replace("##UserName##", reservation.UserFirstName).Replace("##Amount##", totalFinalAmount.ToString("00.00")).Replace("##ReservationCode##", reservation.ReservationCode);
                    isSmsSended = _smsHelper.SendSMS(_SMSConfig, mobileNo, smsContent);
                }
                catch 
                {
                    //Console.WriteLine("Send Message Failed.");

                }
                #endregion

                #region [Send Email]
                // Generate PDF from Template File






                //Console.WriteLine("Send Mail Start.");


                // Get Email Template
                var filePath = Path.Combine("Helpers", "Docs", "EmailTemplate", "InvoiceDetail.html");
                fileInfo = provider.GetFileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    return new ResponseResult<bool>()
                    {
                        Message = "Invoice Detail email template file is missing.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }

                using (var fs = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(fs))
                    {
                        bodyHTML = sr.ReadToEnd();
                    }
                }

                if (string.IsNullOrWhiteSpace(bodyHTML))
                {
                    return new ResponseResult<bool>()
                    {
                        Message = "Invoice Detail email template file is blank.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }

                bodyHTML = bodyHTML.Replace("##UserName##", reservation.UserFirstName)
                    .Replace("##ReservationCode##", reservation.ReservationCode)
                    .Replace("##FinalAmount##", totalFinalAmount.ToString("00.00"));

                bool isEmailSend = _emailHelper.SendMail(reservation.EmailAddress, "Sample Car:Invoice", bodyHTML, "", new List<string>() { pdfFilePath });
                //Console.WriteLine("SendMail End" + reservation.UserFirstName);
                // Delete Temp PDF file
                try
                {
                    if (File.Exists(pdfFilePath))
                        File.Delete(pdfFilePath);
                }
                catch { }
                #endregion

                #region[Response]
                if (isEmailSend && isSmsSended)
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.RecordSaved,
                        Message = ResponseMessage.RecordSaved,
                        Data = true
                    };
                }
                else
                {
                    return new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.InternalServerError,
                        Message = $"{(!isEmailSend ? "Email" + (!isSmsSended ? " and " : "") : "")}{(!isSmsSended ? "SMS" : "")} could not able to send.",
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                #endregion
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseResult<string>> GenerateReservationById(CreateInvoiceVM model)
        {
            try
            {
                //Console.WriteLine("Called::GenerateReservationInvoiceById- " + model.ReservationId);
                #region[Validation]
                if (model == null || model.ReservationId < 1 || model.AccountId < 1 || model.UserId < 1)
                {
                    return new ResponseResult<string>()
                    {
                        ResponseCode = ResponseCode.ValidationFailed,
                        Message = ResponseMessage.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.ValidationFailed
                        }
                    };
                }

                #endregion

                #region[Get Reservation Detail]
                //Console.WriteLine("Called::_reservationRepository.GetReservationDetailById-Get Reservation Detail");
                var reservation = this._reservationRepository.GetReservationById(model.ReservationId, model.AccountId);
                var parkingReservation = reservation.ParkingReservation.FirstOrDefault();
                var reservationVehicle = reservation.ReservationVehicle.FirstOrDefault();
                var vehicleDetails = this._vehiclesRepository.GetVehicleDetailsById(reservationVehicle.VehicleId,model.AccountId);

                if (reservation == null)
                {
                    return new ResponseResult<string>()
                    {
                        ResponseCode = ResponseCode.NoRecordFound,
                        Message = ResponseMessage.NoRecordFound,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.NoRecordFound
                        }
                    };
                }
                #endregion

                #region [Save Invoice Detail]
                //Console.WriteLine("Save Invoice Detail");
                string pdfHtml = "";
                var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
                var pdfHtmlFilePath = Path.Combine("Helpers", "Docs", "EmailTemplate", "ReservationDetail.html");
                //Console.WriteLine("pdfHtmlFilePath::pdfHtmlFilePath- "+ pdfHtmlFilePath);
                var fileInfo = provider.GetFileInfo(pdfHtmlFilePath);
                if (!fileInfo.Exists)
                {
                    return new ResponseResult<string>()
                    {
                        Message = "Print Reservation Detail Pdf HTML file is missing.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                using (var fs = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(fs))
                    {
                        pdfHtml = sr.ReadToEnd();
                    }
                }

                if (string.IsNullOrWhiteSpace(pdfHtml))
                {
                    return new ResponseResult<string>()
                    {
                        Message = "Print Reservation Detail Pdf HTML file is blank.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
                
                var pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "Docs", "Temp", $"ReservationDetail_{Guid.NewGuid()}.pdf");
                pdfHtml = pdfHtml.Replace("##TicketNo##", !string.IsNullOrWhiteSpace(vehicleDetails.Ticket)? vehicleDetails.Ticket : "N/A")
                    .Replace("##SampleResNo##", reservation.ReservationCode)
                   .Replace("##CustomerName##", reservation.User.FirstName + " " + reservation.User.LastName)
                   .Replace("##CustomerEmail##", reservation.User.EmailAddress).Replace("##CustomerMob##", reservation.User.Mobile)
                    .Replace("##CarManufacturer##", vehicleDetails.Make).Replace("##CarColor##", vehicleDetails.Color)
                    .Replace("##CustomerLicenseNo##", vehicleDetails.LicensePlate).Replace("##ReturnDate##", parkingReservation.CheckOutDateTime.ToString().Split(" ")[0])
                    .Replace("##ReturnTime##", parkingReservation.CheckOutDateTime.ToString().Split(" ")[1]).Replace("##CheckedInDate##", parkingReservation.CheckInDateTime.ToString().Split(" ")[0])
                    .Replace("##CheckedInTime##", parkingReservation.CheckInDateTime.ToString().Split(" ")[1]);

                //Console.WriteLine("Read and Update PDF invNo- " + invNo);
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "Docs", "Temp")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "Docs", "Temp"));
                }
                //Console.WriteLine("GetPDFBytes UserId- " + model.UserId+" "+ reservation.Mobile);
                var pdfBytes = PDFHelper.GetPDFBytes(pdfHtml, pdfFilePath);

                string path = "";
                if (string.IsNullOrEmpty(path))
                {
                    return new ResponseResult<string>()
                    {
                        Message = "Failed to Save Print Reservation in to the Server.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }

                try
                {
                    if (File.Exists(pdfFilePath))
                        File.Delete(pdfFilePath);
                }
                catch { }
                #endregion

                return new ResponseResult<string>()
                {
                    ResponseCode = ResponseCode.RecordSaved,
                    Message = ResponseMessage.RecordSaved,
                    Data = path
                };
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private string GetInvoiceName(string airportCode, string locationCode, long reservationId)
        {
            var invoiceNo = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(airportCode))
            {
                invoiceNo.Append((airportCode.Length > 3 ? airportCode.Substring(0, 3) : airportCode).Trim());
            }
            if (!string.IsNullOrWhiteSpace(locationCode))
            {
                invoiceNo.Append((locationCode.Length > 3 ? locationCode.Substring(0, 3) : locationCode).Trim());
            }
            invoiceNo.Append(DateTime.UtcNow.ToString("ddMMyyyyHHmm") + reservationId.ToString());
            return invoiceNo.ToString().ToUpper();
        }

        private string CreateEmailSendContentTable(List<InvoicePriceDetailVM> invoiceData, decimal totalAmount)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table width='100%' cellspacing='0px' class='item-table' style='font-size:14px; border:1px solid #593dba;'>"
                + "<thead style='background-color: #393d41; color:#fff;'>" +
                       "<tr>" +
                           "<th align='left' style='padding:15px; background-color: #393d41; border-right:1px solid #fff;'> Items </th>" +
                           "<th style='padding:15px; background-color: #393d41; border-right:1px solid #fff;'>Rate</th>" +
                           "<th style='padding:15px; background-color: #393d41; border-right:1px solid #fff;'>Quantity</th>" +
                           "<th style='padding:15px; background-color: #393d41; border-right:1px solid #fff;'>Amount</th>" +
                           "<th style='padding:15px; background-color: #393d41; border-right:1px solid #fff;'>Discount</th>" +
                           "<th style='padding:15px; background-color: #393d41; border-right:1px solid #fff;'>Final Amount</th>" +
                         "</tr>" +
                    "</thead>"
                   );

            html.Append("<tbody>");
            if (invoiceData?.Count > 0)
            {
                foreach (var items in invoiceData)
                {
                    html.Append(
                        "<tr>"
                        + "<td align='left' style='padding:15px;'>" + items.Description + "</td>"
                        + "<td align='center' style='padding:15px;'>" + items.Rate + "</td>"
                        + "<td align='center' style='padding:15px;'>" + items.Qty + "</td>"
                        + "<td align='center' style='padding:15px;'>" + items.Amount.ToString("00.00") + "</td>"
                        + "<td align='center' style='padding:15px;'>" + items.DiscountAmount?.ToString("00.00") + "</td>"
                        + "<td align='right' style='padding:15px;'>" + (items.Amount - items.DiscountAmount ?? 0).ToString("00.00") + "</td>"
                        + "</tr>");

                }
                html.Append(
                       "<tr>"
                       + "<td style='background-color:#593dba; color:#fff; padding:15px;'>Total</td>"
                       + "<td colspan='5' align='right' style='background-color:#593dba; color:#fff; padding:15px;'>" + totalAmount.ToString("00.00") + " USD</td>"
                       + "</tr>"
                       +"</tbody>"
                       +"</table>");
            }
           
            return html.ToString();
        }

        //private string CreateReservationContent(ReservationDetailVM reservation)
        //{
        //    StringBuilder html = new StringBuilder();
        //    html.Append("<table width='100%'style='font-size:16px;font-weight:bold;text-align:center;'><tbody><tr> <td>No. 112517<td> </tr></tbody></table><table width='100%'style='font-size:16px;text-align:center;margin-top:15px;font-weight:bold;'><tbody><tr> <td>PARKING TICKET INVOICE</td> </tr></tbody></table><table width='100%'style='font-size:24px;text-align:center;margin-top:15px;font-weight:bold;'><tbody><tr> <td>No. PWPMIAP112517</td> </tr></tbody></table> <table width='100%'style='font-size:15px; margin-top:10px;'><tbody> <tr> <td width='30%'><b>Parker of Reservation # :</b></td> <td>PWPMIAP112517</td> </tr> <tr> <td><b>NAME :</b></td> <td>Tomer Brenner</td> </tr> <tr> <td><b>Email :</b></td> <td>ask@forit.com</td> </tr> <tr> <td><b>PHONE :</b></td> <td>2037703224</td> </tr> <tr> <td><b>CAR MAKE :</b></td> <td>HYNDAI COLOR : gray</td> </tr> <tr> <td><b>LICENSE NO. :</b></td> <td>QVAV66</td> </tr></tbody></table><table width='100%'style='font-size:14px;font-weight:bold;margin-top:15px;'><tbody><tr> <td>RETURN DATE : 17-03-2023 TIME : 09:51</td> </tr></tbody></table> <table width='100%' style='font-size:14px;margin-top:15px;'><tbody><tr> <td colspan=\"4\" style='font-weight:bold;text-align:center;'>Office Use<td> <td></td> <td></td> <td></td> </tr> <tr> <td>SPACE<td> <td></td> <td>INITIALS</td> <td></td> </tr> <tr> <td>SPACE<td> <td></td> <td>INITIALS</td> <td></td> </tr></tbody></table> <table width='100%'style='margin-top:15px;text-align:center;'><tbody><tr> <td>Ask us about the benefits of our Premier Parker Program andsign up today.<td> </tr></tbody></table> ");
            
        //    return html.ToString();
        //}

        /// <summary>
        /// IsInputFormDataIsValid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private ResponseResult<ParkingReservationAmountAndLocationVM> IsInputFormDataIsValid(AddUpdateFlightAndParkingReservationVM model)
        {
            var responseResult = new ResponseResult<ParkingReservationAmountAndLocationVM>();
            Dictionary<string, string[]> errorData = new Dictionary<string, string[]>();
            if (model != null)
            {
                if (model.DepaurtureDateTime == null || model.DepaurtureDateTime == DateTime.MinValue)
                    errorData.Add("depaurtureDateTime", new[] { "Please enter valid depaurture date time." });
                if (model.ReturnDateTime == null || model.ReturnDateTime == DateTime.MinValue)
                    errorData.Add("returnDateTime", new[] { " Please enter valid return date time." });

                //if (model.DepaurtureDateTime != null && model.DepaurtureDateTime < DateTime.UtcNow)
                //    errorData.Add("depaurtureDateTime", new[] { " Please enter valid depaurture date." });

                if (model.DepaurtureDateTime != null && model.ReturnDateTime != null
                    && model.DepaurtureDateTime > model.ReturnDateTime)
                    errorData.Add("returnDateTime", new[] { " Please enter valid return date." });

                if (model.DepaurtureAirportId == 0)
                    errorData.Add("depaurtureAirportId", new[] { "Please enter valid depaurture airport Id." });
                if (model.ReturningToAirportld == 0)
                    errorData.Add("returningToAirportld", new[] { "Please enter valid return airport Id." });
                if (model.FlyingToAirportld == 0)
                    errorData.Add("flyingToAirportld", new[] { "Please enter valid flying to airport Id." });
                if (string.IsNullOrWhiteSpace(model.FlyingToAirline))
                    errorData.Add("flyingToAirline", new[] { "Please enter valid flying to airline name." });
                if (string.IsNullOrWhiteSpace(model.FlyingToFlightNo))
                    errorData.Add("flyingToFlightNo", new[] { "Please enter valid flying to flight number." });
                if (string.IsNullOrWhiteSpace(model.ReturningToAirline))
                    errorData.Add("returningToAirline", new[] { "Please enter valid return to airline name." });
                if (string.IsNullOrWhiteSpace(model.ReturningToFlightNo))
                    errorData.Add("returningToFlightNo", new[] { "Please enter valid return to flight number." });

                if (errorData != null && errorData.Count > 0)
                {
                    responseResult.Message = ResponseMessage.ValidationFailed;
                    responseResult.ResponseCode = ResponseCode.ValidationFailed;
                    responseResult.Error = new ErrorResponseResult()
                    {
                        Detail = errorData
                    };
                }
            }
            else
            {
                responseResult.Message = "Invalid request. Data model is null";
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseCode.ValidationFailed
                };
            }
            return responseResult;
        }

        /// <summary>
        /// Get Shuttle ETA
        /// </summary>
        /// <returns></returns>
        public ResponseResult<string> ShuttleETA()
        {
            return new ResponseResult<string>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = "10 minutes"
            };
        }

        /// <summary>
        /// UpdateCreateReservationActivtyCode
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ScannedResponseVM>> UpdateCreateReservationActivtyCode(ScannedVM model, long accountId, long userId)
        {
            string subLocationType, activityCode;
            long subLocationId, parkingSpotId=0;
            Dictionary<string, string> reservationData = new Dictionary<string, string>();
            if (model == null)
            {
                return new ResponseResult<ScannedResponseVM>()
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ResponseMessage.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            if (string.IsNullOrEmpty(model.QRCodeMapping) && string.IsNullOrEmpty(model.ScannedData))
            {
                return new ResponseResult<ScannedResponseVM>()
                {
                    ResponseCode = ResponseCode.QRCodeInvalid,
                    Message = ResponseMessage.QRCodeInvalid,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.QRCodeInvalid
                    }
                };
            }
            if (string.IsNullOrEmpty(model.QRCodeMapping))
            {
                reservationData = DecryptAndSplit(model.ScannedData);
            }
            if (!string.IsNullOrEmpty(model.QRCodeMapping))
            {
                var subLocation = await _parkingProvidersSubLocationsRepository.GetSubLocationByQRCodeMapping(model.QRCodeMapping);
                if (subLocation == null || string.IsNullOrEmpty(subLocation.QrcodeEncryptedValue))
                    return new ResponseResult<ScannedResponseVM>()
                    {
                        ResponseCode = ResponseCode.NoRecordFound,
                        Message = ResponseMessage.NoRecordFound,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.NoRecordFound
                        }
                    };
                reservationData = DecryptAndSplit(subLocation.QrcodeEncryptedValue);
            }

            subLocationId = Convert.ToInt64(reservationData["SubLocationId"]);
            subLocationType = reservationData["SubLocationType"];
            activityCode = reservationData["ActivityCode"];
            if (reservationData.ContainsKey("ParkingSpotId"))
                parkingSpotId = Convert.ToInt64(reservationData["ParkingSpotId"]);

            

            if (string.IsNullOrEmpty(subLocationType) || subLocationId == 0)
            {
                return new ResponseResult<ScannedResponseVM>()
                {
                    ResponseCode = ResponseCode.QRCodeInvalid,
                    Message = ResponseMessage.QRCodeInvalid,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.QRCodeInvalid
                    }
                };
            }
            var activity = _activityCodeRepository.GetActivityIdByCode(activityCode);

            if (_reservationActivityCodeRepository.CheckAlreadyExist(activity.ActivityCodeId, model.ReservationId, accountId))
            {
                return new ResponseResult<ScannedResponseVM>()
                {
                    ResponseCode = ResponseCode.RecordAlreadyExist,
                    Message = ResponseMessage.RecordAlreadyExist,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.RecordAlreadyExist
                    }
                };
            }

            string scannedBy = model.ScannedBy;
            if (string.IsNullOrWhiteSpace(scannedBy)) scannedBy = "C";
            if (string.IsNullOrWhiteSpace(model.ActivityDoneBy)) model.ActivityDoneBy = scannedBy;

            ScannedResponseVM result = null;

            //If no record exist in ReservationActivityCode table. 
            try
            {
                result = _reservationActivityCodeRepository.GetCurrentAndNextActivityCode<ScannedResponseVM>(model.ReservationId, accountId, scannedBy);
            }
            catch
            {

            }

            if (result != null && !string.IsNullOrEmpty(result.NextActivityCode) && result.NextActivityCode.ToLower().Trim() != activityCode.ToLower().Trim())
            {
                return new ResponseResult<ScannedResponseVM>()
                {
                    ResponseCode = ResponseCode.QRCodeInvalid,
                    Message = ResponseMessage.QRCodeInvalid,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.QRCodeInvalid
                    }
                };
            }

            // Get Parking spot Details
            string parkingStopName = "";
            if (activityCode == "PV" || activityCode == "VSLS" || activityCode == "VKLS")
            {
                if (parkingSpotId > 0)
                {
                    var parkingSpot = await _parkingSpotsRepository.GetParkingSpotById(parkingSpotId, accountId);
                    if (parkingSpot == null || string.IsNullOrWhiteSpace(parkingSpot.Name))
                    {
                        return new ResponseResult<ScannedResponseVM>()
                        {
                            ResponseCode = ResponseCode.ReservationActivityNotCreated,
                            Message = "Parking Spot details are missing",
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.ReservationActivityNotCreated
                            }
                        };
                    }
                    parkingStopName = parkingSpot.Name;
                }

                if (string.IsNullOrWhiteSpace(parkingStopName))
                {
                    return new ResponseResult<ScannedResponseVM>()
                    {
                        ResponseCode = ResponseCode.ReservationActivityNotCreated,
                        Message = "Parking Spot details are missing",
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.ReservationActivityNotCreated
                        }
                    };
                }
            }

            // Update Parking Start Date And End date
            if (activityCode == "AP" || activityCode == "VPC"
                || activityCode == "PV" || activityCode == "VSLS"
                || activityCode == "VKLS")
            {
                var parkingReservation = await _parkingReservationRepo.GetParkingReservationByReservationId(model.ReservationId, accountId);
                if (parkingReservation != null)
                {
                    switch (activityCode)
                    {
                        case "AP":      // Arrive at Parking
                            parkingReservation.StartDateTime = DateTime.UtcNow;
                            break;

                        case "VPC":     // Vehicle Pickedup by Customer
                            parkingReservation.EndDateTime = DateTime.UtcNow;
                            break;

                        case "PV":      // Parked at Vallet
                            parkingReservation.ValletLocation = parkingStopName;
                            break;

                        case "VSLS":    //Vehicle Storage Location Scanned
                            parkingReservation.VehicleLocation = parkingStopName;
                            break;

                        case "VKLS":    // Vehicle Key Location Scanned
                            parkingReservation.VehicleKeyLocation = parkingStopName;
                            break;
                    }

                    parkingReservation.UpdatedBy = userId;
                    parkingReservation.UpdatedOn = DateTime.UtcNow;

                    _parkingReservationRepo.UpdateParkingReservation(parkingReservation);
                }
                else
                {
                    return new ResponseResult<ScannedResponseVM>()
                    {
                        ResponseCode = ResponseCode.ReservationActivityNotCreated,
                        Message = "Parking Reservation details are missing",
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.ReservationActivityNotCreated
                        }
                    };
                }
            }

            ReservationActivityCode reservationActivityCode = new ReservationActivityCode();
            reservationActivityCode.AccountId = accountId;
            reservationActivityCode.ActivityCodeId = activity.ActivityCodeId;
            reservationActivityCode.ReservationId = model.ReservationId;
            reservationActivityCode.ParkingProvidersLocationSubLocationId = subLocationId;
            reservationActivityCode.ActivityDoneBy = model.ActivityDoneBy;
            reservationActivityCode.CreatedOn = DateTime.UtcNow;
            reservationActivityCode.CreatedBy = userId;
            reservationActivityCode.UpdatedOn = DateTime.UtcNow;
            reservationActivityCode.UpdatedBy = userId;

            await this._reservationActivityCodeRepository.CreateReservationActivtyCode(reservationActivityCode);
            if (unitOfWork.CommitWithStatus() < 1)
            {
                return new ResponseResult<ScannedResponseVM>()
                {
                    ResponseCode = ResponseCode.ReservationActivityNotCreated,
                    Message = ResponseMessage.ReservationActivityNotCreated,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ReservationActivityNotCreated
                    }
                };
            }

            var getCurrentandNextActivity = _reservationActivityCodeRepository.GetCurrentAndNextActivityCode<ScannedResponseVM>(model.ReservationId, accountId, scannedBy);

           

            return new ResponseResult<ScannedResponseVM>()
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Data = getCurrentandNextActivity
            };
        }

        public ResponseResult<CarDetailVM> GetReservationVehicleDetailsByVinNumber(long reservationId, string vinNumber, long accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseResult<int>> UpdateParkingReservation(UpdateIsParkedVM model, long accountId, long userId)
        {
            var reservation = _reservationRepository.GetReservationById(model.ReservationId, accountId);
            if (reservation == null || reservation.ParkingReservation?.Count < 1
                || reservation.ParkingReservation.FirstOrDefault().ParkingProvidersLocationId < 1)
            {
                return new ResponseResult<int>()
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }

            int result = _reservationRepository.Update_ParkingReservation(model.ReservationId, accountId, model.IsParked);
            if (result == 0)
            {
                return new ResponseResult<int>()
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }

            if (model.IsParked)
            {
                // Insert VSV Activity Code
                var getCurrentandNextActivity = _reservationActivityCodeRepository.GetCurrentAndNextActivityCode<ScannedResponseVM>(model.ReservationId, accountId, "PA");
                if (getCurrentandNextActivity.NextActivityCode == "VKLS")
                {
                    var parkingProvidersLocationId = reservation.ParkingReservation.FirstOrDefault().ParkingProvidersLocationId ?? 0;
                    var parkingProvidersSubLocation = await _parkingProvidersSubLocationsRepository.GetSubLocationId("VKLS", parkingProvidersLocationId, "Entry");
                    if (parkingProvidersSubLocation == null)
                    {
                        return new ResponseResult<int>()
                        {
                            ResponseCode = ResponseCode.ReservationActivityNotCreated,
                            Message = "Prking Providers Sub Locations details are missing.",
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.ReservationActivityNotCreated
                            }
                        };
                    }

                    ReservationActivityCode reservationActivityCode = new ReservationActivityCode();
                    reservationActivityCode.AccountId = accountId;
                    reservationActivityCode.ActivityCodeId = parkingProvidersSubLocation.ActivityCodeId;
                    reservationActivityCode.ReservationId = model.ReservationId;
                    reservationActivityCode.ParkingProvidersLocationSubLocationId = parkingProvidersSubLocation.ParkingProvidersLocationSubLocationId;
                    reservationActivityCode.ActivityDoneBy = "Skipped";
                    reservationActivityCode.CreatedOn = DateTime.UtcNow;
                    reservationActivityCode.CreatedBy = userId;
                    reservationActivityCode.UpdatedOn = DateTime.UtcNow;
                    reservationActivityCode.UpdatedBy = userId;

                    await _reservationActivityCodeRepository.CreateReservationActivtyCode(reservationActivityCode);

                    if (unitOfWork.CommitWithStatus() < 1)
                    {
                        return new ResponseResult<int>()
                        {
                            ResponseCode = ResponseCode.ReservationActivityNotCreated,
                            Message = ResponseMessage.ReservationActivityNotCreated,
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.ReservationActivityNotCreated
                            }
                        };
                    }
                }
            }

            return new ResponseResult<int>()
            {
                ResponseCode = ResponseCode.ParkingStatus,
                Message = ResponseMessage.ParkingStatus,
            };
        }

        public async Task<ResponseResult<OngoingUpcomingTripVM>> GetJourneyCompletedlist(long accountId, long userId)
        {
            var data = new OngoingUpcomingTripVM();

            var flightReservationQuery = this._flightReservationRepository.GetAllFlightReservation(accountId, userId);
            var airportQuery = this._airportsRepo.GetAllAiportsWithAddress(accountId);
            var parkingLocationQuery = this._parkingReservationRepo.GetParkingReservationQueryByDate(accountId);
            var addressQuery = this._addressRepo.GetAddressQuery(accountId);
            var activeCodeQuery = this._reservationActivityCodeRepository.GetReservationActivityCodeQuery(accountId);

            var resultQuery = from f in flightReservationQuery
                              join da in airportQuery on f.DepaurtureAirportId equals da.AirportId
                              join ra in airportQuery on f.ReturningToAirportld equals ra.AirportId
                              join pr in parkingLocationQuery on f.ReservationId equals pr.ReservationId
                              join ad in addressQuery on pr.ParkingProvidersLocation.AddressId equals ad.AddressId
                              select new TripDetailVM()
                              {
                                  ReservationId = f.ReservationId,
                                  FlightReservationId = f.FlightReservationId,
                                  DepaurtureDateTime = f.DepaurtureDateTime,
                                  FlightNo = f.FlyingToFlightNo,
                                  AirlineCode = f.FlyingToAirline,
                                  ArrivalAirportCode = ra.Code,
                                  ArrivalAirportName = ra.Name,
                                  DepaurtureAirportAddress = (da.Address.Streat1 ?? "" + " " + da.Address.Streat2 ?? "" + " " + da.Address.City ?? "" + " " + da.Address.State ?? ""),
                                  DepaurtureAirportCode = da.Code,
                                  DepaurtureAirportName = da.Name,
                                  DepaurtureCity = da.Address.City ?? "",
                                  ParkingLocationAddress = (ad.Streat1 ?? "" + " " + ad.Streat2 ?? "" + " " + ad.City ?? "" + " " + ad.State ?? ""),
                                  ActivityCode = activeCodeQuery.Where(x => x.ReservationId == f.ReservationId).Select(x => x.ActivityCode).OrderByDescending(x => x.Odering).FirstOrDefault().Code,
                                  Latitude = ad.Latitude,
                                  Longitude = ad.Longitude,
                                  ParkingProvidersLocationId = pr.ParkingProvidersLocationId ?? 0
                              };

            data.CompletedTrips = await resultQuery.Where(x => x.ActivityCode == "VLP").OrderByDescending(x => x.DepaurtureDateTime).ToListAsync();
            if (data == null || data.CompletedTrips.Count == 0)
            {
                return new ResponseResult<OngoingUpcomingTripVM>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            }

            return new ResponseResult<OngoingUpcomingTripVM>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public Dictionary<string, string> DecryptAndSplit(string encrypted)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string dycriptedData = String.Empty;
            if(string.IsNullOrEmpty(dycriptedData))
                dycriptedData = _commonHelper.DecryptString(encrypted, _commonConfig.ScanEncryptionKey);
            dycriptedData = dycriptedData.Replace("\"", string.Empty).Trim();
            string[] scannedDataArray = scannedDataArray = dycriptedData.Split("|");
            if (string.IsNullOrEmpty(dycriptedData))
            {
                return null;

            }
            if (scannedDataArray.Length < 3)
            {
                return null;

            }
            result.Add("SubLocationId", scannedDataArray[0]);
            result.Add("SubLocationType", scannedDataArray[1]);
            result.Add("ActivityCode", scannedDataArray[2]);

            if (scannedDataArray.Length > 3)
            {
                result.Add("ParkingSpotId", scannedDataArray[3]);
            }
            return result;
        }

       
        private string SavePDF( byte[] fileByte,long accountId,string mobileNo, string fileUniqueName, int mediaReferenceEnum)
        {
            //Console.WriteLine("Save PDF Start");
            var mediaUploadView = new MediaUploadView();
            //string returnedPath = 0;
            Document document = new Document()
            {
                FileGuid = System.Guid.NewGuid(),
                FileName = $"{Guid.NewGuid()}",
                FileExtenstion = "*.pdf",
                Filebytes = fileByte,
                FileUniqueName = fileUniqueName
            };

            mediaUploadView.Documents = new List<Document>();
            mediaUploadView.ReferenceId = accountId;
            mediaUploadView.ReferenceType = mediaReferenceEnum;
            mediaUploadView.CreatedBy = Convert.ToInt64(mobileNo);
            mediaUploadView.Documents.Add(document);

            
            return "";
        }

        public ResponseResult<List<ShuttleBoardedListVM>> GetShuttleBoardedList(int terminalId)
        {
            var data = _reservationRepository.GetShuttleBoardedList<ShuttleBoardedListVM>(terminalId);
            if (data == null || data.Count == 0)
            {
                return new ResponseResult<List<ShuttleBoardedListVM>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = new List<ShuttleBoardedListVM>()
                };
            }
            return new ResponseResult<List<ShuttleBoardedListVM>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<int> DeBoardFromShuttle(DeBoardFromShuttleVM model, long accountId, long userId)
        {
            var activity = _activityCodeRepository.GetActivityIdByCode(model.ActivityCode);
            var data = _reservationRepository.UpdateDeboard(activity.ActivityCodeId, model.ReservationActivityId,userId);
           
            return new ResponseResult<int>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
            
        }
        public async Task<ResponseResult<List<ReservationHistoryVM>>> GetReservationsByParkingLocationId(ReservationSearchVM model, long accountId, long userId)
        {
            if (model.FromDate == DateTime.MinValue || model.ToDate == DateTime.MinValue)
            {
                model.FromDate = DateTime.UtcNow.AddMonths(-2);
                model.ToDate = DateTime.UtcNow;
            }
            var data = _reservationRepository.GetReservationsByParkingLocationId<ReservationHistoryVM>(model, userId, accountId);
            if (data == null || data.Count == 0)
            {
                return new ResponseResult<List<ReservationHistoryVM>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = new List<ReservationHistoryVM>()
                };
            }

            foreach (var item in data)
            {

                var user = await _userRepository.GetUser(accountId, item.UserId);
                //item.ReservationCount = _reservationRepository.GetAllReservationListByUserId(item.UserId).Count;
                if (user != null)
                {
                    var userDetail = new UserVM()
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailAddress = user.EmailAddress,
                        Mobile = user.Mobile,
                        UserName = user.UserName,
                        UserStatus = user.UserStatus,
                        StripeCustomerId = user.StripeCustomerId,
                        MobileCode = user.MobileCode,

                    };
                    item.UserDetail = userDetail;
                }

            }
            return new ResponseResult<List<ReservationHistoryVM>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }
        public async Task<ResponseResult<List<FlightAndParkingReservationVM>>> GetReservationsByUserId(long accountId, long userId)
        {
            List<FlightAndParkingReservationVM> lstReservationVM = new List<FlightAndParkingReservationVM>();
            var reservationList = _reservationRepository.GetAllReservationListByUserId(userId);
            foreach (var reservation in reservationList.OrderBy(x => x.IsCancelled))
            {
                var dataNew = new FlightAndParkingReservationVM()
                {
                    ReservationId = reservation.ReservationId,
                    ReservationCode = reservation.ReservationCode,
                    UserId = reservation.UserId,
                    IsCancel= reservation.IsCancelled ?? false  
                };

                dataNew.FlightReservation = mapper.Map<FlightReservationVM>(reservation.FlightReservation.FirstOrDefault());
                dataNew.ParkingReservation = mapper.Map<ParkingReservationVM>(reservation.ParkingReservation.FirstOrDefault());
                dataNew.ReservationVehicle = mapper.Map<ReservationVehicleVM>(reservation.ReservationVehicle.FirstOrDefault());

                lstReservationVM.Add(dataNew);
            }

            return new ResponseResult<List<FlightAndParkingReservationVM>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = lstReservationVM
            };
        }

        public async Task<ResponseResult<ParkingReserAmountAndLocationVM>> CreateReservationNew(AddUpdateFlightReservationVM model,long accountId)
        {
            
                // Validations
                //var validateModelResult = IsInputFormDataIsValid(model);
                //if (validateModelResult.ResponseCode == ResponseCode.ValidationFailed)
                //{
                //    return new ResponseResult<ParkingReserAmountAndLocationVM>()
                //    {
                //        ResponseCode = ResponseCode.ValidationFailed,
                //        Message = validateModelResult.Message,
                //        Error = validateModelResult.Error
                //    };
                //}
                long airportsParkingId = 0;
                var priceResult = await GetReservationPriceAndAddressData(model, accountId, airportsParkingId);
                


                AddFlightReservationVM finalModel = mapper.Map<AddFlightReservationVM>(model);
            if (priceResult == null || priceResult.Data == null)
            {
                return new ResponseResult<ParkingReserAmountAndLocationVM>()
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Invalid departure or return date."
                    }
                };
            }
                finalModel.InTimeGap = priceResult.Data.InTimeGap;
                finalModel.OutTimeGap = priceResult.Data.OutTimeGap;
                finalModel.TotalFinalAmount = priceResult.Data.ParkingTotalAmount;
                finalModel.AirportsParkingId = priceResult.Data.AirportsParkingId;
                finalModel.ParkingProvidersLocationId = priceResult.Data.ParkingProvidersLocationId;
                finalModel.ReservationCode = model.DepaurtureAirportId + DateTime.Now.Ticks.ToString();

                var result = await _reservationRepository.CreateReservationFromSP(finalModel);
                if (result.ReservationId > 0)
                {
                /*
                    #region [Kafka Call to Create Payment Intent]
                    var kafkaRequestModel = new KafkaRequestModel<CreatePaymentIntentRequest>()
                    {
                        MessageType = "CreatePaymentIntent",
                        MessageData = new CreatePaymentIntentRequest()
                        {
                            Amount = Convert.ToInt64(model.TotalFinalAmount * 100),
                            AccountId = model.AccountId,
                            UserId = model.UserId,
                            ReservationId = result.ReservationId,
                            ReservationCode = finalModel.ReservationCode
                        }
                    };
                    var producer = new ProducerWrapper(_kafkaConfig, "ReservationPaymentTopic");
                    await producer.writeMessage(JsonConvert.SerializeObject(kafkaRequestModel));
                    #endregion
                */
                    var reservation = _reservationRepository.GetReservationById(result.ReservationId, accountId);
                    if (reservation == null)
                    {
                        return new ResponseResult<ParkingReserAmountAndLocationVM>()
                        {
                            ResponseCode = ResponseCode.NoRecordFound,
                            Message = ResponseMessage.NoRecordFound,
                            Error = new ErrorResponseResult()
                            {
                                Message = ResponseMessage.NoRecordFound
                            }
                        };
                    }

                    //var flightReservationCollection = reservation.FlightReservation;
                    //var flightReservation = flightReservationCollection.FirstOrDefault();

                    var parkingReservationCollection = reservation.ParkingReservation;
                    var parkingReservation = parkingReservationCollection.FirstOrDefault();

                    
                    //if (flightReservation.DepaurtureAirportId == model.DepaurtureAirportId)
                    //{
                    //    airportsParkingId = parkingReservation.AirportsParkingId;
                    //}
                   
                    
                    ParkingReservationAmountAndLocationVM finalModelNew = new ParkingReservationAmountAndLocationVM()
                    {
                        InTimeGap = priceResult.Data.InTimeGap,
                        OutTimeGap = priceResult.Data.OutTimeGap,
                        ParkingTotalAmount = priceResult.Data.ParkingTotalAmount,
                        AirportsParkingId = priceResult.Data.AirportsParkingId,
                        ParkingProvidersLocationId = priceResult.Data.ParkingProvidersLocationId,
                        ReservationId = result.ReservationId,
                        //FlightReservationId= flightReservation.FlightReservationId,
                        ParkingAddress=priceResult.Data.ParkingAddress,
                        UserId=model.UserId
                    };
                    
                   

                    

                    return new ResponseResult<ParkingReserAmountAndLocationVM>()
                    {
                        ResponseCode = ResponseCode.RecordSaved,
                        Message = ResponseMessage.RecordSaved,
                        Data = finalModelNew
                    };
                }
                else
                {
                    return new ResponseResult<ParkingReserAmountAndLocationVM>()
                    {
                        ResponseCode = ResponseCode.SomethingWentWrong,
                        Message = result.ErrorMessage ?? ResponseMessage.SomethingWentWrong,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.SomethingWentWrong
                        }
                    };
                }
          
        }

        public async Task<ResponseResult<ParkingReservationAmountAndLocationVM>> UpdateReservationFromProviderPortal(AddUpdateFlightAndParkingReservationVM model, long reservationId, long accountId, long userId)
        {
             // Validations Removed


                var reservation = _reservationRepository.GetReservationById(reservationId, accountId);
                if (reservation == null)
                {
                    return new ResponseResult<ParkingReservationAmountAndLocationVM>()
                    {
                        ResponseCode = ResponseCode.NoRecordFound,
                        Message = ResponseMessage.NoRecordFound,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.NoRecordFound
                        }
                    };
                }
                long airportsParkingId = 0;
                var flightReservationCollection = reservation.FlightReservation;

                var flightReservation = flightReservationCollection == null ? new FlightReservation() : flightReservationCollection.FirstOrDefault();

                var parkingReservationCollection = reservation.ParkingReservation;
                var parkingReservation = parkingReservationCollection == null ? new ParkingReservation() : parkingReservationCollection.FirstOrDefault();

                #region [Vatildate Data, Get Price And Address]

                
                var priceResult = await GetReservationPriceAndAddressData(model, accountId, airportsParkingId);

                if (priceResult.ResponseCode != ResponseCode.RecordFetched) return priceResult;
                #endregion


                // Update Reservation Table
                reservation.IsChanged = true;
                reservation.UpdatedBy = userId;
                reservation.UpdatedOn = DateTime.UtcNow;

                
                // Update Parking Reservation
                var startDate = priceResult.Data.InTimeGap > 0 ? model.DepaurtureDateTime.AddMinutes(-priceResult.Data.InTimeGap) : model.DepaurtureDateTime;
                var endDate = priceResult.Data.OutTimeGap > 0 ? model.ReturnDateTime.AddMinutes(priceResult.Data.OutTimeGap) : model.ReturnDateTime;
                if (parkingReservationCollection.Count > 0)
                {
                    parkingReservation.ParkingProvidersLocationId = priceResult.Data.ParkingProvidersLocationId;
                    parkingReservation.AirportsParkingId = priceResult.Data.AirportsParkingId;
                    parkingReservation.StartDateTime = startDate;
                    parkingReservation.EndDateTime = endDate;
                    parkingReservation.IsConcentedToRent = model.IsBorrowingCarForRent;
                    parkingReservation.UpdatedOn = DateTime.UtcNow;
                    parkingReservation.UpdatedBy = userId;
                    parkingReservation.Comment = model.Comment;
                    parkingReservation.SourceId = model.SourceId;
                    parkingReservation.BookingConfirmationNo = model.BookingConfirmationNo;
                    reservation.ParkingReservation = parkingReservationCollection;
                }
                _reservationRepository.UpdateReservation(reservation);
                if (unitOfWork.CommitWithStatus() > 0)
                {
                    priceResult.Data.ReservationId = reservationId;
                    priceResult.ResponseCode = ResponseCode.RecordSaved;
                    priceResult.Message = ResponseMessage.RecordSaved;
                    return priceResult;
                }
                else
                {
                    return new ResponseResult<ParkingReservationAmountAndLocationVM>()
                    {
                        Message = "Could not able to save details.",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
            
        }

    }
}