using Common.Model;
using Sample.Customer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.ServiceWorker
{
    public interface IGroupService
    {
        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<GroupsVM>> GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all,long accountId);

        /// <summary>
        /// Get Group By GroupID
        /// </summary>
        /// <returns></returns>
        Task<GroupsVM> GetGroupById(long groupId);
        /// <summary>
        /// Get Group By UserID
        /// </summary>
        /// <returns></returns>
        Task<List<GroupsModel>> GetGroupByUserId(long userId);

        /// <summary>
        /// To Create New Group
        /// </summary>
        /// <param name="group">new Group object/param>
        /// <returns></returns>
        Task<ResponseResult<GroupsVM>> CreateGroup(GroupsModel group, long userId);

        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GroupsModel>> GetAllGroups();

        /// <summary>
        /// Get Groups Rights By GroupId
        /// </summary>
        /// <param name="groupId">group identifier</param>
        /// <returns></returns>
        Task<IEnumerable<GroupRightsModel>> GetGroupsRightsByGroupId(int groupId);

        /// <summary>
        /// Get Group Modules
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        Task<IEnumerable<GroupRightsModel>> GetGroupModules(int userId);

        /// <summary>
        /// To Update existing Group
        /// </summary>
        /// <param name="group">group object</param>
        /// <returns></returns>

        Task<ResponseResult<GroupsVM>> UpdateGroup(long groupId, GroupsModel group, long userId);

        /// <summary>
        /// To Update Group Partially
        /// </summary>
        /// /// <param name="groupId">Group ID</param>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        Task<ResponseResult<GroupsVM>> UpdatePartialGroup(long groupId, GroupsModel group, long userId);

        /// <summary>
        /// To Delete existing Group
        /// </summary>
        /// <param name="groupId">group identifier</param>
        /// <returns></returns>
        Task<long> DeleteGroup(long groupId);
    }
}
