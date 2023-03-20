using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class ParkingProvidersLocationsRepository : RepositoryBase<ParkingProvidersLocations>, IParkingProvidersLocationsRepository
    {
        public ParkingProvidersLocationsRepository(CloudAcceleratorContext context) : base(context)
        {

        }       
        public ParkingProvidersLocations CreateParkingProvidersLocations(ParkingProvidersLocations objParkingProvidersLocations, long accountId)
        {
            throw new NotImplementedException();
        }

        public void DeleteParkingProvidersLocations(ParkingProvidersLocations objParkingProvidersLocations)
        {
            throw new NotImplementedException();
        }

        public List<ParkingProvidersLocations> GetAllParkingProvidersLocations()
        {
            throw new NotImplementedException();
        }

        public List<ParkingProvidersLocations> GetParkingProvidersLocationsById(long parkingProvidersLocationsId)
        {
            //return base.GetById(parkingProvidersLocationsId);
            List<ParkingProvidersLocations> objParkingProvidersLocations = base.context.ParkingProvidersLocations.
                Where(x=>x.ParkingProvidersLocationId == parkingProvidersLocationsId).Include(x => x.Address).Include(x => x.ParkingHeadsRate).ToList();
            return objParkingProvidersLocations;
        }

        public async Task<ParkingProvidersLocations> GetParkingProvidersLocationsById(long parkingProvidersLocationsId, long accountId)
        {
            return await SingleAsnc(x => x.AccountId == accountId && x.ParkingProvidersLocationId == parkingProvidersLocationsId);
        }

        public ParkingProvidersLocations UpdateParkingProvidersLocations(long parkingHeadid, long accountId, ParkingProvidersLocations objParkingProvidersLocations)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ParkingProvidersLocations> GetParkingProvidersLocationsQuery(long accountId)
        {
            return base.GetQuery(x => x.AccountId == accountId);//, "ParkingProvidersLocation"
        }
    }
}
