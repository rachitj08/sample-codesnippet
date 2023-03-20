using Common.Model;
using Sample.Customer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class GroupRepository : RepositoryBase<Groups>, IGroupRepository
    {
        public IConfiguration configuration { get; }
        public GroupRepository(CloudAcceleratorContext context, IConfiguration configuration) : base(context)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<GroupsVM>> GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all, long accountId)
        {
            IQueryable<Groups> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("");
            StringBuilder sbPrevious = new StringBuilder("");

            result = (from g in base.context.Groups
                      where g.AccountId == accountId
                      select g);

            if (!all)
            {
                listCount = result.Count();

                var rowIndex = 0;

                if (pageNumber > 0)
                {
                    rowIndex = (pageNumber - 1) * pageSize;
                    if (((pageNumber + 1) * pageSize) <= listCount)
                        sbNext.Append("pageNumber=" + (pageNumber + 1) + "&pageSize=" + pageSize);

                    if (pageNumber > 1)
                        sbPrevious.Append("pageNumber=" + (pageNumber - 1) + "&pageSize=" + pageSize);
                }
                else if (offset > 0)
                {
                    rowIndex = offset;

                    if ((offset + pageSize + 1) <= listCount)
                        sbNext.Append("offset=" + (offset + pageSize) + "&pageSize=" + pageSize);

                    if ((offset - pageSize) > 0)
                        sbPrevious.Append("offset=" + (offset - pageSize) + "&pageSize=" + pageSize);
                }
                else
                {
                    if (pageSize < listCount)
                        sbNext.Append("pageNumber=" + (rowIndex + 1) + "&pageSize=" + pageSize);
                }

                result = result.Skip(rowIndex).Take(pageSize);
            }
            else
            {
                listCount = result.Count();
                sbNext.Append("all=" + all);
                sbPrevious.Append("all=" + all);
            }

            if (!string.IsNullOrWhiteSpace(ordering))
            {
                ordering = string.Concat(ordering[0].ToString().ToUpper(), ordering.AsSpan(1));
                if (typeof(Groups).GetProperty(ordering) != null)
                {
                    result = result.OrderBy(m => EF.Property<object>(m, ordering));
                    if (!string.IsNullOrEmpty(sbNext.ToString()))
                        sbNext.Append("&ordering=" + ordering);

                    if (!string.IsNullOrEmpty(sbPrevious.ToString()))
                        sbPrevious.Append("&ordering=" + ordering);
                }
            }
            else
            {
                result = result.OrderByDescending(x => x.GroupId);
            }

            var groupsNew = new List<GroupsVM>();
            if (result != null && result.Count() > 0)
            {
                List<GroupRightsVM> listGroupsRights = new List<GroupRightsVM>();
                groupsNew = await result.Select(x => new GroupsVM()
                {
                    GroupId = x.GroupId,
                    Description = x.Description,
                    AccountId = x.AccountId,
                    Name = x.Name,
                    Status = x.Status,
                    GroupRights = x.GroupRights.Select(y => new GroupRightsVM()
                    {
                        GroupRightId = y.GroupRightId,
                        AccountId = y.AccountId,
                        ModuleId = y.ModuleId
                    }).ToList()
                }).ToListAsync();
            }

            return new ResponseResultList<GroupsVM>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = groupsNew,
            };
        }

        /// <summary>
        /// Get Group By Id
        /// </summary>
        /// <returns></returns> 
        public async Task<GroupsVM> GetGroupById(long groupId)
        {
            var group = await (from groups in base.context.Groups
                               where groups.GroupId == groupId
                               select groups
                                ).FirstOrDefaultAsync();

            var groupRight = await (from groupRights in base.context.GroupRights
                                    where groupRights.GroupId == groupId
                                    select groupRights
                                ).ToListAsync();

            GroupsVM groupsVM = new GroupsVM();
            if (group != null)
            {
                List<GroupRightsVM> listGroupsRights = new List<GroupRightsVM>();
                groupsVM.GroupId = group.GroupId;
                groupsVM.Description = group.Description;
                groupsVM.AccountId = group.AccountId;
                groupsVM.Name = group.Name;
                groupsVM.Status = group.Status;
                groupsVM.GroupRights = groupRight.Select(y => new GroupRightsVM()
                {
                    GroupRightId = y.GroupRightId,
                    AccountId = y.AccountId,
                    ModuleId = y.ModuleId
                }).ToList();
            }
            return groupsVM;
        }

        /// <summary>
        /// Get Group By Name
        /// </summary>
        /// <returns></returns>
        public async Task<GroupsVM> GetGroupByName(string groupName, long accountId)
        {
            var group = await (from groups in base.context.Groups
                               where groups.AccountId == accountId && groups.Name.Trim() == groupName.Trim()
                               select groups
                                ).FirstOrDefaultAsync();
             
            var groupsVM = new GroupsVM();
            if (group != null)
            { 
                groupsVM.GroupId = group.GroupId;
                groupsVM.Description = group.Description;
                groupsVM.AccountId = group.AccountId;
                groupsVM.Name = group.Name;
                groupsVM.Status = group.Status; 
            }
            return groupsVM;
        }

        /// <summary>
        /// GetGroupByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Groups>> GetGroupByUserId(long userId)
        {
            var group = await base.context.Groups.Join(base.context.UserGroupMappings, x => x.GroupId, y => y.GroupId, (x, y) => new { Group = x, UserGroup = y })
                .Where(x => x.UserGroup.UserId == userId).Select(x => x.Group).ToListAsync();
            return group;
        }

        /// <summary>
        /// To Create New Group
        /// </summary>
        /// <param name="group">new Group object</param>
        /// <returns> Group object</returns>
        public async Task<Groups> CreateGroup(Groups objGroup)
        {
            return base.Create(objGroup);
        }
        /// <summary>
        /// CreateGroupRights
        /// </summary>
        /// <param name="groupRights"></param>
        /// <returns></returns>
        public async Task CreateGroupRights(List<GroupRights> groupRights)
        {
            base.context.GroupRights.AddRange(groupRights);
        }

        /// <summary>
        /// To Create Group
        /// </summary>
        /// <param name="group">New Group Object</param>
        /// <returns></returns>
        public async Task<GroupsVM> CreateGroup(GroupsModel group, long userId)
        {
            var groupNew = new Groups
            {
                Description = group.Description,
                AccountId = group.AccountId,
                Name = group.Name,
                Status = group.Status,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = userId
            };

            base.context.Groups.Add(groupNew);
            var result = await base.context.SaveChangesAsync();
            var groupsVM = new GroupsVM();

            if (result > 0)
            {
                groupsVM.GroupId = groupNew.GroupId;
                groupsVM.Description = groupNew.Description;
                groupsVM.AccountId = groupNew.AccountId;
                groupsVM.Name = groupNew.Name;
                groupsVM.Status = groupNew.Status;

                if (group.GroupRights != null && group.GroupRights.Count > 0)
                {
                    var listGroupRights = group.GroupRights
                        .Select(x => new GroupRights()
                        {
                            GroupId = groupNew.GroupId,
                            AccountId = groupNew.AccountId,
                            ModuleId = x.ModuleId,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = userId
                        });

                    base.context.AddRange(listGroupRights);
                    result = await base.context.SaveChangesAsync();
                    if (result > 0)
                    {
                        groupsVM.GroupRights = listGroupRights.Select(y => new GroupRightsVM()
                        {
                            GroupRightId = y.GroupRightId,
                            AccountId = y.AccountId,
                            ModuleId = y.ModuleId
                        }).ToList();
                    }
                }
            }
            return groupsVM;
        }

        /// <summary>
        /// For getting all groups 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Groups>> GetAllGroups()
        {
            return base.context.Groups.ToList();
        }

        /// <summary>
        /// Get Groups Rights By their GroupId
        /// </summary>
        /// <param name="groupId"> Group Identifier </param>
        /// <returns></returns>
        public Task<IEnumerable<GroupRights>> GetGroupsRightsByGroupId(int groupId)
        {
            return (Task<IEnumerable<GroupRights>>)base.context.GroupRights.Where(group => group.GroupId == groupId);
        }

        /// <summary>
        /// Get Group Modules by user id
        /// </summary>
        /// <param name="userId"> user identifier</param>
        /// <returns></returns>
        public Task<IEnumerable<GroupRights>> GetGroupModules(int userId)
        {
            return (Task<IEnumerable<GroupRights>>)base.context.GroupRights.Where(grouprights => grouprights.AccountId == userId);
        }


        /// <summary>
        /// To Update Group
        /// </summary>
        /// <param name="group">new group object</param>
        /// <returns></returns>
        public async Task<GroupsVM> UpdateGroup(long groupId, GroupsModel groupNew, long userId)
        {
            var group = await base.context.Groups.FindAsync(groupId);

            var groupsVM = new GroupsVM();

            if (groupNew != null)
            {
                group.Name = groupNew.Name;
                group.Description = groupNew.Description;
                //group.AccountId = groupNew.AccountId;
                group.Status = groupNew.Status;
                group.UpdatedOn = DateTime.UtcNow;
                group.UpdatedBy = userId;
                base.context.Update(group);

                // Group Rigths
                var groupRightList = await base.context.GroupRights.Where(x => x.GroupId == groupId).ToListAsync();
                if (groupRightList != null && groupRightList.Count > 0)
                    base.context.RemoveRange(groupRightList);

                if (groupNew.GroupRights != null && groupNew.GroupRights.Count > 0)
                {
                    var listGroupRights = groupNew.GroupRights
                        .Select(x => new GroupRights()
                        {
                            GroupId = groupId,
                            GroupRightId = x.GroupRightId,
                            AccountId = groupNew.AccountId,
                            ModuleId = x.ModuleId,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = userId,
                            UpdatedOn = DateTime.UtcNow,
                            UpdatedBy = userId
                        });
                    base.context.UpdateRange(listGroupRights);
                    groupsVM.GroupRights = listGroupRights.Select(y => new GroupRightsVM()
                    {
                        GroupRightId = y.GroupRightId,
                        AccountId = y.AccountId,
                        ModuleId = y.ModuleId
                    }).ToList();
                }
                await base.context.SaveChangesAsync();

                groupsVM.GroupId = groupId;
                groupsVM.Description = groupNew.Description;
                groupsVM.AccountId = groupNew.AccountId;
                groupsVM.Name = groupNew.Name;
                groupsVM.Status = groupNew.Status;
            }
            return groupsVM;
        }

        /// <summary>
        /// To Update Group Partially
        /// </summary>
        /// /// <param name="groupId">Group ID</param>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        public async Task<GroupsVM> UpdatePartialGroup(long groupId, GroupsModel groupNew, long userId)
        {
            return await UpdateGroup(groupId, groupNew, userId);
        }

        /// <summary>
        /// To Delete Group
        /// </summary>
        /// <param name="groupId">The groupId to delete </param>
        /// <returns></returns>
        public async Task<long> DeleteGroup(long groupId)
        {
            long result = 0;
            var group = await base.context.Groups.FirstOrDefaultAsync(x => x.GroupId == groupId);

            if (group != null)
            {
                // Due to foregin key reference in UserGroupMappings we delete group record from User Group Mappings tabel
                var lstgroupMappingData = await base.context.UserGroupMappings.Where(x => x.GroupId == groupId).ToListAsync();
                if (lstgroupMappingData != null && lstgroupMappingData.Count > 0)
                {
                    base.context.UserGroupMappings.RemoveRange(lstgroupMappingData);
                }

                // Due to foregin key reference in GroupRights we delete group record from GroupRights tabel
                var groupRights = await base.context.GroupRights.Where(x => x.GroupId == groupId).ToListAsync();
                if (groupRights != null && groupRights.Count > 0)
                {
                    base.context.GroupRights.RemoveRange(groupRights);
                }

                // Delete record from group table
                base.context.Groups.Remove(group);

                //Commit the transaction
                result = await base.context.SaveChangesAsync();
            }

            return result;
        }

    }
}
