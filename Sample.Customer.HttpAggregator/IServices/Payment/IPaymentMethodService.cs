using Common.Model;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Threading.Tasks;
using Sample.Customer.Model.Model;

namespace Sample.Customer.HttpAggregator.IServices.Payment
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPaymentMethodService
    {
        /// <summary>
        /// Create Payment Method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResponseResult<PaymentResponseVM>> CreatePaymentMethod(CardRequestVM model);
        /// <summary>
        /// Check Valid Payment
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<bool>> FetchValidPaymentMethod();
    }
}
