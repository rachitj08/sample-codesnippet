using Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator
{
    /// <summary>
    /// Has Permission Attribute 
    /// </summary>

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]

    public class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permissionName;
        private readonly PermissionType _permissionType;

        /// <summary>
        /// Has Permission Attribute
        /// </summary>
        /// <param name="permissionType"></param>
        /// <param name="permissionName"></param>
        public HasPermissionAttribute(PermissionType permissionType = PermissionType.None, string permissionName = "")
        {
            _permissionName = permissionName;
            _permissionType = permissionType;
        }

        /// <summary>
        /// On Authorization
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_permissionType == PermissionType.None && string.IsNullOrWhiteSpace(_permissionName)) return;
            
            var services = context.HttpContext.RequestServices;
            var userService = (IUserService)services.GetService(typeof(IUserService));

            long userId = 0, accountId = 0;
            if (context.HttpContext.User != null && context.HttpContext.User.Claims != null && context.HttpContext.User.Claims.Count() > 0)
            {
                var id = context.HttpContext.User.FindFirst("UserId")?.Value;
                if (!string.IsNullOrWhiteSpace(id)) {
                    userId = Convert.ToInt64(id);
                    context.HttpContext.Request.Headers["userId"] = userId.ToString();
                }
               
            }

            if (userId < 1 && context.HttpContext.Request.Headers.ContainsKey("userId"))
            {
                Int64.TryParse(context.HttpContext.Request.Headers["userId"], out userId);
            }

            if (userId < 1 || !context.HttpContext.Request.Headers.ContainsKey("accountId") || 
                !Int64.TryParse(context.HttpContext.Request.Headers["accountId"], out accountId) || accountId < 1) {
                context.Result = new BadRequestObjectResult(new ResponseResult()
                {
                    Data = null,
                    ResponseCode = ResponseCode.Unauthorized,
                    Message = ResponseMessage.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.Unauthorized
                    }
                });
                return;
            }

            var moduleName = _permissionName;
            if (string.IsNullOrWhiteSpace(moduleName) && context.HttpContext.Request.RouteValues.ContainsKey("controller"))
            {
                var controllerName = Convert.ToString(context.HttpContext.Request.RouteValues["controller"]);
                moduleName = Enum.GetName(typeof(PermissionType), _permissionType) + "_" + controllerName;
            }

            Task.Run(async () => await userService.CheckUserPermission(userId, accountId, moduleName));

        }
    }


    /// <summary>
    /// Permission Types
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// No Permission
        /// </summary>
        None,

        /// <summary>
        /// View Right
        /// </summary>
        View,

        /// <summary>
        /// Create Right
        /// </summary>
        Add,

        /// <summary>
        /// Update Right
        /// </summary>
        Edit,

        /// <summary>
        /// Delete Right
        /// </summary>
        Delete
    }
}


