using Sample.Admin.HttpAggregator.IServices;
using Common.Model;
using Sample.Admin.Model;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Groups Controller
    /// </summary>
    [Route("api/groups")]
    [ApiController]
    [Authorize]
    public class GroupController : BaseApiController
    {

        private readonly IGroupService groupService;

        /// <summary>
        /// Group Controller constructor to Inject dependency
        /// </summary>
        /// <param name="groupService">group service </param>
        /// <param name="logger">logger service </param>
        public GroupController(IGroupService groupService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(groupService), groupService);
            this.groupService = groupService;
        }

        /// <summary>
        ///  Information for all existing groups
        /// </summary>
        /// <param name="ordering">Ordering Fields: (All Fields)</param>
        /// <param name="offset">The initial index from which to return the results.</param>
        /// <param name="pageSize">Number of results to return per page.</param>
        /// <param name="pageNumber">Page Number of results.</param>
        /// <param name="all">It will return results (all pages) without pagination.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResultList), 200)]
        public async Task<IActionResult> GetAllGroups([FromQuery] string ordering, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] bool all = false)
        {
            return await Execute(async () =>
            {
                var groups = await groupService.GetAllGroups(HttpContext, ordering, offset, pageSize, pageNumber, all);
                if (groups.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(groups);
                else
                    return BadRequest(groups);
            });

        }

        /// <summary>
        ///  Information for group by id
        /// </summary>
        /// <param name="groupId">A unique integer value identifying this Group.</param>
        /// <returns></returns>
        [Route("{groupId}")]
        [HttpGet]
        [ProducesResponseType(typeof(ResponseResult<GroupsModel>), 200)]
        public async Task<IActionResult> GetGroupById([FromRoute] long groupId)
        {
            return await Execute(async () =>
            {
                var result = await this.groupService.GetGroupById(groupId);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Group
        /// </summary>
        /// <param name="group">The new group object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult<GroupsVM>), 201)]
        public async Task<IActionResult> CreateGroup([FromBody] GroupsModel group)
        {

            return await Execute(async () =>
            {
                if (group != null)
                {
                    var result = await this.groupService.CreateGroup(group);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Created("api/groups/", result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<GroupsVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<GroupsVM>
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
        /// This api is used for Updating existing Group
        /// </summary>
        /// <param name="group">The existing group object.</param>
        /// <param name="groupId">A unique integer value identifying this Group.</param>
        /// <returns></returns>
        [Route("{groupId}")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult<GroupsVM>), 200)]
        public async Task<IActionResult> UpdateGroup([FromRoute] long groupId, [FromBody] GroupsModel group)
        {
            return await Execute(async () =>
            {
                if (groupId != 0 && group != null)
                {
                    var result = await this.groupService.UpdateGroup(groupId, group);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<GroupsVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult<GroupsVM>
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
        /// This api is used for Updating existing Group partially
        /// </summary>
        /// <param name="group">The existing group object.</param>
        /// <param name="groupId">A unique integer value identifying this Group.</param>
        /// <returns></returns>
        [Route("{groupId}")]
        [HttpPatch]
        [ProducesResponseType(typeof(ResponseResult<GroupsVM>), 200)]
        public async Task<IActionResult> UpdatePartialGroup([FromRoute] long groupId, [FromBody] GroupsModel group)
        {
            return await Execute(async () =>
            {
                if (groupId != 0 && group != null)
                {
                    var result = await this.groupService.UpdatePartialGroup(groupId, group);
                    if (result != null)
                    {
                        if (result.ResponseCode == ResponseCode.RecordSaved)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result);
                        }
                    }
                    else
                    {
                        return BadRequest(new ResponseResult<GroupsVM>
                        {
                            ResponseCode = ResponseCode.InternalServerError,
                            Message = ResponseMessage.InternalServerError,
                            Error = new ErrorResponseResult
                            {
                                Message = ResponseMessage.InternalServerError
                            }
                        });
                    }
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });

            });

        }


        /// <summary>
        /// This api is used for deleing  Group
        /// </summary>
        /// <param name="groupId">A unique integer value identifying this Group.</param>
        /// <returns></returns>
        [Route("{groupId}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> DeleteGroup([FromRoute] long groupId)
        {

            return await Execute(async () =>
            {
                var result = await this.groupService.DeleteGroup(groupId);

                if (result != null)
                {
                    if (result.ResponseCode == ResponseCode.RecordDeleted)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }

                return BadRequest(new ResponseResult
                {
                    ResponseCode = ResponseCode.NoRecordFound,
                    Message = ResponseMessage.NoRecordFound,
                    Error = new ErrorResponseResult
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                });
            });

        }

    }
}
