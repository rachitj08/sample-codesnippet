using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sample.Customer.Service.ServiceWorker;
using Sample.Customer.Model;
using Utilities;

namespace Sample.Customer.API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : BaseApiController
    {
        private readonly IGroupService groupService;

        /// <summary>
        /// Module Controller constructor to Inject dependency
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
        public async Task<IActionResult> GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all = false)
        {
            return await Execute(async () =>
            {
                var result = await this.groupService.GetAllGroups(ordering, offset, pageSize, pageNumber, all,base.loggedInAccountId);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest(result);
            });

        }

        /// <summary>
        ///  Information for group by id
        /// </summary>
        /// <param name="groupId">A unique integer value identifying this Group.</param>
        /// <returns></returns>
        [Route("{groupId}")]
        [HttpGet]
        public async Task<IActionResult> GetGroupById([FromRoute] long groupId)
        {
            return await Execute(async () =>
            {
                var result = await this.groupService.GetGroupById(groupId);
                return Ok(result);
            });
        }

        /// <summary>
        /// GetGroupByUserId
        /// </summary>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        [Route("{userId}/GetGroupByUserId")]
        [HttpGet]
        public async Task<IActionResult> GetGroupByUserId([FromRoute] long userId)
        {
            return await Execute(async () =>
            {
                var result = await this.groupService.GetGroupByUserId(userId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for Creating new Group
        /// </summary>
        /// <param name="group">The new group object.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupsModel group)
        {
            return await Execute(async () =>
            {
                if (group.AccountId < 1 && loggedInAccountId > 0) group.AccountId = loggedInAccountId;
                var result = await this.groupService.CreateGroup(group, loggedInUserId);
                return Ok(result);
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
        public async Task<IActionResult> UpdateGroup([FromRoute] long groupId, [FromBody] GroupsModel group)
        {
            return await Execute(async () =>
            {
                if (group.AccountId < 1 && loggedInAccountId > 0) group.AccountId = loggedInAccountId;
                var result = await this.groupService.UpdateGroup(groupId, group, loggedInUserId);
                return Ok(result);
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
        public async Task<IActionResult> UpdatePartialGroup([FromRoute] long groupId, [FromBody] GroupsModel group)
        {
            return await Execute(async () =>
            {
                if (group.AccountId < 1 && loggedInAccountId > 0) group.AccountId = loggedInAccountId;
                var result = await this.groupService.UpdatePartialGroup(groupId, group, loggedInUserId);
                return Ok(result);
            });
        }

        /// <summary>
        /// This api is used for deleing  Group
        /// </summary>
        /// <param name="groupId">group identifier</param>
        /// <returns></returns>
        [Route("{groupId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup([FromRoute] long groupId)
        {
            return await Execute(async () =>
            {
                var result = await this.groupService.DeleteGroup(groupId);
                return Ok(result);
            });
        }
    }
}
