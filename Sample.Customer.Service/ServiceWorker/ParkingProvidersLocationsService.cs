using Common.Model;
using System;
using Utilities;
using Sample.Customer.Model.Model.ParkingHeads;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class ParkingProvidersLocationsService : IParkingProvidersLocationsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IParkingProvidersLocationsRepository _parkingProvidersLocationsRepository;
        private readonly IAirportsParkingService _airportsParkingService;

        public ParkingProvidersLocationsService(IUnitOfWork unitOfWork, IParkingProvidersLocationsRepository parkingProvidersLocationsRepository,
             IAirportsParkingService airportsParkingService)
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(parkingProvidersLocationsRepository), parkingProvidersLocationsRepository);
            Check.Argument.IsNotNull(nameof(airportsParkingService), airportsParkingService);
            this.unitOfWork = unitOfWork;
            this._parkingProvidersLocationsRepository = parkingProvidersLocationsRepository;
            _airportsParkingService = airportsParkingService;

        }
        public ParkingProvidersLocationsVM CreateParkingProvidersLocations(ParkingProvidersLocationsVM ojParkingProvidersLocationsVM,
            long accountId)
        {
            throw new NotImplementedException();
        }

        public void DeleteParkingProvidersLocations(ParkingProvidersLocationsVM ojParkingProvidersLocationsVM)
        {
            throw new NotImplementedException();
        }

        public ResponseResultList<ParkingProvidersLocationsVM> GetAllParkingProvidersLocations()
        {
            throw new NotImplementedException();
        }

        public ResponseResult<ParkingProvidersLocationsVM> GetParkingProvidersLocationsById(long parkingProvidersLocationsId)
        {
            throw new NotImplementedException();
        }

        public ParkingProvidersLocationsVM UpdateParkingProvidersLocations(long parkingHeadid, long accountId, ParkingProvidersLocationsVM ojParkingProvidersLocationsVM)
        {
            throw new NotImplementedException();
        }


    }
}
