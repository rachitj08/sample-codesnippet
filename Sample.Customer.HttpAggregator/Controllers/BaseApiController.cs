using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Model;
using Common.Model.Model;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Customer.HttpAggregator.Controllers
{
    /// <summary>
    /// Base Api Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Logged In User Id
        /// </summary>
        public long loggedInUserId = 0;


        /// <summary>
        /// Account Id
        /// </summary>
        public long headerAccountId = 0;
        /// <summary>
        /// 
        /// </summary>
        public long headerClientId = 0;

        #region [Private Member]
        /// <summary>
        /// logger Private member
        /// </summary>
        private IFileLogger logger;

        /// <summary>
        /// Start Date Private Member
        /// </summary>
       private DateTime StartDate;

        /// <summary>
        /// End Date  Private Member
        /// </summary>
       private DateTime EndDate;

        #endregion

        #region [Constructor]

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BaseApiController()
        {

        }

        /// <summary>
        /// To Logger dependency inject
        /// </summary>
        /// <param name="logger"></param>
        public BaseApiController(IFileLogger logger)
        {
            this.logger = logger;
        }

        #endregion


        #region [public Method]


        /// <summary>
        /// This execute function call in every controller to log activity and exception
        /// </summary>
        /// <param name="action">The action parameter for action result</param>
        /// <returns></returns>
        protected async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
        {
            try
            {
                if (!HttpContext.Request.Headers.ContainsKey("userId") && User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    string userId = User.FindFirst("UserId")?.Value;
                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        loggedInUserId = Convert.ToInt64(userId);
                        HttpContext.Request.Headers.Add("userid", loggedInUserId.ToString());
                    }
                }


                if (loggedInUserId < 1 && HttpContext.Request.Headers.ContainsKey("userId"))
                {
                    Int64.TryParse(HttpContext.Request.Headers["userId"], out loggedInUserId);
                }

                if (HttpContext.Request.Headers.ContainsKey("accountId"))
                {
                    Int64.TryParse(HttpContext.Request.Headers["accountId"], out headerAccountId);
                }
                if (HttpContext.Request.Headers.ContainsKey("clientid"))
                {
                    Int64.TryParse(HttpContext.Request.Headers["clientid"], out headerClientId);
                }

                StartDate = DateTime.UtcNow;
                var result = action().GetAwaiter().GetResult();
                EndDate = DateTime.UtcNow;
                AuditLogging(StartDate, EndDate, true);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Customer Aggregator Exception :: "+ex.Message);
                Console.WriteLine("Customer Aggregator Stack trace :: "+ex.StackTrace);
                ExceptionLogging(StartDate, EndDate, false, ex);
                return BadRequest(new ResponseResult()
                {
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ex.StackTrace,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                });
            }
        }

        /// <summary>
        /// To Log all activity and exception in the logger file
        /// </summary>
        /// <param name="startDate">The startDate datetime parameter</param>
        /// <param name="endDate">The EndDate datetime parameter</param>
        /// <param name="status">The status boolean parameter for success </param>
        /// <param name="message">The message string parameter</param>
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
        void ExceptionLogging(DateTime startDate, DateTime endDate, bool status,Exception ex)
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
       
        #endregion
    }
}
