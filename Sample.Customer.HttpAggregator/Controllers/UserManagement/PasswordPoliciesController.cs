using Common.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using System;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Utilities;

namespace Sample.Customer.HttpAggregator.Controllers.UserManagement
{
    /// <summary>
    /// Get Password Policies
    /// </summary>
    [Route("api/passwordPolicies")]
    [ApiController]
    public class PasswordPoliciesController : BaseApiController
    {
        #region [Private Variables]
        /// <summary>
        /// Password Policy Service private variable
        /// 
        /// </summary>
        private readonly IPasswordPolicyService passwordPolicyService;
        #endregion

        #region [Constructor]
        /// <summary>
        /// /
        /// </summary>
        /// <param name="passwordPolicyService"></param>
        /// <param name="logger"></param>
        public PasswordPoliciesController(IPasswordPolicyService passwordPolicyService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(passwordPolicyService), passwordPolicyService);
            this.passwordPolicyService = passwordPolicyService;
        }
        #endregion

        /// <summary>
        /// Get Password Policy
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<PasswordPolicyModel>), 200)]
        [HasPermission(PermissionType.View)]
        public async Task<IActionResult> Get()
        {
            if (!HttpContext.Request.Headers.ContainsKey("accountId")
                       || !Int64.TryParse(HttpContext.Request.Headers["accountId"], out var accountId))
            {
                return BadRequest(new ResponseResult<PasswordPolicyModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                });
            }

            return await Execute(async () =>
            {
                var result = await this.passwordPolicyService.GetPasswordPolicyByAccountId(accountId);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        /// This api is used for Creating new password policy
        /// </summary>
        /// <param name="model">The new passwordpolicy object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<PasswordPolicyModel>), 201)]
        [HasPermission(PermissionType.Add)]
        public async Task<IActionResult> CreateModule([FromBody] PasswordPolicyModel model)
        {
            return await Execute(async () =>
            {
                if (model != null)
                {
                    var result = await this.passwordPolicyService.CreatePasswordPolicy(model);
                    if (result != null)
                        return Created("api/passwordpolicies/", result);
                    else
                        return BadRequest(result);
                }

                return BadRequest(new ResponseResult<PasswordPolicyModel>
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                });

            });

        }

        /// <summary>
        /// This api is used for Updating existing Password policy
        /// </summary>
        /// <param name="model">The existing password policy object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult<PasswordPolicyModel>), 200)]
        [HasPermission(PermissionType.Edit)]
        public async Task<IActionResult> UpdateModule([FromBody] PasswordPolicyModel model)
        {
            return await Execute(async () =>
            {
                var result = await this.passwordPolicyService.UpdatePasswordPolicy(model);
                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordSaved)
                        return Ok(result);
                    else
                        return BadRequest(result);
                }
                else
                {
                    return BadRequest(new ResponseResult<PasswordPolicyModel>
                    {
                        ResponseCode = ResponseCode.InternalServerError,
                        Message = ResponseMessage.InternalServerError,
                        Error = new ErrorResponseResult
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    });
                }
            });

        }
    }
}
