using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {


        public AddressRepository(CloudAcceleratorContext context) : base(context)
        {

        }

        public async Task<IList<Address>> GetAllAddress(long accountId)
        {
            return await GetAll(x => x.AccountId == accountId);
        }

        public IQueryable<Address> GetAddressQuery(long accountId)
        {
            return GetQuery(x => x.AccountId == accountId);
        }

        public async Task<Address> CreateAddress(Address address)
        {
            return await CreateAsync(address);
        }
        public Address UpdateAddress(Address address)
        {
            return Update(address);
        }
        public Address GetById(long addresId)
        {

            var add = context.Address.Where(x => x.AddressId == addresId).FirstOrDefault();
            return add;

        }
        public async Task<Address> CreateAddressNew(Address address)
        {
            var add= await CreateAsync(address);
            await base.context.SaveChangesAsync();
            return add;
        }
        public async Task<Address> UpdateAddressNew(Address address)
        {
            var add = Update(address);
            await base.context.SaveChangesAsync();
            return add;
        }
    }
}
