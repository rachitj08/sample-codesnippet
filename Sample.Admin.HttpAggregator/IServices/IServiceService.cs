using Common.Model;
using Sample.Admin.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// interface for Service
    /// </summary>
    public interface IServiceService
    {
        /// <summary>
        /// Get Service List
        /// </summary>
        /// <param name="httpContext">httpContext for base URL</param>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (ServiceId, ServiceName)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        Task<ResponseResultList<ServiceModel>> GetServiceList(HttpContext httpContext, int pageSize, int pageNumber, string ordering, string search, int offset, bool all);

        /// <summary>
        /// Get Service Detail
        /// </summary>
        /// <param name="serviceId">Service Id</param>
        /// <returns></returns>
        Task<ResponseResult<ServiceModel>> GetServiceDetails(int serviceId);
    }
}
