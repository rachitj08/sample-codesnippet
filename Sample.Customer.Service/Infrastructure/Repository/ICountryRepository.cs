using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetCountry();
        Country GetById(long countryId);
        Country GetByCode(string countryCode);
    }
}
