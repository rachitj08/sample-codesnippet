using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using Common.Model;
using Common.Model.Model;

namespace Sample.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public int loggedInUserId;

        private readonly IFileLogger logger;

        private DateTime StartDate;

        private DateTime EndDate;

       
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BaseApiController()
        {
            // To Do-- Default constructors 

        }

        /// <summary>
        /// To Logger dependency inject
        /// </summary>
        /// <param name="logger"> To log into files</param>
        public BaseApiController(IFileLogger logger)
        {
            this.logger = logger;
        }

       
        /// <summary>
        /// Execute Method
        /// </summary>
        /// <param name="action"> For all the action results </param>
        /// <returns></returns>
        protected async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
        {
            try
            {
                if (HttpContext.Request.Headers.ContainsKey("userid"))
                {
                    loggedInUserId = Convert.ToInt32(HttpContext.Request.Headers["userid"]);
                }

                StartDate = DateTime.UtcNow;
                var result = action().GetAwaiter().GetResult();
                EndDate = DateTime.UtcNow;
                AuditLogging(StartDate, EndDate, true);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Admin API Exception :: " + ex.Message);
                Console.WriteLine("Admin API Stack trace :: " + ex.StackTrace);
                ExceptionLogging(StartDate, EndDate, false, ex);
                ResponseResult response = new ResponseResult();
                response.Error = new ErrorResponseResult() { Message = ex.Message };
                response.ResponseCode = ResponseCode.InternalServerError;
                return BadRequest(response);
            }
        }

        /// <summary>
        /// To Log  Messages for success or failure 
        /// </summary>
        /// <param name="startDate"> startdate in DateTime</param>
        /// <param name="endDate">enddate in DateTime</param>
        /// <param name="status"> Status for success or failure</param>
        /// <param name="message">message shown for log</param>
        void AuditLogging(DateTime startDate, DateTime endDate, bool status, string message = "success")
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



            
        }

    }
}