using Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IStateRepository
    {
        Task<List<State>> GetState(long countryId);
       State GetById(long countryId);
       State GetStateByName(string stateName,long countryId);
    }
}
