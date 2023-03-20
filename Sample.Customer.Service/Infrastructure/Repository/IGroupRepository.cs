using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Model;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public interface IGroupRepository
    {
        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<GroupsVM>> GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all, long accountId);

        /// <summary>
        /// Get Group By GroupID
        /// </summary>
        /// <returns></returns>
        Task<GroupsVM> GetGroupById(long groupId);

        /// <summary>
        /// Get Group By Name
        /// </summary>
        /// <returns></returns>
        Task<GroupsVM> GetGroupByName(string groupName, long accountId);

        /// <summary>
        /// Get Group By UserID
        /// </summary>
        /// <returns></returns>
        Task<List<Groups>> GetGroupByUserId(long userId);

        /// <summary>
        /// To Create New Group
        /// </summary>
        /// <param name="group">new Group object/param>
        /// <returns></returns>
        Task<GroupsVM> CreateGroup(GroupsModel group, long userId);

        /// <summary>
        /// To Create New Group
        /// </summary>
        /// <param name="group">new Group object</param>
        /// <returns> Group object</returns>
        Task<Groups> CreateGroup(Groups group);

        /// <summary>
        /// Get all Groups
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Groups>> GetAllGroups();
        /// <summary>
        /// Get Groups Rights By GroupId
        /// </summary>
        /// <param name="groupId">group identifier</param>
        /// <returns></returns>
        Task<IEnumerable<GroupRights>> GetGroupsRightsByGroupId(int groupId);

        /// <summary>
        /// Get Group Modules by user id
        /// </summary>
        /// <param name="userId"> user identifier</param>
        /// <returns></returns>
       Task<IEnumerable<GroupRights>> GetGroupModules(int userId);
        /// <summary>
        /// To Update Group
        /// </summary>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<GroupsVM> UpdateGroup(long groupId, GroupsModel group, long userId);

        /// <summary>
        /// To Update Group Partially
        /// </summary>
        /// /// <param name="groupId">Group ID</param>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        Task<GroupsVM> UpdatePartialGroup(long groupId, GroupsModel group, long userId);

        /// <summary>
        /// To Delete Group
        /// </summary>
        /// <param name="groupId">The groupId to delete</param>
        /// <returns></returns>
        Task<long> DeleteGroup(long groupId);

        /// <summary>
        /// CreateGroupRights
        /// </summary>
        /// <param name="groupRights"></param>
        /// <returns></returns>
        Task CreateGroupRights(List<GroupRights> groupRights);
    }
}
