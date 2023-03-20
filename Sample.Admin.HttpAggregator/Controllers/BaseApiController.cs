using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Model;
using Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// BaseApiController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Logged In User Id
        /// </summary>
        public int loggedInUserId = 0;

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
                if (User != null && User.Claims != null && User.Claims.Count() > 0)
                {
                    loggedInUserId = Convert.ToInt32(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
                    HttpContext.Request.Headers.Add("UserId", loggedInUserId.ToString());
                }
                var result = action().GetAwaiter().GetResult();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message - {ex.Message}");
                Console.WriteLine($"StackTrace - {ex.StackTrace}");
                AuditLogging(StartDate, EndDate, false, ex.Message);
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

        #endregion
    }
}
