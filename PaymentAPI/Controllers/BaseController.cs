using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace PaymentAPI.Controllers
{
    /// <summary>
    /// Controller For handling base/common operations
    /// </summary>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Handle Internal Server Errror
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public InternalServerErrorObjectResult InternalServerError()
        {
            return new InternalServerErrorObjectResult();
        }

        /// <summary>
        /// Handle Internal Server Errror
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public InternalServerErrorObjectResult InternalServerError(object value)
        {
            return new InternalServerErrorObjectResult(value);
        }

        /// <summary>
        /// Get current user id
        /// </summary>
        public string UserId
        {
            get
            {
                return User?.Claims?.FirstOrDefault(x => x.Type == "Id")?.Value;
            }
        }

        /// <summary>
        /// Get current user email
        /// </summary>
        public string UserEmail
        {
            get
            {
                return User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            }
        }

        /// <summary>
        /// Get current user Role
        /// </summary>
        public string UserRole
        {
            get
            {
                return User.Claims.FirstOrDefault(x => x.Type == "RoleId").Value;
            }
        }
    }

    /// <summary>
    /// Internal Server error result
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Internal Server error result
        /// </summary>
        /// <param name="value"></param>
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        /// <summary>
        /// Internal Server error result
        /// </summary>
        public InternalServerErrorObjectResult() : this(null)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}