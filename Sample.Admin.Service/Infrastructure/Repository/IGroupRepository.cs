using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public interface IGroupRepository
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
        Task<GroupsVM> CreateGroup(GroupsModel group, int userId);

        /// <summary>
        /// To Update Group
        /// </summary>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        /// <returns></returns>
        Task<GroupsVM> UpdateGroup(long groupId, GroupsModel group, int userId);

        /// <summary>
        /// To Update Group Partially
        /// </summary>
        /// /// <param name="groupId">Group ID</param>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        Task<GroupsVM> UpdatePartialGroup(long groupId, GroupsModel group, int userId);

        /// <summary>
        /// To Delete Group
        /// </summary>
        /// <param name="groupId">The groupId to delete</param>
        /// <returns></returns>
        Task<long> DeleteGroup(long groupId);
    }
}
