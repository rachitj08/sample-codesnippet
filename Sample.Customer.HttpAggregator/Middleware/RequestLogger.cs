using Common.Model;
using Common.Model.Model;
using Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using UAParser;

namespace Sample.Customer.HttpAggregator.Middleware
{
    /// <summary>
    /// RequestLogger
    /// </summary>
    public class RequestLogger
    {
        private readonly RequestDelegate _next;
        private readonly IRequestLogger _requestLogger;
        /// <summary>
        /// RequestLogger
        /// </summary>
        /// <param name="next"></param>
        /// <param name="requestLogger"></param>
        public RequestLogger(RequestDelegate next, IRequestLogger requestLogger)
        {
            _next = next;
            _requestLogger = requestLogger;
        }
        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var request = new RequestLog();
            //if (httpContext.Request.Path.Value.Contains("swagger")
            //    || httpContext.Request.Path.Value == "/swagger/index.html")
            //{
            //    await _next.Invoke(httpContext);
            //    return;
            //}
             
            // Get Device Id
            var deviceId = GetHeaderValue(httpContext, "deviceId");

            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = GetHeaderValue(httpContext, "x-dcmguid");
                if (string.IsNullOrEmpty(deviceId))
                    deviceId = GetHeaderValue(httpContext, "x-up-subno");
                if (string.IsNullOrEmpty(deviceId))
                    deviceId = GetHeaderValue(httpContext, "x-jphone-uid");
                if (string.IsNullOrEmpty(deviceId))
                    deviceId = GetHeaderValue(httpContext, "x-em-uid");
            }
            if (!string.IsNullOrEmpty(deviceId))
                request.DeviceId = deviceId;
            //else
            //{
            //    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new ResponseResult()
            //    {
            //        Message = "Device Id not provided",
            //        ResponseCode = ResponseCode.InternalServerError,
            //        Error = new ErrorResponseResult()
            //        {
            //            Message = "Device Id not provided",
            //        }
            //    }));
            //    return;
            //}

            //API Name
            request.Apiname = httpContext.Request.Path.Value;

            // IP Address
            request.Ipaddress = GetIPAddressValue(httpContext);

            // Requested From
            request.RequestedFrom = GetRequestedFromValue(httpContext);

            // Get Account Id 
            string accountId = GetHeaderValue(httpContext, "accountid");
            if (!string.IsNullOrEmpty(accountId)) request.AccountId = Convert.ToInt64(accountId);

            // Get User Id
            request.UserId = 0;
            string userId = GetHeaderValue(httpContext, "userid");
            if (!string.IsNullOrEmpty(userId)) request.UserId = Convert.ToInt64(userId);

            // Get API Version
            request.AppVersion = GetHeaderValue(httpContext, "AppVersion");

            // Get Token
            request.EncryptedToken = GetHeaderValue(httpContext, "Authorization");

            // Get User - Agent and Device Info
            request.DeviceType = 0;
            request.UserAgent = GetHeaderValue(httpContext, "User-Agent");
            if (httpContext.Request.Headers.ContainsKey("User-Agent"))
            {
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(request.UserAgent);
                if (clientInfo != null)
                {
                    //Device Model/Manufacture
                    if (clientInfo.Device != null)
                    {
                        if (!string.IsNullOrWhiteSpace(clientInfo.Device.Model))
                            request.DeviceModel = clientInfo.Device.Model;

                        if (!string.IsNullOrWhiteSpace(clientInfo.Device.Brand))
                            request.DeviceManufacture = clientInfo.Device.Brand;
                        else if (!string.IsNullOrWhiteSpace(clientInfo.Device.Family))
                            request.DeviceManufacture = clientInfo.Device.Family;
                    }

                    //Device OS
                    if (clientInfo.OS != null && !string.IsNullOrWhiteSpace(clientInfo.OS.Family))
                        request.DeviceOs = clientInfo.OS.Family;
                    else if (clientInfo.UA != null && !string.IsNullOrWhiteSpace(clientInfo.UA.Family))
                        request.DeviceOs = clientInfo.UA.Family;
                    else if (clientInfo.Device != null && !string.IsNullOrWhiteSpace(clientInfo.Device.Family))
                        request.DeviceOs = clientInfo.Device.Family;

                    //Device OS Version
                    if (clientInfo.OS != null && !string.IsNullOrWhiteSpace(clientInfo.OS.Major))
                        request.DeviceOsversion = clientInfo.OS.Major;
                    else if (clientInfo.UA != null && !string.IsNullOrWhiteSpace(clientInfo.UA.Major))
                        request.DeviceOsversion = clientInfo.UA.Major;

                    if (!string.IsNullOrWhiteSpace(request.DeviceOs))
                        request.DeviceType = GetDeviceType(request.DeviceOs);
                }
            }
            request.CreatedOn = DateTime.UtcNow;

            _requestLogger.AddRequestLog(request).ConfigureAwait(false);
            await _next.Invoke(httpContext);
        }

        private string GetIPAddressValue(HttpContext httpContext)
        {
            if (httpContext.Request.HttpContext != null
                && httpContext.Request.HttpContext.Connection != null
                && httpContext.Request.HttpContext.Connection.RemoteIpAddress != null)
            {
                return httpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            else if (httpContext.Connection != null && httpContext.Connection.RemoteIpAddress != null)
            {
                return httpContext.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                return "";
            }
        }

        private string GetRequestedFromValue(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Origin", out var origin))
            {
                return origin;
            }
            else if (httpContext.Request.Host != null
                && !string.IsNullOrWhiteSpace(httpContext.Request.Host.Value))
            {
                return httpContext.Request.Host.Value;
            }
            else
            {
                return "";
            }
        }

        private string GetHeaderValue(HttpContext httpContext, string key)
        {
            if (httpContext.Request.Headers.ContainsKey(key)
                && httpContext.Request.Headers.TryGetValue(key, out var rtnValue)
                && !string.IsNullOrWhiteSpace(rtnValue))
            {
                return rtnValue;
            }
            return "";
        }

        private short GetDeviceType(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 4;
            value = value.Trim().ToLower();

            if (value.Contains("ios") || value.Contains("iphone"))
                return 1;
            else if (value.Contains("android"))
                return 2;
            else if (value.Contains("windows") || value.Contains("mac"))
                return 3;
            else
                return 4;
        }
    }

    /// <summary>
    /// RequestLogExtensions
    /// </summary>
    public static class RequestLogExtensions
    {
        /// <summary>
        /// UseRequestLog
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogger>();
        }
    }


}
