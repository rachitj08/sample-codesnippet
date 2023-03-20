using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.Reservation;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class AirportsParkingService : IAirportsParkingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAirportsParkingRepository _airportsParkingRepository;
        private readonly IMapper mapper;

        public AirportsParkingService(IUnitOfWork unitOfWork, IAirportsParkingRepository airportsParkingRepository, IMapper mapper)
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(airportsParkingRepository), airportsParkingRepository);
            Check.Argument.IsNotNull(nameof(mapper), mapper);
            this.unitOfWork = unitOfWork;
            this._airportsParkingRepository = airportsParkingRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// GetAllAirportsParkingByAirportId
        /// </summary>
        /// <param name="airportId"></param>
        /// <returns></returns>
        public async Task<AirportsParkingVM> GetAllAirportsParkingByAirportId(long airportId)
        {
            AirportsParkingVM objAirportsParkingVM = new AirportsParkingVM();
            AirportsParking objAirportsParking = await _airportsParkingRepository.GetAllAirportsParkingByAirportId(airportId);
            if (objAirportsParking != null && objAirportsParking.Airport != null)
            {
                objAirportsParkingVM.AirportsParkingId = objAirportsParking.AirportsParkingId;
                objAirportsParkingVM.AirportId = objAirportsParking.AirportId;
                objAirportsParkingVM.ParkingProvidersLocationId = objAirportsParking.ParkingProvidersLocationId;
                objAirportsParkingVM.CreatedOn = objAirportsParking.CreatedOn;
                objAirportsParkingVM.CreatedBy = objAirportsParking.CreatedBy;
                objAirportsParkingVM.UpdatedOn = objAirportsParking.UpdatedOn;
                objAirportsParkingVM.UpdatedBy = objAirportsParking.UpdatedBy;
                objAirportsParkingVM.LoggedInUserId = objAirportsParking.LoggedInUserId;

                if (objAirportsParkingVM.Airport == null)
                    objAirportsParkingVM.Airport = new AirportsVM();
                objAirportsParkingVM.Airport.AirportId = objAirportsParking.Airport.AirportId;
                objAirportsParkingVM.Airport.Name = objAirportsParking.Airport.Name;
                objAirportsParkingVM.Airport.Code = objAirportsParking.Airport.Code;
                objAirportsParkingVM.Airport.AddressId = objAirportsParking.Airport.AddressId;
            }
            return objAirportsParkingVM;
        }


        /// <summary>
        /// Select Airports Parking By AirportId for reservation
        /// </summary>
        /// <param name="airportId"></param>
        /// <param name="accountId"></param>
        /// <param name="airportsParkingId"></param>
        /// <returns></returns>
        public async Task<AirportParkingVM> SelectAirportsParkingByAirportId(long airportId, long accountId, long airportsParkingId)
        {
            AirportsParking airportsParking;
            if (airportsParkingId < 1)
            {
                airportsParking = await _airportsParkingRepository.GetAirportsParkingByAirportId(airportId, accountId).FirstOrDefaultAsync();
            }
            else
            {
                airportsParking = await _airportsParkingRepository.GetAirportsParkingById(airportsParkingId, accountId);
            }

            if (airportsParking != null && airportsParking.Airport != null)
            {
                var dataModel = new AirportParkingVM();
                dataModel.AirportsParkingId = airportsParking.AirportsParkingId;
                dataModel.AirportId = airportsParking.AirportId;
                dataModel.ParkingProvidersLocationId = airportsParking.ParkingProvidersLocationId;
                dataModel.LoggedInUserId = airportsParking.LoggedInUserId;

                dataModel.ParkingProvidersLocations = mapper.Map<ParkingProvidersLocationsVM>(airportsParking?.ParkingProvidersLocation);   
                dataModel.Airport = mapper.Map<AirportsVM>(airportsParking.Airport);                   
                dataModel.ParkingProvidersLocationAddress = mapper.Map<AddressVM>(airportsParking?.ParkingProvidersLocation?.Address);
                return dataModel;
            }
            return null;
        }
         
    }
}
