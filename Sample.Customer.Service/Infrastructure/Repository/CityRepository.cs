using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository.ParkingProvider;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(CloudAcceleratorContext context) : base(context)
        {

        }



        public async Task<List<City>> GetCity(long stateId)
        {
            return await GetAll(x => x.StateId == stateId);
        }


        public City GetById(long countryId)
        {
            return GetById(countryId);
        } 
        public City GetCityByName(string cityName,long stateId)
        {
            return context.City.Where(x => x.Name == cityName && x.StateId==stateId).FirstOrDefault();
        }
    }
}
