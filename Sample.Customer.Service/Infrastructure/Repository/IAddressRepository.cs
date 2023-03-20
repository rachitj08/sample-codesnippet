using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
  public interface IAddressRepository
    {
        Task<IList<Address>> GetAllAddress(long accountId);
        IQueryable<Address> GetAddressQuery(long accountId);
        Task<Address> CreateAddress(Address address);
        Address UpdateAddress(Address address);

        Address GetById(long addresId);
        Task<Address> CreateAddressNew(Address address);
        Task<Address> UpdateAddressNew(Address address);
    }
}
