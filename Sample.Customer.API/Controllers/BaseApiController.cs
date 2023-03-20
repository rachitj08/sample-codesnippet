using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using Common.Model;
using Common.Model.Model;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public long loggedInAccountId = 0;

        public long loggedInUserId = 0;

        public string deviceId = "";

        public string apiName = "";

        public long loggedInClientId = 0; 

        private IFileLogger logger;

        private DateTime StartDate;

        private DateTime EndDate;

        public Task<IActionResult> DefaultResponse = null;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public BaseApiController()
        {

        }

        /// <summary>
        /// To Logger dependency inject
        /// </summary>
        /// <param name="logger">logger taken parameter for logging in the application </param>
        public BaseApiController(IFileLogger logger)
        {
            this.logger = logger;
        }



        /// <summary>
        /// Execute Method
        /// </summary>
        /// <param name="action"> action performed for any result set</param>
        /// <returns></returns>
        protected async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
        {
            try
            {
                if (HttpContext.Request.Headers.ContainsKey("accountId"))
                {
                    loggedInAccountId = Convert.ToInt64(HttpContext.Request.Headers["accountId"]);
                }

                if (HttpContext.Request.Headers.ContainsKey("userid") 
                    && !string.IsNullOrWhiteSpace(Convert.ToString(HttpContext.Request.Headers["userid"])))
                {
                    long.TryParse(HttpContext.Request.Headers["userid"], out loggedInUserId);
                }

                if (HttpContext.Request.Headers.ContainsKey("deviceId") 
                    && !string.IsNullOrWhiteSpace(Convert.ToString(HttpContext.Request.Headers["deviceId"])))
                {
                    deviceId = Convert.ToString(HttpContext.Request.Headers["deviceId"]);
                }
                if (HttpContext.Request.Headers.ContainsKey("clientid"))
                {
                    loggedInClientId = Convert.ToInt64(HttpContext.Request.Headers["clientid"]);
                }
                HttpContext.Request.RouteValues["controller"].ToString();
                apiName = string.Concat(HttpContext.Request.RouteValues["controller"].ToString(), "/",
                       HttpContext.Request.RouteValues["action"].ToString());

                StartDate = DateTime.UtcNow;
                var result = action().GetAwaiter().GetResult();
                EndDate = DateTime.UtcNow;
                AuditLogging(StartDate, EndDate, true);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Customer API Exception :: " + ex.Message);
                Console.WriteLine("Customer API Stack trace :: " + ex.StackTrace);
                ExceptionLogging(StartDate, EndDate, false, ex);
                ResponseResult response = new ResponseResult();
                response.Error = new ErrorResponseResult() { Message = ex.Message };
                response.ResponseCode = ResponseCode.InternalServerError;
                return BadRequest(response);
            }
        }

        /// <summary>
        /// To Log  Messages
        /// </summary>
        /// <param name="startDate">when logging started </param>
        /// <param name="endDate"> when logging ended</param>
        /// <param name="status">log status </param>
        /// <param name="message"> message after successful logging</param>
        void AuditLogging(DateTime startDate, DateTime endDate, bool status, string message = "success")
        {

            TimeSpan timeDiff = endDate - startDate;

            int ms = (int)timeDiff.TotalMilliseconds;

            long UserId = 0;
            if (HttpContext.Request.Headers.ContainsKey("userid"))
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Request.Headers["userid"]))
                    long.TryParse(HttpContext.Request.Headers["userid"], out UserId);
            }
            var absoluteUri = string.Concat(
                        HttpContext.Request.Scheme,
                        "://",
                        HttpContext.Request.Host.ToUriComponent(),
                        HttpContext.Request.PathBase.ToUriComponent(),
                        HttpContext.Request.Path.ToUriComponent(),
                        HttpContext.Request.QueryString.ToUriComponent());

            var logData = new AuditLogEntity
            {
                LoggedDate = DateTime.UtcNow,
                StartDate = startDate,
                EndDate = endDate,
                Url = Convert.ToString(absoluteUri),
                IsSuccess = status,
                Method = Convert.ToString(HttpContext.Request.Method),
                ProcessedTime = Convert.ToString(ms),
                UserId = UserId,
                Message = message,
                ModuleId = 1
            };
            this.logger.WriteInfo(logData);
        }

        /// <summary>
        /// To Log all activity and exception in the logger file
        /// </summary>
        /// <param name="startDate">The startDate datetime parameter</param>
        /// <param name="endDate">The EndDate datetime parameter</param>
        /// <param name="status">The status boolean parameter for success </param>
        /// <param name="message">The message string parameter</param>
        void ExceptionLogging(DateTime startDate, DateTime endDate, bool status, Exception ex)
        {
            TimeSpan timeDiff = endDate - startDate;
            int ms = (int)timeDiff.TotalMilliseconds;

            long UserId = 0;
            if (HttpContext.Request.Headers.ContainsKey("userid"))
            {
                UserId = Convert.ToInt64(HttpContext.Request.Headers["userid"]);
            }
            var absoluteUri = string.Concat(
                        HttpContext.Request.Scheme,
                        "://",
                        HttpContext.Request.Host.ToUriComponent(),
                        HttpContext.Request.PathBase.ToUriComponent(),
                        HttpContext.Request.Path.ToUriComponent(),
                        HttpContext.Request.QueryString.ToUriComponent());



            ExceptionLogger exceptionLogger = new ExceptionLogger()
            {
                Message = ex.Message,
                Exception = ex.ToString(),
                UserId = loggedInUserId,
                LoggedDate = DateTime.Now,
                StackTrace = ex.StackTrace,
                Method = Convert.ToString(HttpContext.Request.Method),
                ModuleId = 1,
                Url = Convert.ToString(absoluteUri),
            };
            this.logger.ExceptionLog(exceptionLogger);

        }

    }
}