using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IPaymentIntentService
    {
        /// <summary>
        /// Create Payment Intent
        /// </summary>
        /// <param name="modelData"></param>
        Task CreatePaymentIntent(string modelData);

        /// <summary>
        /// Update Payment Intent
        /// </summary>
        /// <param name="modelData"></param>
        Task UpdatePaymentIntent(string modelData);

        /// <summary>
        /// Cancel Payment Intent
        /// </summary>
        /// <param name="modelData"></param>
        void CancelPaymentIntent(string modelData);

        /// <summary>
        /// Capture Payment Intent
        /// </summary>
        /// <param name="modelData"></param>
        Task CapturePaymentIntent(string modelData);
    }
}
