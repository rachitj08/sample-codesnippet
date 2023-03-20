using System.Threading.Tasks;
using Common.Model;
using Sample.Admin.Service.ServiceWorker;
using Sample.Admin.Model;
using Sample.Admin.Model.User;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Sample.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUsersController : BaseApiController
    {
        private readonly IAdminUserService adminUserService;

        /// <summary>
        /// Admin User Type Controller constructor to Inject dependency
        /// </summary>
        /// <param name="currencyService">Service for Admin User</param>
        public AdminUsersController(IAdminUserService adminUserService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(adminUserService), adminUserService);
            this.adminUserService = adminUserService;
        }

        /// <summary>
        /// adminUsers_list
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="search">Search Fields: (UserId, UserName, Email)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllAdminUsers([FromQuery] string ordering, [FromQuery] string search, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] int offset, [FromQuery] bool all)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.GetAllAdminUsers(ordering, search, pageSize, pageNumber, offset, all);
                return Ok(result);
            });

        }

        /// <summary>
        /// Information for Admin User 
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetAdminUserDetail([FromRoute] int userId)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.GetAdminUserDetail(userId);
                return Ok(result);
            });
        }


        /// <summary>
        /// This api is used for Creating new  Admin User
        /// </summary>
        /// <param name="adminUser">The new admin User object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateAdminUser([FromBody] UserCreationModel adminUser)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.CreateAdminUser(adminUser, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Updating existing admin user
        /// </summary>
        /// <param name="adminUser">The existing admin user object.</param>
        /// <param name="userId">A unique integer value identifying this admin user.</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateAdminUser([FromRoute] int userId, [FromBody] UserCreationModel adminUser)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.UpdateAdminUser(userId, adminUser, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// This api is used for Updating existing admin user partially
        /// </summary>
        /// <param name="adminUser">The existing admin user object.</param>
        /// <param name="moduleId">A unique integer value identifying this admin user .</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePartialAdminUser([FromRoute] int userId, [FromBody] UserCreationModel adminUser)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.UpdatePartialAdminUser(userId, adminUser, loggedInUserId);
                return Ok(result);
            });

        }

        /// <summary>
        /// This api is used for deleing Admin User
        /// </summary>
        /// <param name="userId">admin user identifier</param>
        /// <returns></returns>
        [Route("{userId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAdminUser([FromRoute] int userId)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.DeleteAdminUser(userId);
                return Ok(result);
            });
        }

        /// <summary>
        /// Change password of Admin User
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="OldPassword"></param>
        /// <returns>return response result</returns>
        [Route("ChangePassword")]
        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.ChangePassword(model, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Set password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("setPassword")]
        [HttpPost]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.SetPassword(model, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Reset password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("forgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.ForgotPassword(model);
                return Ok(result);
            });
        }

        /// <summary>
        ///  Verify token for Admin User
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        [Route("verify")]
        [HttpPost]
        public async Task<IActionResult> VerifyToken([FromBody] VerifyTokenModel tokenModel)
        {
            return await Execute(async () =>
            {
                var result = await this.adminUserService.VerifyToken(tokenModel);
                return Ok(result);
            });
        }
    }
}