using AutoMapper;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Model.VinDecoder;
using Sample.Customer.Service.Helpers;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class VehiclesService : IVehiclesService
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
        private readonly IParkingProvidersLocationsRepository _parkingProvidersLocationsRepository;
        private readonly IUserVehiclesRepository _userVehiclesRepository;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly IMapper mapper;
        private readonly IVehicleTagMappingRepository _vehicleTagMappingRepository;
        private readonly IReservationVehicleRepository _reservationVehicleRepository;
        private readonly IVehicleInfoService _vehicleInfoService;
        private readonly IVehicleFeaturesRepository _vehicleFeaturesRepository;
        private readonly IVehicleFeaturesMappingRepository _vehicleFeaturesMappingRepository;
        private readonly IVehicleCategoryRepository _vehicleCategoryRepository;

        public VehiclesService(IUnitOfWork unitOfWork, IReservationRepository reservationRepository,
           IAirportsParkingService airportsParkingService, IAirportsRepository airportsRepo,
           IParkingHeadsService parkingHeadsService, IFlightReservationRepository flightReservationRepository,
           IReservationActivityCodeRepository reservationActivityCodeRepository, IMapper mapper,
           IParkingReservationRepository parkingReservationRepo, IAddressRepository addressRepo,
           IActivityCodeRepository activityCodeRepository,
           IParkingProvidersLocationsRepository parkingProvidersLocationsRepository,
           IUserVehiclesRepository userVehiclesRepository,
           IVehiclesRepository vehiclesRepository,
           IVehicleTagMappingRepository vehicleTagMappingRepository,
           IReservationVehicleRepository reservationVehicleRepository,
           IVehicleInfoService vehicleInfoService,
           IVehicleFeaturesRepository vehicleFeaturesRepository,
           IVehicleFeaturesMappingRepository vehicleFeaturesMappingRepository,
           IVehicleCategoryRepository vehicleCategoryRepository)
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
            Check.Argument.IsNotNull(nameof(parkingProvidersLocationsRepository), parkingProvidersLocationsRepository);
            Check.Argument.IsNotNull(nameof(parkingProvidersLocationsRepository), parkingProvidersLocationsRepository);
            Check.Argument.IsNotNull(nameof(userVehiclesRepository), userVehiclesRepository);
            Check.Argument.IsNotNull(nameof(vehiclesRepository), vehiclesRepository);
            Check.Argument.IsNotNull(nameof(vehicleTagMappingRepository), vehicleTagMappingRepository);
            Check.Argument.IsNotNull(nameof(reservationVehicleRepository), reservationVehicleRepository);
            Check.Argument.IsNotNull(nameof(vehicleInfoService), vehicleInfoService);
            Check.Argument.IsNotNull(nameof(vehicleFeaturesRepository), vehicleFeaturesRepository);
            Check.Argument.IsNotNull(nameof(vehicleFeaturesMappingRepository), vehicleFeaturesMappingRepository);
            Check.Argument.IsNotNull(nameof(vehicleCategoryRepository), vehicleCategoryRepository);

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
            _parkingProvidersLocationsRepository = parkingProvidersLocationsRepository;
            _userVehiclesRepository = userVehiclesRepository;
            _vehiclesRepository = vehiclesRepository;
            _vehicleTagMappingRepository = vehicleTagMappingRepository;
            _reservationVehicleRepository = reservationVehicleRepository;
            _vehicleInfoService = vehicleInfoService;
            _vehicleFeaturesRepository = vehicleFeaturesRepository;
            _vehicleFeaturesMappingRepository = vehicleFeaturesMappingRepository;
            _vehicleCategoryRepository = vehicleCategoryRepository;
        }

        public ResponseResult<InComingOutGoingCarsVM> GetAllCars(string flag, long parkingProviderLocationId, long accountId, int searchType)
        {

            var data = new InComingOutGoingCarsVM();

            switch (searchType)
            {
                case (int)CarSearchType.All:
                    {
                        var incomingCars = _userVehiclesRepository.GetAllInComing<CarDetailVM>(flag, accountId, parkingProviderLocationId);
                        var ongoingCars = _userVehiclesRepository.GetAllOutgoing<CarDetailVM>(flag, accountId, parkingProviderLocationId);
                        if (incomingCars != null && incomingCars.Count > 0)
                            data.InComingCars = incomingCars;
                        if (ongoingCars != null && ongoingCars.Count > 0)
                            data.OutGoingCars = ongoingCars;
                    }
                    break;
                case (int)CarSearchType.IncomingCar:
                    {
                        var incomingCars = _userVehiclesRepository.GetAllInComing<CarDetailVM>(flag, accountId, parkingProviderLocationId);
                        if (incomingCars != null && incomingCars.Count > 0)
                            data.InComingCars = incomingCars;

                    }
                    break;
                case (int)CarSearchType.OutgoingCar:
                    {
                        var ongoingCars = _userVehiclesRepository.GetAllOutgoing<CarDetailVM>(flag, accountId, parkingProviderLocationId);
                        if (ongoingCars != null && ongoingCars.Count > 0)
                            data.OutGoingCars = ongoingCars;

                    }
                    break;
            }
            if (data == null)
            {
                return new ResponseResult<InComingOutGoingCarsVM>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = data
                };
            }
            return new ResponseResult<InComingOutGoingCarsVM>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = data
            };
        }

        public ResponseResult<CarDetailCountVM> GetCarCount(DateTime inputDateValue, long parkingProviderLocationId, long accountId)
        {
            CarDetailCountVM carDetailCountVM = new CarDetailCountVM();
            inputDateValue = DateTime.UtcNow;
            var dataInComingCount = _userVehiclesRepository.GetAllInComingCount<CarDetailCountVM>(inputDateValue, accountId, parkingProviderLocationId);
            var dataOngoingCount = _userVehiclesRepository.GetAllOutgoingCount<CarDetailCountVM>(inputDateValue, accountId, parkingProviderLocationId);

            var inComingCountYesterday = _userVehiclesRepository.GetAllInComingCountByDate<CarDetailCountVM>(inputDateValue.AddHours(-24), accountId, parkingProviderLocationId);
            var outgoingCountYesterday = _userVehiclesRepository.GetAllOutgoingCountByDate<CarDetailCountVM>(inputDateValue.AddHours(-24), accountId, parkingProviderLocationId);

            int incomingCarToday = dataInComingCount.IncomingCarCount;
            int outGoingCarToday = dataOngoingCount.OutgoingCarCount;// - dataOngoingCount.OutgoingCarCount;

            int incomingCarYes = inComingCountYesterday.IncomingCarCount;
            int outGoingCarYes = outgoingCountYesterday.OutgoingCarCount;

            int diffOutgoing = outGoingCarToday - outGoingCarYes;
            if (diffOutgoing > 0)
                carDetailCountVM.OutgoingIncreasePer = outGoingCarYes == 0 ? (diffOutgoing * 100) : (diffOutgoing * 100) / outGoingCarYes;
            else
                carDetailCountVM.OutgoingDecreasePer = outGoingCarYes == 0 ? (((outGoingCarYes - outGoingCarToday)) * 100) : (((outGoingCarYes - outGoingCarToday)) * 100) / outGoingCarYes;

            int diffIncoming = incomingCarToday - incomingCarYes;
            if (diffIncoming > 0)
                carDetailCountVM.IncomingIncreasePer = incomingCarYes == 0 ? (diffIncoming * 100) : (diffIncoming * 100) / incomingCarYes;
            else
                carDetailCountVM.IncomingDecreasePer = incomingCarYes == 0 ? (((incomingCarYes - incomingCarToday)) * 100) : (((incomingCarYes - incomingCarToday)) * 100) / incomingCarYes;

            if (dataInComingCount == null || dataOngoingCount == null)
            {
                return new ResponseResult<CarDetailCountVM>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            }
            carDetailCountVM.IncomingCarCount = dataInComingCount.IncomingCarCount;
            carDetailCountVM.OutgoingCarCount = dataOngoingCount.OutgoingCarCount;
            return new ResponseResult<CarDetailCountVM>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = carDetailCountVM
            };
        }
        public ResponseResult<DayWiseCarCountVM> GetDayWiseCount(long parkingProviderLocationId, long accountId)
        {
            DayWiseCarCountVM model = new DayWiseCarCountVM();

            var dataInComingCount = _userVehiclesRepository.GetIncomingDayWiseCount<DayWiseIncomingCount>(0, accountId, parkingProviderLocationId);
            var dataOutgoingCount = _userVehiclesRepository.GetOutgoingDayWiseCount<DayWiseOutgoingCount>(0, accountId, parkingProviderLocationId);
            if (dataInComingCount == null || dataOutgoingCount == null)
            {
                return new ResponseResult<DayWiseCarCountVM>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            }
            model.DayWiseIncomingCounts = dataInComingCount;
            model.DayWiseOutgoingCounts = dataOutgoingCount;

            return new ResponseResult<DayWiseCarCountVM>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = model
            };
        }

        /// <summary>
        /// GetVehicleDetailByTagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<VehicleDetailVM>> GetVehicleDetailByTagId(string tagId, long accountId)
        {
            var vehicleTagMappingResult = await _vehicleTagMappingRepository.GetVehicleTagMappingByTagId(tagId, accountId);
            if (vehicleTagMappingResult == null || vehicleTagMappingResult.Vehicle == null)
            {
                return new ResponseResult<VehicleDetailVM>()
                {
                    Message = ResponseMessage.VehicleDataNotExistWithTag,
                    ResponseCode = ResponseCode.VehicleDataNotExistWithTag,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.VehicleDataNotExistWithTag
                    }
                };
            }

            var vehicleDetails = new VehicleDetailVM()
            {
                CarTitle = vehicleTagMappingResult.Vehicle.Make,
                CarNumber = vehicleTagMappingResult.Vehicle.LicensePlate,
                CarImagePath = vehicleTagMappingResult.Vehicle.Logo,
                CarColorName = vehicleTagMappingResult.Vehicle.Color,
                VIN = vehicleTagMappingResult.Vehicle.Vinnumber,
                TagID = vehicleTagMappingResult.TagId,
                UserId = vehicleTagMappingResult.Vehicle.LoggedInUserId ?? 0,
                VehicleId = vehicleTagMappingResult.VehicleId
            };
            

            return new ResponseResult<VehicleDetailVM>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = vehicleDetails
            };
        }

        /// <summary>
        /// CreateReservationVehicle
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> CreateReservationVehicle(CreateReservationVehicleReqVM model, long accountId, long userId)
        {
            // Validations
            if (model == null || accountId < 1 || userId < 1)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            };

            var errorDetails = new Dictionary<string, string[]>();
            if (model.ReservationId < 1)
            {
                errorDetails.Add("reservationId", new string[] { "Field could not be blank or zero." });
            }
            if (model.VehicleId < 1)
            {
                errorDetails.Add("vehicleId", new string[] { "Field could not be blank or zero." });
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

            // Get Reservation Details
            Reservation reservationResult = _reservationRepository.GetReservationById(model.ReservationId, accountId);
            if (reservationResult == null || reservationResult.ParkingReservation == null || reservationResult.ParkingReservation.Count < 1)
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

            // save user vehicle
            UserVehicles userVehicles = _userVehiclesRepository.GetUserVehiclesDataByVehicleIdAndUserID(model.VehicleId, accountId, reservationResult.UserId);
            
            if (userVehicles == null)
            {
                userVehicles = new UserVehicles()
                {
                    VehicleId = model.VehicleId,
                    UserId = reservationResult.UserId,
                    LoggedInUserId = userId,
                    AccountId = accountId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };
                await _userVehiclesRepository.CreateUserVehicleDetails(userVehicles);
            }

            // Create Reservation Vehicle
            var reservationVehicle = new ReservationVehicle()
            {
                ReservationId = model.ReservationId,
                ParkingProvidersLocationId = reservationResult.ParkingReservation.FirstOrDefault().ParkingProvidersLocationId ?? 0,
                VehicleId = model.VehicleId,
                IsConsented = false,
                AccountId = accountId,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            }; 

           await _reservationVehicleRepository.CreateReservationVehicleDetails(reservationVehicle);

            if (unitOfWork.CommitWithStatus() > 0)
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
                    Message = "Could not able to save details for Reservation Vehicle.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }

        /// <summary>
        /// Create Vehicle & Reservation Vehicle
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="vehicleId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> CreateVehicle(CreateVehicleReqVM model, long accountId, long userId)
        {
            // Validations
            if (model == null || accountId < 1 || userId < 1)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            };

            var errorDetails = new Dictionary<string, string[]>();
            if (model.ReservationId < 1)
            {
                errorDetails.Add("reservationId", new string[] { "Field could not be blank or zero." });
            }
            if (string.IsNullOrWhiteSpace(model.VinNumber))
            {
                errorDetails.Add("vinNumber", new string[] { "Field could not be blank or zero." });
            }
            if (string.IsNullOrWhiteSpace(model.LicensePlateNumber))
            {
                errorDetails.Add("licensePlateNumber", new string[] { "Field could not be blank or zero." });
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

            // Get Reservation Details
            Reservation reservationResult = _reservationRepository.GetReservationById(model.ReservationId, accountId);

            if (reservationResult == null || reservationResult.ParkingReservation == null || reservationResult.ParkingReservation.Count < 1)
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
            bool newVehicle = false;
            Vehicles vehicle =null;
            var vehicleCategory = await _vehicleCategoryRepository.GetVehicleCategoryByName("Others", accountId);
            if (vehicleCategory == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = "Could not able to vehicle category details.",
                    ResponseCode = ResponseCode.SomethingWentWrong,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.SomethingWentWrong
                    }
                };
            }
            if (model.VinNumber=="provider")
            {
                vehicle = new Vehicles()
                {
                    Make = model.Make ?? "",
                    Model = model.Model ?? "",
                    Year = model.ModelYear ?? "",
                    LicensePlate = model.LicensePlateNumber,
                    Vinnumber = model.VinNumber,
                    NumberOfDoors = Convert.ToInt32(model.NumberOfDoors),
                    IsTransmissionAutomatic = model.Transmission,
                    NumberOfBags = Convert.ToInt32(model.NumberOfBags),
                    NumberOfPassenger = Convert.ToInt32(model.NumberOfPassenger),

                    RegistrationState = "",
                    Logo = "",
                    Color = model.CarColor,
                    IsConvertable = false,
                    Ticket=model.Ticket,
                    VehicleStateId= model.VehicleStateId,
                    AccountId = accountId,
                    LoggedInUserId = userId,
                    VehicleCategoryId = vehicleCategory.VehicleCategoryId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };
                var vehicleResult = await _vehiclesRepository.CreateVehicleDetails(vehicle);
                if (vehicleResult == null || vehicleResult.VehicleId < 1)
                {
                    return new ResponseResult<bool>()
                    {
                        Message = ResponseMessage.SomethingWentWrong,
                        ResponseCode = ResponseCode.SomethingWentWrong
                    };
                }

                vehicle.VehicleId = vehicleResult.VehicleId;
                newVehicle = true;
            }
            else
            {
                // Get vehicle Details based on VIN number
                vehicle = _vehiclesRepository.GetVehicleDetailsByVinNumber(model.VinNumber, accountId);
               
                if (vehicle == null)
                {
                    // Getting vehicle Info data from Vin Decoder API
                    var vinDecoderResponse = await _vehicleInfoService.GetVehicleInfo(model.VinNumber);
                    if (vinDecoderResponse.ResponseCode != ResponseCode.RecordFetched || vinDecoderResponse.Data == null || vinDecoderResponse.Data.VehicleInfoData == null)
                    {
                        return new ResponseResult<bool>()
                        {
                            Message = vinDecoderResponse.Message,
                            ResponseCode = vinDecoderResponse.ResponseCode,
                            Error = vinDecoderResponse.Error
                        };
                    }

                    

                    vehicle = new Vehicles()
                    {
                        Make = vinDecoderResponse.Data.VehicleInfoData.Make ?? "",
                        Model = vinDecoderResponse.Data.VehicleInfoData.Model ?? "",
                        Year = vinDecoderResponse.Data.VehicleInfoData.ModelYear ?? "",
                        LicensePlate = model.LicensePlateNumber,
                        Vinnumber = model.VinNumber,
                        NumberOfDoors = Convert.ToInt32(vinDecoderResponse.Data.VehicleInfoData.NumberofDoors),
                        IsTransmissionAutomatic = vinDecoderResponse.Data.VehicleInfoData.Transmission?.Trim().ToLower() == "automatic",
                        NumberOfBags = Convert.ToInt32(vinDecoderResponse.Data.VehicleInfoData.MinimumvolumeofLuggage),
                        NumberOfPassenger = Convert.ToInt32(vinDecoderResponse.Data.VehicleInfoData.NumberofSeats),

                        RegistrationState = "",
                        Logo = "",
                        Color = model.CarColor,
                        IsConvertable = false,
                        Ticket = model.Ticket,
                        VehicleStateId = model.VehicleStateId,
                        AccountId = accountId,
                        LoggedInUserId = userId,
                        VehicleCategoryId = vehicleCategory.VehicleCategoryId,
                        CreatedBy = userId,
                        CreatedOn = DateTime.UtcNow
                    };

                    // Save Vehicle data
                    var vehicleResult = await _vehiclesRepository.CreateVehicleDetails(vehicle);
                    if (vehicleResult == null || vehicleResult.VehicleId < 1)
                    {
                        return new ResponseResult<bool>()
                        {
                            Message = ResponseMessage.SomethingWentWrong,
                            ResponseCode = ResponseCode.SomethingWentWrong
                        };
                    }

                    vehicle.VehicleId = vehicleResult.VehicleId;
                    newVehicle = true;
                }
            }
            // Save Vehicle Tag Mapping Table
            VehicleTagMapping vehicleTagMapping = null;
            if (!newVehicle)
            {
                vehicleTagMapping = await _vehicleTagMappingRepository.GetVehicleTagMappingByTagIdAndVehicleId(model.TabId, vehicle.VehicleId, accountId);                
            }

            if (vehicleTagMapping == null)
            {
                vehicleTagMapping = new VehicleTagMapping()
                {
                    TagId = model.TabId,
                    VehicleId = vehicle.VehicleId,
                    AccountId = accountId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };
                await _vehicleTagMappingRepository.CreateVehicleTagMappingDetails(vehicleTagMapping);
            }

            // save user vehicle
            UserVehicles userVehicles = null;
            if (!newVehicle)
            {
                userVehicles = _userVehiclesRepository.GetUserVehiclesDataByVehicleIdAndUserID(vehicle.VehicleId, accountId, reservationResult.UserId);
            }

            if (userVehicles == null)
            {
                userVehicles = new UserVehicles()
                {
                    VehicleId = vehicle.VehicleId,
                    UserId = reservationResult.UserId,
                    LoggedInUserId = userId,
                    AccountId = accountId,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow
                };

                await _userVehiclesRepository.CreateUserVehicleDetails(userVehicles);
            }

            // Save reservation vehicle 
            var reservationVehicle = new ReservationVehicle()
            {
                ParkingProvidersLocationId = reservationResult.ParkingReservation.FirstOrDefault().ParkingProvidersLocationId ?? 0,
                ReservationId = reservationResult.ReservationId,
                VehicleId = vehicle.VehicleId,
                IsConsented = false,
                AccountId = accountId,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            };

            await _reservationVehicleRepository.CreateReservationVehicleDetails(reservationVehicle);

            if (unitOfWork.CommitWithStatus() > 0)
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
                    Message = "Could not able to save details for vehicle details.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }

        public ResponseResult<List<CarDetailVM>> GetJourneyCompletedlist(string flag, long parkingProviderLocationId, long accountId)
        {
            var data=  _userVehiclesRepository.GetJourneyCompletedlist<CarDetailVM>(flag, accountId, parkingProviderLocationId);
            if (data == null || data.Count==0)
            {
                return new ResponseResult<List<CarDetailVM>>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            }
            return new ResponseResult<List<CarDetailVM>>()
            {
                Data = data,
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched
            };
        }
    }
}
