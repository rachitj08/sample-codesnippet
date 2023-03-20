using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IPaymentDetailsRepository
    {
        /// <summary>
        /// Get Payment Details By Id
        /// </summary>
        /// <param name="paymentDetailId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<PaymentDetailVM> GetPaymentDetailsById(long paymentDetailId, long accountId);

        /// <summary>
        /// Create Payment Details
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> CreatePaymentDetails(PaymentDetails model, long accountId, long userId);

        /// <summary>
        /// Update Payment Details
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> UpdatePaymentDetails(PaymentDetails model, long accountId, long userId);
    }
}