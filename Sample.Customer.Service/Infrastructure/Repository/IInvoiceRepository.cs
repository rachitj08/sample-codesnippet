using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IInvoiceRepository
    {
        Task<int> CreateInvoice(Invoice model);

        Task<int> CreateInvoice(InvoiceVM model, long accountId, long userId);
        
    }
}
