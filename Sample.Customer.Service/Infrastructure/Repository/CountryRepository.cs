using Common.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository.ParkingProvider;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(CloudAcceleratorContext context) : base(context)
        {

        }

       

        public IEnumerable<Country> GetCountry()
        {
            return GetAll();
        }

        Country ICountryRepository.GetById(long countryId)
        {
            return GetById(countryId);
        } 
        public Country GetByCode(string countryCode)
        {
            return context.Country.Where(x=> x.CountryCode==countryCode || x.Name==countryCode).FirstOrDefault();
        }
    }
}
