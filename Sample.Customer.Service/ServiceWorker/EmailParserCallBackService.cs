using Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class EmailParserCallBackService : IEmailParserCallBackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailParserDetailRepository _emailRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAirportAddressService _airportAddressService;
        private readonly IReservationService _reservationService;

        public EmailParserCallBackService(IUnitOfWork unitOfWork, IEmailParserDetailRepository emailRepo,
            IUserRepository userRepo, IAirportAddressService airportAddressService, 
            IReservationService reservationService)
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(emailRepo), emailRepo);
            Check.Argument.IsNotNull(nameof(userRepo), userRepo);
            Check.Argument.IsNotNull(nameof(airportAddressService), airportAddressService);
            Check.Argument.IsNotNull(nameof(reservationService), reservationService);

            _unitOfWork = unitOfWork;
            _emailRepo = emailRepo;
            _userRepo = userRepo;
            _airportAddressService = airportAddressService;
            _reservationService = reservationService;
        }

        //public ResponseResult<bool> UpdateCallBackResult(EmailParserCallBackVM model)
        //{
        //    EmailParserCallBack emailParserCallBack= emailParserCallBackRepo.GetResultByMsgType(model.MsgId);
        //    emailParserCallBack.RequestId = model.RequestId;
        //    emailParserCallBack.UpdatedBy = model.LoggedUserId;
        //    emailParserCallBack.UpdatedOn = DateTime.Now;
        //    emailParserCallBackRepo.UpdateCallBackResult(emailParserCallBack);
        //    return new ResponseResult<bool>()
        //    {
        //        Message = ResponseMessage.RecordSaved,
        //        ResponseCode = ResponseCode.RecordSaved,
        //        Data = true
        //    };

        // }

        public async Task<ResponseResult<EmailParseDetails>> GetDetailsFromMessageId(Guid messageId, long accountId)
        {
            var msgDetails = await _emailRepo.GetResultByMessageId(messageId, accountId);
            if (msgDetails == null || string.IsNullOrWhiteSpace(msgDetails.Mobile))
            {
                return new ResponseResult<EmailParseDetails>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }

            var user = await _userRepo.GetUserByUserMobile(msgDetails.Mobile, accountId);
            if (user == null)
            {
                return new ResponseResult<EmailParseDetails>()
                {
                    Message = "No user for mobile number",
                    ResponseCode = ResponseCode.NoUser,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            } 

            var data = new EmailParseDetails()
            {
                UserId = user.UserId,
                RequestIds = msgDetails.RequestId,
                EmailParseDetailId = msgDetails.EmailParserDetailId
            };

            return new ResponseResult<EmailParseDetails>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };

        }
         
        public async Task<ResponseResult<List<EmailParseDetails>>> GetAllEmailParseDetails(long accountId)
        {
            var data = await _emailRepo.GetAllPendingEmailDetails(accountId);
            if (data != null)
            {
                return new ResponseResult<List<EmailParseDetails>>()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = data.Select(x => new EmailParseDetails()
                    {
                        EmailParseDetailId = x.EmailParserDetailId,
                        RequestIds = x.RequestId,
                        MessageId = x.MessageId
                    }).ToList()
                };
            }
            else
            {
                return new ResponseResult<List<EmailParseDetails>>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }
         
        public async Task<ResponseResult<bool>> UpdateEmailParseDetail(long emailParseDetailId, long accountId, long userId, short status, string message)
        {
            var record = await _emailRepo.GetById(accountId, emailParseDetailId);
            if (record == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }

            record.Status = status;
            record.ProcessMessage = message;
            record.UpdatedOn = DateTime.UtcNow;
            record.UpdatedBy = userId;
            _emailRepo.UpdateCallBackResult(record);
            
            if (_unitOfWork.CommitWithStatus() > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = true
                };
            }
            else
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }

        public async Task<ResponseResult<bool>> EmailTravelItineraryCreateReservation(EmailParserReservationDetails model)
        {
            Console.WriteLine("Sample.Customer.Service :: EmailTravelItineraryCreateReservation :: called " + model.MessageId);
            var result = await CreateReservation(model);
            Console.WriteLine("Sample.Customer.Service :: Get Result from Create Reservation :: called " + JsonConvert.SerializeObject(result));
            if (result.emailParseDetailId > 0) {
                short status = (short)(result.saveResult.ResponseCode == ResponseCode.RecordSaved ? 1 : 2);
                await UpdateEmailParseDetail(result.emailParseDetailId, model.accountId, result.userId, status, result.saveResult.Message);
            }
            return result.saveResult;
        }

        private async Task<(ResponseResult<bool> saveResult, long emailParseDetailId, long userId)> CreateReservation(EmailParserReservationDetails model)
        {
            Console.WriteLine("Sample.Customer.Service :: CreateReservation :: called ");
            long emailParseDetailId = 0;
            long userId = 0;
            try
            {
                // Get Email Parse Details based on messageId
                var emailParseDetails = await GetDetailsFromMessageId(model.MessageId, model.accountId);
                if (emailParseDetails.ResponseCode != ResponseCode.RecordFetched)
                {
                    return (new ResponseResult<bool>()
                    {
                        ResponseCode = emailParseDetails.ResponseCode,
                        Message = emailParseDetails.Message,
                        Error = emailParseDetails.Error
                    }, emailParseDetailId, userId);
                }
                Console.WriteLine("Sample.Customer.Service :: GetDetailsFromMessageId :: called "+ model.MessageId+" "+ JsonConvert.SerializeObject(emailParseDetails));
                emailParseDetailId = emailParseDetails.Data.EmailParseDetailId;
                userId = emailParseDetails.Data.UserId;

                // Get Email Parse Details
                var requestIds = emailParseDetails.Data.RequestIds;
                var emailParseResponse = await _awardWalletProvider.GetParseEmailResponse(requestIds);
                if (emailParseResponse.ResponseCode != ResponseCode.RecordFetched)
                {
                    return (new ResponseResult<bool>()
                    {
                        ResponseCode = emailParseResponse.ResponseCode,
                        Message = emailParseResponse.Message,
                        Error = emailParseResponse.Error,
                    }, emailParseDetailId, userId);
                }
                Console.WriteLine("Sample.Customer.Service :: GetParseEmailResponse :: called " + requestIds + " " + JsonConvert.SerializeObject(emailParseResponse));

                var emailParseResult = emailParseResponse.Data;

                if (emailParseResult == null || emailParseResult.itineraries == null || emailParseResult.itineraries.Count < 1 ||
                    emailParseResult.itineraries[0].segments == null || emailParseResult.itineraries[0].segments.Count < 1)
                {
                    return (new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.ValidationFailed,
                        Message = "Itineraries details are missing",
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.ValidationFailed,
                        }
                    }, emailParseDetailId, userId);
                }

                var segments = emailParseResult.itineraries[0].segments.OrderBy(x => x.departure.utcDateTime).ToList();
                var departureAirport = segments.First();
                var returnAirport = segments.Last();

                if (departureAirport.departure.utcDateTime == null)
                {
                    return (new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.ValidationFailed,
                        Message = "Departure date time is missing",
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.ValidationFailed,
                        }
                    }, emailParseDetailId, userId);
                }
                var airports = await _airportAddressService.GetAirportDetails(model.accountId,
                    new string[] { departureAirport.departure.airportCode, departureAirport.arrival.airportCode, returnAirport.arrival.airportCode });
                if (airports == null || airports.ResponseCode != ResponseCode.RecordFetched)
                {
                    return (new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.ValidationFailed,
                        Message = "Airport details are missing",
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.ValidationFailed,
                        }
                    }, emailParseDetailId, userId);
                }

                var departureAirportId = airports.Data.Where(x => x.Code == departureAirport.departure.airportCode).Select(x => x.AirportId).FirstOrDefault();
                var flyingToAirportld = airports.Data.Where(x => x.Code == departureAirport.arrival.airportCode).Select(x => x.AirportId).FirstOrDefault();
                DateTime departureDateTime = Convert.ToDateTime(departureAirport.departure.utcDateTime);

                long returningToAirportld;
                DateTime? returnDateTime = null;

                if (segments == null)
                {
                    returningToAirportld = airports.Data.Where(x => x.Code == returnAirport.arrival.airportCode).Select(x => x.AirportId).FirstOrDefault();
                    returnDateTime = returnAirport.arrival.utcDateTime;
                }
                else
                {
                    returningToAirportld = departureAirportId;
                }

                if (departureAirportId < 1 || returningToAirportld < 1)
                {
                    return (new ResponseResult<bool>()
                    {
                        ResponseCode = ResponseCode.ValidationFailed,
                        Message = "Airport details are missing",
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.ValidationFailed,
                        }
                    }, emailParseDetailId, userId);
                }

                if (returnDateTime == null)
                {
                    // To Do- From Config
                    returnDateTime = departureDateTime.AddDays(7);
                }

                //Console.WriteLine($"departureAirportId={departureAirportId} | flyingToAirportld={flyingToAirportld}");

                var reservationModel = new AddUpdateFlightReservationVM()
                {
                    IsHomeAirport = false,
                    IsBorrowingCarForRent = false,
                    DepaurtureDateTime = departureDateTime,
                    DepaurtureAirportId = departureAirportId,
                    FlyingToAirline = departureAirport.marketingCarrier.airline.name,
                    FlyingToFlightNo = departureAirport.marketingCarrier.flightNumber,
                    FlyingToAirportld = flyingToAirportld,
                    ReturnDateTime = Convert.ToDateTime(returnDateTime),
                    ReturningToAirline = returnAirport.marketingCarrier.airline.name,
                    ReturningToFlightNo = returnAirport.marketingCarrier.flightNumber,
                    ReturningToAirportld = returningToAirportld,
                    AccountId = model.accountId,
                    UserId = userId,
                    FlightReservationStatus = 0                    
                };


                #region [Vatildate Data, Get Price And Address]
                var priceResult = await _reservationService.GetReservationPriceAndAddressData(reservationModel, model.accountId);

                if (priceResult.ResponseCode != ResponseCode.RecordFetched)
                {
                    return (new ResponseResult<bool>()
                    {
                        ResponseCode = priceResult.ResponseCode,
                        Message = priceResult.Message,
                        Error = priceResult.Error
                    }, emailParseDetailId, userId);
                }

                reservationModel.InTimeGap = priceResult.Data.InTimeGap;
                reservationModel.OutTimeGap = priceResult.Data.OutTimeGap;
                reservationModel.TotalFinalAmount = priceResult.Data.ParkingTotalAmount;
                reservationModel.AirportsParkingId = priceResult.Data.AirportsParkingId;
                reservationModel.ParkingProvidersLocationId = priceResult.Data.ParkingProvidersLocationId;
                #endregion

                // Create Reservation
                return (await _reservationService.CreateReservation(reservationModel), emailParseDetailId, userId);
            }
            catch(Exception ex)
            {
                return (new ResponseResult<bool>()
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ex.Message,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                }, emailParseDetailId, userId);
            }
        }
     }
}
