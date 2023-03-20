using Common.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.IServices.UserManagement
{
    /// <summary>
    /// Group Service interface
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Group Service to get group list
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<GroupsVM>> GetAllGroups(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Group By Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<GroupsVM>> GetGroupById(long groupId);

        /// <summary>
        /// To Create new Group
        /// </summary>
        /// <param name="group">group object</param>
        /// <returns></returns>
        Task<ResponseResult<GroupsVM>> CreateGroup(GroupsModel group);

        /// <summary>
        /// To Update existing Group
        /// </summary>
        /// <param name="group">group object</param>
        /// /// <param name="groupId">Unique Group Id</param>
        /// <returns></returns>

        Task<ResponseResult<GroupsVM>> UpdateGroup(long groupId, GroupsModel group);

        /// <summary>
        /// To Update existing Group
        /// </summary>
        /// <param name="group">group object</param>
        /// /// <param name="groupId">Unique Group Id</param>
        /// <returns></returns>

        Task<ResponseResult<GroupsVM>> UpdatePartialGroup(long groupId, GroupsModel group);

        /// <summary>
        /// To Delete existing Group
        /// </summary>
        /// <param name="groupId">group identifier</param>
        /// <returns></returns>
        Task<ResponseResult<GroupsModel>> DeleteGroup(long groupId);
    }
}
