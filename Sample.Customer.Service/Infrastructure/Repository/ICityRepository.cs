using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface ICityRepository
    {
        Task<List<City>> GetCity(long stateId);
        City GetById(long countryId);
        City GetCityByName(string cityName, long stateId);
    }
}
