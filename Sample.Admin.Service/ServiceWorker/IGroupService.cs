using Common.Model;
using Sample.Admin.Service.Helpers;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public interface IGroupService
    {
        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<GroupsVM>> GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get Group By GroupID
        /// </summary>
        /// <returns></returns>
        Task<GroupsVM> GetGroupById(long groupId);

        /// <summary>
        /// To Create New Group
        /// </summary>
        /// <param name="group">new Group object/param>
        /// <returns></returns>
        Task<ResponseResult<GroupsVM>> CreateGroup(GroupsModel group, int loggedInUserId);

        /// <summary>
        /// To Update existing Group
        /// </summary>
        /// <param name="group">group object</param>
        /// <returns></returns>

        Task<ResponseResult<GroupsVM>> UpdateGroup(long groupId, GroupsModel group, int loggedInUserId);

        /// <summary>
        /// To Update Group Partially
        /// </summary>
        /// /// <param name="groupId">Group ID</param>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        Task<ResponseResult<GroupsVM>> UpdatePartialGroup(long groupId, GroupsModel group, int loggedInUserId);

        /// <summary>
        /// To Delete existing Group
        /// </summary>
        /// <param name="groupId">group identifier</param>
        /// <returns></returns>
        Task<long> DeleteGroup(long groupId);
    }
}
