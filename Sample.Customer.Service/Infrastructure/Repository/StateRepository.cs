using Common.Model;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository.ParkingProvider;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class StateRepository : RepositoryBase<State>, IStateRepository
    {
        public StateRepository(CloudAcceleratorContext context) : base(context)
        {

        }

       

        public async Task<List<State>> GetState(long countryId)
        {
            return await GetAll(x=> x.CountryId ==countryId);
        }

        public State GetStateByName(string stateName,long countryId)
        {
            return context.State.Where(x=> x.Name==stateName && x.CountryId==countryId).FirstOrDefault();
        }

        State GetById(long countryId)
        {
            return GetById(countryId);
        }
       
    }
}
