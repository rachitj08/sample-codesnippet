using Common.Enum.StorageEnum;
using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Model.Model.ParkingProvider;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Model.Model.StorageModel;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;
using Sample.Customer.Service.Infrastructure.Repository.ParkingProvider;

namespace Sample.Customer.Service.ServiceWorker.ParkingProvider
{
    public class ParkingProviderService : IParkingProviderService
    {
        private readonly IParkingProviderRepository _parkingProviderRepository;
        private readonly IActivityCodeRepository activityCodeRepository;
        private readonly IParkingProvidersSubLocationsRepository parkingProvidersSubLocationsRepository;
        private readonly IReservationRepository reservationRepository;
        private readonly IUserRepository userRepository;
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IAddressRepository addressRepository;
        private readonly IAirportsParkingRepository airportsParkingRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IStateRepository stateRepository;
        private readonly ICityRepository cityRepository;
        private readonly IUserService userService;
        private readonly IReservationService reservationService;
        private readonly IVehiclesService vehiclesService;
        private readonly IParkingHeadsService parkingHeadsService;
        private readonly IParkingReservationRepository parkingReservationRepository;
        private readonly IParkingSpotsRepository parkingSpotsRepository;
        private readonly IUserVehiclesRepository userVehiclesRepository;
        private readonly IVehicleTagMappingRepository vehicleTagMappingRepository;
        private readonly IReservationVehicleRepository reservationVehicleRepository;
        private readonly IReservationActivityCodeRepository reservationActivityCodeRepository;
        private readonly IReservationPaymentHistoryRepository reservationPaymentHistoryRepository;
        private readonly IFileLogger fileLogger;
        //private readonly ParkingHeadsCustomRateRepository parkingHeadsCustomRateRepository;
        public ParkingProviderService(IParkingProviderRepository parkingProviderRepository, IActivityCodeRepository activityCodeRepository, IParkingProvidersSubLocationsRepository parkingProvidersSubLocationsRepository,
           IReservationRepository reservationRepository, IUserRepository userRepository, IAddressRepository addressRepository, IAirportsParkingRepository airportsParkingRepository,
           ICountryRepository countryRepository, IStateRepository stateRepository, ICityRepository cityRepository, IParkingReservationRepository parkingReservationRepository, IParkingSpotsRepository parkingSpotsRepository, IUserVehiclesRepository userVehiclesRepository,
        IUserService userService, IReservationService reservationService, IVehiclesService vehiclesService, IVehiclesRepository vehiclesRepository, IParkingHeadsService parkingHeadsService, IVehicleTagMappingRepository vehicleTagMappingRepository, IReservationVehicleRepository reservationVehicleRepository, IReservationActivityCodeRepository reservationActivityCodeRepository,
        IReservationPaymentHistoryRepository reservationPaymentHistoryRepository,IFileLogger fileLogger)
        {
            Check.Argument.IsNotNull(nameof(parkingProviderRepository), parkingProviderRepository);
            Check.Argument.IsNotNull(nameof(activityCodeRepository), activityCodeRepository);
            Check.Argument.IsNotNull(nameof(reservationRepository), reservationRepository);
            Check.Argument.IsNotNull(nameof(userRepository), userRepository);
            Check.Argument.IsNotNull(nameof(addressRepository), addressRepository);
            Check.Argument.IsNotNull(nameof(airportsParkingRepository), airportsParkingRepository);
            Check.Argument.IsNotNull(nameof(vehiclesService), vehiclesService);
            Check.Argument.IsNotNull(nameof(userService), userService);
            Check.Argument.IsNotNull(nameof(vehiclesRepository), vehiclesRepository);
            Check.Argument.IsNotNull(nameof(parkingReservationRepository), parkingReservationRepository);
            Check.Argument.IsNotNull(nameof(parkingSpotsRepository), parkingSpotsRepository);
            Check.Argument.IsNotNull(nameof(userVehiclesRepository), userVehiclesRepository);
            Check.Argument.IsNotNull(nameof(vehicleTagMappingRepository), vehicleTagMappingRepository);
            Check.Argument.IsNotNull(nameof(reservationVehicleRepository), reservationVehicleRepository);
            Check.Argument.IsNotNull(nameof(reservationActivityCodeRepository), reservationActivityCodeRepository);
            Check.Argument.IsNotNull(nameof(reservationPaymentHistoryRepository), reservationPaymentHistoryRepository);
            Check.Argument.IsNotNull(nameof(fileLogger), fileLogger);
            //Check.Argument.IsNotNull(nameof(parkingHeadsCustomRateRepository), parkingHeadsCustomRateRepository);
            this.activityCodeRepository = activityCodeRepository;
            _parkingProviderRepository = parkingProviderRepository;
            this.parkingProvidersSubLocationsRepository = parkingProvidersSubLocationsRepository;
            this.reservationRepository = reservationRepository;
            this.addressRepository = addressRepository;
            this.airportsParkingRepository = airportsParkingRepository;
            this.vehiclesService = vehiclesService;
            this.countryRepository = countryRepository;
            this.stateRepository = stateRepository;
            this.cityRepository = cityRepository;
            this.userRepository = userRepository;
            this.reservationService = reservationService;
            this.userService = userService;
            this.vehiclesRepository = vehiclesRepository;
            this.parkingHeadsService = parkingHeadsService;
            this.parkingReservationRepository = parkingReservationRepository;
            this.parkingSpotsRepository = parkingSpotsRepository;
            this.userVehiclesRepository = userVehiclesRepository;
            this.vehicleTagMappingRepository = vehicleTagMappingRepository;
            this.reservationVehicleRepository = reservationVehicleRepository;
            this.reservationActivityCodeRepository = reservationActivityCodeRepository;
            this.reservationPaymentHistoryRepository = reservationPaymentHistoryRepository;
            this.fileLogger = fileLogger;
            //this.parkingHeadsCustomRateRepository = parkingHeadsCustomRateRepository;
        }
        #region Manage QR Code


        public ResponseResult<IEnumerable<DropDownMaster>> GetActivityCode()
        {
            var data = _parkingProviderRepository.GetActivityCode();
            if (data == null)
                return new ResponseResult<IEnumerable<DropDownMaster>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<DropDownMaster>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };

        }

        public ResponseResult<IEnumerable<DropDownMaster>> GetParkingProvider()
        {
            var data = _parkingProviderRepository.GetParkingProvider();
            if (data == null)
                return new ResponseResult<IEnumerable<DropDownMaster>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<DropDownMaster>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<IEnumerable<DropDownMaster>> GetParkingProviderLocation(long providerId)
        {
            var data = _parkingProviderRepository.GetParkingProviderLocation(providerId);
            if (data == null)
                return new ResponseResult<IEnumerable<DropDownMaster>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<DropDownMaster>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<IEnumerable<DropDownMaster>> GetParkingSpotId(long subLocationId)
        {
            IEnumerable<DropDownMaster> data = _parkingProviderRepository.GetParkingSpotId(subLocationId);
            if (data == null)
                return new ResponseResult<IEnumerable<DropDownMaster>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<DropDownMaster>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<IEnumerable<Common.Model.SubLocationType>> GetSubLocationType()
        {
            var data = _parkingProviderRepository.GetSubLocationType();
            if (data == null)
                return new ResponseResult<IEnumerable<Common.Model.SubLocationType>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<Common.Model.SubLocationType>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<IEnumerable<QRListVM>> GetQRList(long parkingProviderId)
        {
            var data = _parkingProviderRepository.GetQRList(parkingProviderId);
            if (data == null)
                return new ResponseResult<IEnumerable<QRListVM>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<QRListVM>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<IEnumerable<DropDownMaster>> GetSubLocation(long parkingProviderlocationId)
        {
            var data = _parkingProviderRepository.GetSubLocation(parkingProviderlocationId);
            if (data == null)
                return new ResponseResult<IEnumerable<DropDownMaster>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<DropDownMaster>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<bool> UploadandSaveQR(QRUploadVM model, long loggedInUserId, long loggedInAccountId)
        {
            ImageResponseView imageResponseView = new ImageResponseView();
            MediaResponseView response = new MediaResponseView();
            string uploadedFileName = string.Empty;
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = "Invalid request",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(model.ProfileDocument.FileName))
            {
                errorDetails.Add("fileName", new string[] { "This field may not be blank." });
            }

            if (model.ProfileDocument.Filebytes == null || model.ProfileDocument.Filebytes.Length < 1)
            {
                errorDetails.Add("fileByte", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        Detail = errorDetails
                    }
                };
            }

            //var user = await this.usersRepository.GetUserByUserId(accountId, loggedInUserId);
            //if (user == null)
            //{
            //    return new ResponseResult<bool>()
            //    {
            //        Message = ResponseMessage.ValidationFailed,
            //        ResponseCode = ResponseCode.ValidationFailed,
            //        Error = new ErrorResponseResult()
            //        {
            //            Message = ResponseMessage.ValidationFailed,
            //        }
            //    };
            //}

            // Upload Image
            if (model != null && loggedInUserId != 0)
            {
                if (model.ProfileDocument.Filebytes.Length > 0)
                {
                    var mediaUploadView = new MediaUploadView();
                    mediaUploadView.Documents = new List<Document>();
                    mediaUploadView.ReferenceId = Convert.ToInt64(model.ProviderId);
                    mediaUploadView.ReferenceType = (int)MediaReferenceEnum.QRGenerator;
                    mediaUploadView.CreatedBy = Convert.ToInt64(model.ProviderLocationId);
                    model.ProfileDocument.FileGuid = Guid.NewGuid();
                    model.ProfileDocument.FileName = model.ActivityCode;
                    model.ProfileDocument.FileExtenstion = "*.png";
                    model.ProfileDocument.FileUniqueName = "QRCode";
                    mediaUploadView.Documents.Add(model.ProfileDocument);
                    imageResponseView = null;
                    if (imageResponseView != null)
                    {
                        response.Data = imageResponseView.SavedPathList;
                        response.ImageHeight = imageResponseView.ImageHeight;
                        response.ImageWidth = imageResponseView.ImageWidth;

                        response.Message = "Success";
                        response.Type = "Success";
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "Failed";
                        response.Type = "Failed";
                    }
                    if (response.Type == "Success" && response.Data != null && response.Data.Count > 0)
                    {
                        uploadedFileName = Convert.ToString(response.Data[0]);
                    }

                }

            }

            // Validate Model
            if (string.IsNullOrWhiteSpace(uploadedFileName))
            {
                return new ResponseResult<bool>()
                {
                    Message = "Could not able to save file",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        Detail = errorDetails
                    }
                };
            }
            var activityCode = activityCodeRepository.GetActivityIdByCode(model.ActivityCode);
            var data = parkingProvidersSubLocationsRepository.CheckIfQRCodeExist(model.SubLocationType, activityCode.ActivityCodeId, Convert.ToInt64(model.ProviderLocationId));
            //string mappingCode = model.ActivityCode + " " + activityCodeId;
            string mappingCode = "0" + model.ProviderId + " 00" + model.ProviderLocationId + " " + model.SubLocationType + " 000" + model.SubLocationId;
            int val = 0;
            if (data != null)
                val = parkingProvidersSubLocationsRepository.UpdateQRData(model, uploadedFileName, activityCode.ActivityCodeId, mappingCode);
            else
                val = parkingProvidersSubLocationsRepository.SaveQRData(model, uploadedFileName, activityCode.ActivityCodeId, mappingCode, activityCode.Name);
            if (val <= 0)
                return new ResponseResult<bool>()
                {
                    Message = "Unable to save qr code.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        Detail = errorDetails
                    }
                };
            return new ResponseResult<bool>()
            {
                Message = uploadedFileName,
                ResponseCode = ResponseCode.RecordSaved,

            };


        }
        #endregion

        public async Task<ResponseResult<UpsertProviderReservationVM>> GetProviderReservation(long reservationId, long userId, long accountId)
        {

            try
            {
                var reservation = this.reservationRepository.GetReservationById(reservationId, accountId);
                if (reservation == null)
                {
                    return new ResponseResult<UpsertProviderReservationVM>()
                    {
                        ResponseCode = ResponseCode.NoRecordFound,
                        Message = ResponseMessage.NoRecordFound
                    };
                }
                var model = await GetParkingReservation(reservation, userId, accountId);
                return new ResponseResult<UpsertProviderReservationVM>()
                {
                    ResponseCode = ResponseCode.RecordSaved,
                    Message = ResponseMessage.RecordSaved,
                    Data = model
                };
                //from f in flightReservationQuery
                //join da in airportQuery on f.DepaurtureAirportId equals da.AirportId
                //join ra in airportQuery on f.ReturningToAirportld equals ra.AirportId
                //join pr in parkingLocationQuery on f.ReservationId equals pr.ReservationId
                //join ad in addressQuery on pr.ParkingProvidersLocation.AddressId equals ad.AddressId
            }
            catch (Exception ex)
            {

                return new ResponseResult<UpsertProviderReservationVM>()
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ResponseMessage.InternalServerError,
                    Data = null,
                    Error = new ErrorResponseResult()
                    {
                        Message = ex.Message

                    }
                };
            }
        }

        private async Task<UpsertProviderReservationVM> GetParkingReservation(Reservation reservation, long userId, long accountId)
        {
            

            var user = await this.userRepository.GetUserByUserId(accountId, reservation.UserId);
            UpsertVehicleVM upsertVehicleVM = null;
            var parkingReservation = reservation.ParkingReservation.FirstOrDefault();
            var reservationVehicle = reservation.ReservationVehicle.FirstOrDefault();
            if (reservationVehicle != null && reservationVehicle.VehicleId > 0)
            {
                var vehicle = this.vehiclesRepository.GetVehicleDetailsById(reservationVehicle.VehicleId, accountId);

                if (vehicle != null)
                {
                    upsertVehicleVM = new UpsertVehicleVM()
                    {
                        CarColor = vehicle.Color,
                        LicensePlateNumber = vehicle.LicensePlate,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        VehicleId = vehicle.VehicleId,
                        Ticket = vehicle.Ticket,
                        VehicleStateId = vehicle.VehicleStateId ?? 0
                    };
                }
            }
            UpsertReservationVM upsertReservationVM = new UpsertReservationVM();
            upsertReservationVM.ReservationId = reservation.ReservationId;
            upsertReservationVM.IsCancel = reservation.IsCancelled ?? false;
            if (parkingReservation != null)
            {
                upsertReservationVM.BookingConfirmationNo = parkingReservation.BookingConfirmationNo;
                upsertReservationVM.SourceId = parkingReservation.SourceId ?? 0;
                upsertReservationVM.Comment = parkingReservation.Comment;
                upsertReservationVM.ProviderLocationId = parkingReservation.ParkingProvidersLocationId ?? 0;
                upsertReservationVM.DepaurtureDateTime = parkingReservation.EndDateTime;
                upsertReservationVM.ReturnDateTime = parkingReservation.StartDateTime;
                upsertReservationVM.CheckInDateTime = parkingReservation.CheckInDateTime;
                upsertReservationVM.CheckOutDateTime = parkingReservation.CheckOutDateTime;

                upsertReservationVM.VehicleLocation = parkingReservation.VehicleLocation;
                upsertReservationVM.VehicleKeyLocation = parkingReservation.VehicleKeyLocation;
                upsertReservationVM.VehicleLocation = parkingReservation.VehicleLocation;

            }
            UpsertUserVM upsertUserVM = new UpsertUserVM()
            {
                Email = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MobileCode = user.MobileCode,
                MobileNumber = user.Mobile,
                UserId = user.UserId,
            };
            if (user.AddressId != null && user.AddressId > 0)
            {
                var address = addressRepository.GetById(user.AddressId ?? 0);
                if (address != null)
                {
                    var country = countryRepository.GetByCode(address.Country);
                    if (country != null)
                    {
                        var state = stateRepository.GetStateByName(address.State, country.Id);
                        if (state != null)
                        {
                            upsertUserVM.State = state.Id;
                            upsertUserVM.StateName = state.Name;
                            var city = cityRepository.GetCityByName(address.City, state.Id);
                            if (city != null)
                            {
                                upsertUserVM.City = city.Id;
                                upsertUserVM.CityName = city.Name;
                            }
                        }
                        upsertUserVM.Country = country.Id;
                        upsertUserVM.CountryName = country.Name;
                    }
                    upsertUserVM.Address = address.Streat1 ?? "" + " " + address.Streat2 ?? "";
                    upsertUserVM.PostalCode = address.Zip;
                }
            }
            var source = _parkingProviderRepository.GetSource();
            var reservationPaymentHistory = await reservationPaymentHistoryRepository.GetAllReservationPaymentHistory(upsertReservationVM.ReservationId);
            var reservationPaymentHistoryVM=reservationPaymentHistory.Select(x=> new ReservationPaymentHistoryVM()
            {
                ReservationId= x.ReservationId,
                BaseRate= x.BaseRate,
                IsCustomRate= x.IsCustomRate,
                IsSystemRate= x.IsSystemRate,
                PaymentMode= x.PaymentMode,
                PaymentOn= x.PaymentOn,
                PaymentType= x.PaymentType,
                ReservationPaymentHistoryId= x.ReservationPaymentHistoryId,
                SourceName=source.Where(m=> m.Id==x.SourceId.ToString()).Select(m=>m.Name).FirstOrDefault(),   
                Tax=x.Tax,
                TotalAmout=x.TotalAmout,
            }).ToList();
            return new UpsertProviderReservationVM()
            {
                UpsertReservationVM = upsertReservationVM,
                UpsertVehicleVM = upsertVehicleVM,
                UpsertUserVM = upsertUserVM,
                ReservationPaymentHistoryVM= reservationPaymentHistoryVM
            };
        }
        public async Task<ResponseResult<ParkingReserAmountAndLocationVM>> CreateReservation(UpsertProviderReservationVM model, long userId, long accountId)
        {

            //User Created
            long upsertUserId = await UpsertIsertUser(model.UpsertUserVM, accountId, userId);
            if (upsertUserId == 0)
            {
                return new ResponseResult<ParkingReserAmountAndLocationVM>()
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = "Error while creating user.",
                    Data = null,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Unable to create user for reservation."

                    }
                };
            }
            var airportParking = await airportsParkingRepository.GetAirportsParkingByLocationId(model.UpsertReservationVM.ProviderLocationId, accountId);

            #region [Create Reservation]
            AddUpdateFlightReservationVM vm = new AddUpdateFlightReservationVM()
            {
                BookingConfirmationNo = model.UpsertReservationVM.BookingConfirmationNo,
                Comment = model.UpsertReservationVM.Comment,
                CustomUserId = upsertUserId,
                DepaurtureAirportId = airportParking.AirportId,
                DepaurtureDateTime = model.UpsertReservationVM.DepaurtureDateTime,
                ReturnDateTime = model.UpsertReservationVM.ReturnDateTime,
                SourceId = model.UpsertReservationVM.SourceId ?? 0,
                AccountId = accountId,
                ComingFrom = model.UpsertReservationVM.CommingFrom == "" ? "" : model.UpsertReservationVM.CommingFrom.ToLower()
            };
            vm.UserId = upsertUserId;
            var createdReservation = await reservationService.CreateReservationNew(vm, accountId);
            #endregion
            //Vehicle 
            if (!string.IsNullOrEmpty(model.UpsertVehicleVM.LicensePlateNumber))
            {
                Vehicles vehicle = new Vehicles()
                {
                    Make = model.UpsertVehicleVM.Make ?? "",
                    Model = model.UpsertVehicleVM.Model ?? "",
                    Year = "",
                    LicensePlate = model.UpsertVehicleVM.LicensePlateNumber,
                    NumberOfDoors = 2,
                    IsTransmissionAutomatic = false,
                    NumberOfBags = 2,
                    NumberOfPassenger = 2,
                    Vinnumber = "provider",
                    RegistrationState = "",
                    Logo = "",
                    Color = model.UpsertVehicleVM.CarColor,
                    IsConvertable = false,
                    Ticket = model.UpsertVehicleVM.Ticket,
                    VehicleStateId = model.UpsertVehicleVM.VehicleStateId,
                    AccountId = accountId,
                    LoggedInUserId = userId,
                    VehicleCategoryId = 13,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };
                await vehiclesRepository.CreateVehicleDetails(vehicle);


                VehicleTagMapping vehicleTagMapping = new VehicleTagMapping()
                {
                    TagId = "123",
                    VehicleId = vehicle.VehicleId,
                    AccountId = accountId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };
                await vehicleTagMappingRepository.CreateVehicleTagMappingDetailsNew(vehicleTagMapping);


                // save user vehicle

                UserVehicles userVehicles = new UserVehicles()
                {
                    VehicleId = vehicle.VehicleId,
                    UserId = upsertUserId,
                    LoggedInUserId = userId,
                    AccountId = accountId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };

                await userVehiclesRepository.CreateUserVehicleDetailsNew(userVehicles);
                if (createdReservation.Data != null && createdReservation.Data.ReservationId > 0 && vehicle.VehicleId > 0)
                {
                    var reservationVehicle = new ReservationVehicle()
                    {
                        ParkingProvidersLocationId = model.UpsertReservationVM.ProviderLocationId,
                        ReservationId = createdReservation.Data.ReservationId,
                        VehicleId = vehicle.VehicleId,
                        IsConsented = false,
                        AccountId = accountId,
                        CreatedBy = userId,
                        CreatedOn = DateTime.UtcNow
                    };

                    await reservationVehicleRepository.CreateReservationVehicleDetailsNew(reservationVehicle);
                   
                }
              
            }

            if (createdReservation != null && createdReservation.Data != null)
            {
                var rate=parkingHeadsService.GetParkingPriceDetail(model.UpsertReservationVM.ProviderLocationId, model.UpsertReservationVM.DepaurtureDateTime.ToUniversalTime(), model.UpsertReservationVM.ReturnDateTime.ToUniversalTime(), accountId,false,0);
                var days = (model.UpsertReservationVM.ReturnDateTime.ToUniversalTime() - model.UpsertReservationVM.DepaurtureDateTime.ToUniversalTime()).TotalDays;

                var tax = rate.Data.Where(x => x.ParkingHeadId == (int)ParkingHeadRateEnum.GovernmentRate).Select(x => x.Amount).Sum();
                if (model.UpsertReservationVM.CustomRateType == "flat")
                {
                    tax = (model.UpsertReservationVM.CustomRate * 7) / 100;


                }
                ReservationPaymentHistory reservationPaymentHistory = new ReservationPaymentHistory()
                {
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    IsCustomRate = model.UpsertReservationVM.IsCustomRate,
                    PaymentMode = model.UpsertReservationVM.PaymentMode,
                    PaymentOn = DateTime.UtcNow,
                    IsSystemRate = true,
                    PaymentType = "Prepaid",
                    ReservationId = createdReservation.Data.ReservationId,
                    SourceId = model.UpsertReservationVM.SourceId,
                };
                if (model.UpsertReservationVM.CustomRateType == "flat")
                {
                    reservationPaymentHistory.Tax = (model.UpsertReservationVM.CustomRate * 7) / 100;
                    reservationPaymentHistory.BaseRate = Convert.ToDecimal(Convert.ToDouble((model.UpsertReservationVM.CustomRate - reservationPaymentHistory.Tax)) / days);
                    reservationPaymentHistory.TotalAmout = reservationPaymentHistory.BaseRate + reservationPaymentHistory.Tax;

                }
                else
                {
                    reservationPaymentHistory.Tax = (model.UpsertReservationVM.CustomRate * 7) / 100;
                    reservationPaymentHistory.BaseRate = Convert.ToDecimal(Convert.ToDouble((model.UpsertReservationVM.CustomRate)) / days);
                    reservationPaymentHistory.TotalAmout = reservationPaymentHistory.BaseRate + reservationPaymentHistory.Tax;

                }

                try
                {
                    await reservationPaymentHistoryRepository.CreateReservationPaymentHistory(reservationPaymentHistory,accountId);
                    if (model.UpsertReservationVM.IsCustomRate)
                    {
                        //ParkingHeadsCustomRate parkingHeadsCustomRate = new ParkingHeadsCustomRate()
                        //{
                        //    AccountId = accountId,
                        //    CreatedBy = userId,
                        //    CreatedOn = DateTime.UtcNow,
                        //    ParkingProviderLocationId = model.UpsertReservationVM.ProviderLocationId,
                        //    ParkingHeadId = (int)ParkingHeadRateEnum.CustomRateType,
                        //    Rate = model.UpsertReservationVM.CustomRate,
                        //    ReservationId = createdReservation.Data.ReservationId,
                        //    EndDate = DateTime.UtcNow.AddDays(30),
                        //    FromDate = DateTime.UtcNow.AddDays(-30),
                        //    IsActive= true,
                        //};
                        //await parkingHeadsCustomRateRepository.CreateAndSaveAsync(parkingHeadsCustomRate);

                    }
                    //CreateInvoiceVM createInvoiceVM = new CreateInvoiceVM()
                    //{
                    //    AccountId = accountId,
                    //    ReservationId = createdReservation.Data.ReservationId,
                    //    UserId = userId,
                    //    IsCheckOut = false
                    //};
                    //await reservationService.GenerateReservationInvoiceById(createInvoiceVM);
                }
                catch (Exception ex)
                {
                    
                }
            }
            return createdReservation;


        }

        public async Task<ResponseResult<ParkingReservationAmountAndLocationVM>> UpdateReservation(UpsertProviderReservationVM model, long userId, long accountId)
        {
            if (model.UpsertReservationVM.ReservationId == 0)
            {
                return new ResponseResult<ParkingReservationAmountAndLocationVM>()
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = "Reservation id not found.",
                    Data = null,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Unable to create user for reservation."

                    }
                };
            }
            //User Created
            long upsertUserId = await UpsertIsertUser(model.UpsertUserVM, accountId, userId);
            if (upsertUserId == 0)
            {
                return new ResponseResult<ParkingReservationAmountAndLocationVM>()
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = "Error while creating user.",
                    Data = null,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Unable to create user for reservation."

                    }
                };
            }
            var airportParking = await airportsParkingRepository.GetAirportsParkingByLocationId(model.UpsertReservationVM.ProviderLocationId, accountId);

            #region [Update Reservation]
            AddUpdateFlightAndParkingReservationVM vm = new AddUpdateFlightAndParkingReservationVM()
            {
                BookingConfirmationNo = model.UpsertReservationVM.BookingConfirmationNo,
                Comment = model.UpsertReservationVM.Comment,
                CustomUserId = upsertUserId,
                DepaurtureAirportId = airportParking.AirportId,
                DepaurtureDateTime = model.UpsertReservationVM.DepaurtureDateTime,
                ReturnDateTime = model.UpsertReservationVM.ReturnDateTime,
                SourceId = model.UpsertReservationVM.SourceId ?? 0,
                
            };

            var updatedReservation = await reservationService.UpdateReservationFromProviderPortal(vm, model.UpsertReservationVM.ReservationId, accountId, userId);
            #endregion
            //Vehicle 
            if (!string.IsNullOrEmpty(model.UpsertVehicleVM.LicensePlateNumber))
            {
                if (model.UpsertVehicleVM.VehicleId > 0)
                {
                    var vehicle = vehiclesRepository.GetAllVehicles(accountId).Where(x => x.VehicleId == model.UpsertVehicleVM.VehicleId).FirstOrDefault();
                    vehicle.UpdatedOn = DateTime.Now;
                    vehicle.UpdatedBy = userId;
                    vehicle.AccountId = accountId;
                    vehicle.Color = model.UpsertVehicleVM.CarColor;
                    vehicle.Make = model.UpsertVehicleVM.Make;
                    vehicle.Model = model.UpsertVehicleVM.Model;
                    vehicle.LicensePlate = model.UpsertVehicleVM.LicensePlateNumber;
                    vehicle.VehicleStateId = model.UpsertVehicleVM.VehicleStateId;
                    vehicle.Ticket = model.UpsertVehicleVM.Ticket;
                    vehiclesRepository.UpdateVehicleDetails(vehicle);
                }
                else
                {
                    CreateVehicleReqVM createVehicleReqVM = new CreateVehicleReqVM()
                    {
                        CarColor = model.UpsertVehicleVM.CarColor,
                        Make = model.UpsertVehicleVM.Make,
                        Model = model.UpsertVehicleVM.Model,
                        LicensePlateNumber = model.UpsertVehicleVM.LicensePlateNumber,
                        ModelYear = "",
                        TabId = "1234",
                        ReservationId = updatedReservation.Data.ReservationId,
                        VinNumber = "provider",
                        VehicleStateId = model.UpsertVehicleVM.VehicleStateId ?? 0,
                        Ticket = model.UpsertVehicleVM.Ticket,

                    };
                    await vehiclesService.CreateVehicle(createVehicleReqVM, accountId, userId);
                }
                //vehiclesService.CreateVehicle(createVehicleReqVM, accountId, userId);
            }


            return updatedReservation;


        }

        private async Task<long> UpsertIsertUser(UpsertUserVM model, long accountId, long userId)
        {

            Users user = null;
            long upsertUserId = 0;
            if (model.UserId == 0)
            {
                user = await userRepository.GetUserByEmailId(model.Email, accountId);
                if (user == null)
                    user = await userRepository.GetUserByUserMobile(model.MobileNumber, accountId);
            }
            else
                user = await userRepository.GetUserByUserId(accountId, model.UserId);

            Address address = new Address()
            {
                AccountId = accountId,
                Country = model.CountryName,
                State = model.StateName,
                City = model.CityName,
                Streat1 = model.Address,
                Zip = model.PostalCode,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = userId,
            };
            if (user == null)
            {


                var savedAddress = await addressRepository.CreateAddressNew(address);
                CreateUserModel createUserModel = new CreateUserModel()
                {
                    EmailAddress = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Mobile = model.MobileNumber,
                    MobileCode = model.MobileCode,
                    UserName = model.Email,
                    UserStatus = 1,
                    Password = " ",
                    AddressId = savedAddress.AddressId,

                };
                var userVm = await userService.CreateUser(createUserModel, accountId, userId, string.Empty);
                if (userVm != null && userVm.Data != null && userVm.Data.UserId > 0)
                    upsertUserId = userVm.Data.UserId;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.FirstName))
                    user.FirstName = model.FirstName;
                if (!string.IsNullOrEmpty(model.LastName))
                    user.LastName = model.LastName;
                if (!string.IsNullOrEmpty(model.Email))
                    user.EmailAddress = model.Email;
                if (!string.IsNullOrEmpty(model.MobileCode))
                    user.MobileCode = model.MobileCode;
                if (!string.IsNullOrEmpty(model.MobileNumber))
                    user.Mobile = model.MobileNumber;
                var addressNew = addressRepository.GetAddressQuery(accountId).Where(x => x.AddressId == user.AddressId).FirstOrDefault();
                if (addressNew == null)
                {
                    var savedAddress = await addressRepository.CreateAddressNew(address);
                    user.AddressId = savedAddress.AddressId;
                }
                else
                {
                    addressNew.Country = model.CountryName;
                    addressNew.State = model.StateName;
                    addressNew.City = model.CityName;
                    addressNew.Streat1 = model.Address;
                    addressNew.Zip = model.PostalCode;
                    address.UpdatedBy = userId;
                    address.UpdatedOn = DateTime.UtcNow;
                    await addressRepository.UpdateAddressNew(addressNew);
                }

                var updatedUser = await userRepository.UpdateUserNew(user);
                upsertUserId = updatedUser.UserId;
            }
            return upsertUserId;
        }

        public async Task<ResponseResult<List<UpsertProviderReservationVM>>> GetAllProviderReservation(long userId, long accountId)
        {
            List<UpsertProviderReservationVM> model = new List<UpsertProviderReservationVM>();
            var reservationList = reservationRepository.GetAllReservationListByUserId(userId);
            foreach (var reservation in reservationList)
            {
                UpsertProviderReservationVM upsertProviderReservationVM = await GetParkingReservation(reservation, userId, accountId);
                model.Add(upsertProviderReservationVM);
            }
            return new ResponseResult<List<UpsertProviderReservationVM>>()
            {
                Data = model,
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseMessage.RecordFetched
            };
        }


        public ResponseResult<IEnumerable<CountryVM>> GetAllCountry()
        {
            var countryList = countryRepository.GetCountry();
            var countryVm = countryList.Select(x => new CountryVM()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return new ResponseResult<IEnumerable<CountryVM>>()
            {
                Data = countryVm,
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseMessage.RecordFetched
            };
        }
        public async Task<ResponseResult<IEnumerable<StateVM>>> GetStateByCountryId(long countryId)
        {
            var stateList = await stateRepository.GetState(countryId);
            var stateVm = stateList.Select(x => new StateVM()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return new ResponseResult<IEnumerable<StateVM>>()
            {
                Data = stateVm,
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseMessage.RecordFetched
            };
        }
        public async Task<ResponseResult<IEnumerable<CityVM>>> GetCityByStateId(long stateId)
        {
            var cityList = await cityRepository.GetCity(stateId);
            var cityVm = cityList.Select(x => new CityVM()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return new ResponseResult<IEnumerable<CityVM>>()
            {
                Data = cityVm,
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseMessage.RecordFetched
            };
        }

        public ResponseResult<IEnumerable<DropDownMaster>> GetSource()
        {
            var data = _parkingProviderRepository.GetSource();
            if (data == null)
                return new ResponseResult<IEnumerable<DropDownMaster>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<DropDownMaster>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };

        }

        public ResponseResult<List<InvoicePriceDetailVM>> GetParkingPriceDetail(ParkingRateReqVM model, long accountId)
        {
            return parkingHeadsService.GetParkingPriceDetail(model.ParkingProviderLocationId, model.StartDateTime.ToUniversalTime(), model.EndDateTime.ToUniversalTime(), accountId,model.IsCustomRate,model.ReservationId);

        }


        public ResponseResult<IEnumerable<DropDownMaster>> GetParkingSpotByLocationandSpotType(long providerLocationId, long spotType, long accountId)
        {
            IEnumerable<DropDownMaster> data = _parkingProviderRepository.GetParkingSpotByLocationandSpot(providerLocationId, spotType, accountId);
            if (data == null)
                return new ResponseResult<IEnumerable<DropDownMaster>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<DropDownMaster>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }
        public ResponseResult<IEnumerable<DropDownMaster>> GetParkingSpotType(long accountId)
        {
            var data = _parkingProviderRepository.GetParkingSpotType(accountId);
            if (data == null)
                return new ResponseResult<IEnumerable<DropDownMaster>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            return new ResponseResult<IEnumerable<DropDownMaster>>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public async Task<ResponseResult<bool>> CheckInVehicle(CheckInCheckOutVM model, long userId, long accountId)
        {
            var reservation = reservationRepository.GetReservationById(model.ReservationId, accountId);
            var parkingReservation = reservation.ParkingReservation.FirstOrDefault();
            if (parkingReservation != null)
            {
                
                var parkingSpot = await parkingSpotsRepository.GetParkingSpotById(model.SpotId, accountId);
                if (parkingSpot != null)
                {
                    parkingSpot.UpdatedOn = DateTime.UtcNow;
                    parkingSpot.UpdatedBy = userId;
                    parkingSpot.IsOccupied = true;
                    parkingReservation.ValletLocation = parkingSpot.Name;

                    parkingReservation.UpdatedBy = userId;
                    parkingReservation.UpdatedOn = DateTime.UtcNow;
                    parkingReservation.CheckInDateTime = model.CheckInDateTime;

                   
                    await parkingReservationRepository.UpdateParkingReservationNew(parkingReservation);
                    await parkingSpotsRepository.UpdateParkingSpot(parkingSpot);
                    await UpdateActivityForCheckInCheckOut(accountId, userId, model.ReservationId, model.ProviderLocationId, false);
                    return new ResponseResult<bool>()
                    {
                        Message = ResponseMessage.RecordSaved,
                        ResponseCode = ResponseCode.RecordSaved,
                        Data = true
                    };
                }
            }

            return new ResponseResult<bool>()
            {
                Message = ResponseMessage.NoRecordFound,
                ResponseCode = ResponseCode.NoRecordFound,
                Error = new ErrorResponseResult()
                {
                    Message = "No parking reservation found."
                }
            };
        }

        public async Task<ResponseResult<bool>> CheckOutVehicle(CheckInCheckOutVM model, long userId, long accountId)
        {
            var reservation = reservationRepository.GetReservationById(model.ReservationId, accountId);
            var parkingReservation = reservation.ParkingReservation.FirstOrDefault();
            if (parkingReservation != null)
            {
                var parkingSpot = await parkingSpotsRepository.GetParkingSpotByName(parkingReservation.ValletLocation, accountId);
                //var parkingSpot = await parkingSpotsRepository.GetParkingSpotById(model.SpotId, accountId);
                if (parkingSpot != null)
                {
                    parkingReservation.UpdatedBy = userId;
                    parkingReservation.UpdatedOn = DateTime.UtcNow;
                    parkingReservation.CheckOutDateTime = model.CheckOutDateTime;
                    parkingReservation.ValletLocation = parkingSpot.Name;
                    await parkingReservationRepository.UpdateParkingReservationNew(parkingReservation);

                    parkingSpot.UpdatedOn = DateTime.UtcNow;
                    parkingSpot.UpdatedBy = userId;
                    parkingSpot.IsOccupied = false;
                    await parkingSpotsRepository.UpdateParkingSpot(parkingSpot);
                    await UpdateActivityForCheckInCheckOut(accountId, userId, model.ReservationId, model.ProviderLocationId, false);
                   
                    return new ResponseResult<bool>()
                    {
                        Message = ResponseMessage.RecordSaved,
                        ResponseCode = ResponseCode.RecordSaved,
                        Error = new ErrorResponseResult()
                        {
                            Message = ""
                        }
                    };
                   
                    //try
                    //{
                    //    return await reservationService.GenerateReservationInvoiceById(createInvoiceVM);
                    //}
                    //catch (Exception ex)
                    //{

                    //    return new ResponseResult<bool>()
                    //    {
                    //        Message = ResponseMessage.NoRecordFound,
                    //        ResponseCode = ResponseCode.NoRecordFound,
                    //        Error = new ErrorResponseResult()
                    //        {
                    //            Message = ex.ToString()
                    //        }
                    //    };
                    //}
                }
            }

            return new ResponseResult<bool>()
            {
                Message = ResponseMessage.NoRecordFound,
                ResponseCode = ResponseCode.NoRecordFound,
                Error = new ErrorResponseResult()
                {
                    Message = "No parking reservation found."
                }
            };
        }

        public async Task<ReservationActivityCode> UpdateActivityForCheckInCheckOut(long accountId, long userId, long reservationId, long providerLocation, bool isCheckOut)
        {
            string code = isCheckOut ? "BSP" : "PV";
            var activityCode = activityCodeRepository.GetActivityIdByCode(code);
            var sublocation = parkingProvidersSubLocationsRepository.GetSubLocationByParkingProvide(providerLocation).Where(x => x.ActivityCodeId == activityCode.ActivityCodeId).FirstOrDefault();
            ReservationActivityCode reservationActivityCode = new ReservationActivityCode();
            reservationActivityCode.AccountId = accountId;
            reservationActivityCode.ActivityCodeId = activityCode.ActivityCodeId;
            reservationActivityCode.ReservationId = reservationId;
            reservationActivityCode.ActivityDoneBy = "C";
            
            if (isCheckOut)
            {
                reservationActivityCode.ParkingProvidersLocationSubLocationId = sublocation == null? 13 : sublocation.ParkingProvidersLocationSubLocationId;
                
            }
            else
            {
                reservationActivityCode.ParkingProvidersLocationSubLocationId = sublocation == null? 5: reservationActivityCode.ParkingProvidersLocationSubLocationId;
            }
            reservationActivityCode.CreatedOn = DateTime.UtcNow;
            //PV VRVLS 17
            reservationActivityCode.CreatedBy = userId;
            reservationActivityCode.UpdatedOn = DateTime.UtcNow;
            reservationActivityCode.UpdatedBy = userId;

           return await this.reservationActivityCodeRepository.CreateReservationActivtyCodeAndSave(reservationActivityCode);

        }

    }
}
