using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator.IServices.Services.CommonUtility
{
    /// <summary>
    /// IServiceCommonUtility
    /// </summary>
    public interface IServiceCommonUtility
    {
        /// <summary>
        /// Manage Service Client Response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objHttpResponseMessage"></param>
        /// <param name="serviceOperationType"></param>
        /// <returns></returns>
        public ResponseResult<T> ManageServiceClientResponse<T>(HttpResponseMessage objHttpResponseMessage, string serviceOperationType);
        /// <summary>
        /// Manage Service Client Response
        /// </summary>
        /// <param name="objHttpResponseMessage"></param>
        /// <param name="serviceOperationType"></param>
        /// <returns></returns>
        public ResponseResult ManageServiceClientResponse(HttpResponseMessage objHttpResponseMessage, string serviceOperationType);
    }
}
