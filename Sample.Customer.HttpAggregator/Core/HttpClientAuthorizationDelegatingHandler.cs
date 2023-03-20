using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccesor;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccesor"></param>
        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor;
        }
        /// <summary>
        /// Validate user token and add Authorization key in Headers of the request object.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string tokenString = string.Empty;
            var authorizationHeader = _httpContextAccesor.HttpContext
                .Request.Headers["Authorization"];

            if (_httpContextAccesor.HttpContext.Request.Headers.ContainsKey("accountid"))
            {
                var accountidHeader = _httpContextAccesor.HttpContext.Request.Headers["accountid"];
                request.Headers.Add("accountid", new List<string>() { accountidHeader.ToString() });
            }

            if (_httpContextAccesor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var token = _httpContextAccesor.HttpContext.Request.Headers["Authorization"];
                request.Headers.Add("Authorization", new List<string>() { token.ToString() });
            } 

            if (!request.Headers.Contains("userid") && _httpContextAccesor.HttpContext.Request.Headers.ContainsKey("userid"))
            {
                var userId = _httpContextAccesor.HttpContext.Request.Headers["userid"];
                request.Headers.Add("userid", new List<string>() { userId.ToString() });
            }

            if (!request.Headers.Contains("deviceId") && _httpContextAccesor.HttpContext.Request.Headers.ContainsKey("deviceId"))
            {
                var deviceId = _httpContextAccesor.HttpContext.Request.Headers["deviceId"];
                request.Headers.Add("deviceId", new List<string>() { deviceId.ToString() });
            }
            //if (!string.IsNullOrEmpty(authorizationHeader) && !string.IsNullOrEmpty(accountId))
            //{
            //    tokenString = authorizationHeader.ToString().Replace("Bearer ", "").Replace("Bearer=", "");
            //    //var isTokenValid = GetToken(tokenString, Convert.ToInt64(userId));
            //    //if (isTokenValid)
            //    //{
            //    //    request.Headers.Add("Authorization", new List<string>() { tokenString });
            //    //}
            //    //else
            //    //{
            //    //    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //    //}
            //}
            //else
            //{
            //    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //}

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
