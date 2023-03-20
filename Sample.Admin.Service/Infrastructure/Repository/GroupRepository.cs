using Sample.Admin.Service.Infrastructure.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Sample.Admin.Service.Helpers;
using Common.Model;
using System.Text;
using Sample.Admin.Model;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class GroupRepository : RepositoryBase<Groups>, IGroupRepository
    {
        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration configuration { get; }

        public GroupRepository(CloudAcceleratorContext context, IConfiguration configuration) : base(context)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<GroupsVM>> GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            IQueryable<Groups> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("");
            StringBuilder sbPrevious = new StringBuilder("");

            result = (from g in base.context.Groups
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
                    Name = x.Name,
                    Status = x.Status,
                    GroupRights = x.GroupRights.Select(y => new GroupRightsVM()
                    {
                        GroupRightId = y.GroupRightId,
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
                groupsVM.Name = group.Name;
                groupsVM.Status = group.Status;
                groupsVM.GroupRights = groupRight.Select(y => new GroupRightsVM()
                {
                    GroupRightId = y.GroupRightId,
                    ModuleId = y.ModuleId
                }).ToList();
            }
            return groupsVM;
        }

        /// <summary>
        /// To Create Group
        /// </summary>
        /// <param name="group">New Group Object</param>
        /// <returns></returns>
        public async Task<GroupsVM> CreateGroup(GroupsModel group, int userId)
        {
            var groupNew = new Groups
            {
                Description = group.Description,
                Name = group.Name,
                Status = group.Status,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = userId
            };
            base.context.Groups.Add(groupNew);
            await base.context.SaveChangesAsync();

            var groupsVM = new GroupsVM()
            {
                GroupId = groupNew.GroupId,
                Description = groupNew.Description,
                Name = groupNew.Name,
                Status = groupNew.Status
            };

            if (group.GroupRights != null && group.GroupRights.Count > 0)
            {
                var listGroupRights = group.GroupRights.Select(x=> new GroupRights
                {
                    GroupId = groupNew.GroupId,
                    ModuleId = x.ModuleId,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = userId
                }); 
                
                base.context.AddRange(listGroupRights);

                groupsVM.GroupRights = listGroupRights.Select(y => new GroupRightsVM()
                {
                    GroupRightId = y.GroupRightId,
                    ModuleId = y.ModuleId
                }).ToList();

                await base.context.SaveChangesAsync();
            }
            return groupsVM;
        }

        /// <summary>
        /// To Update Group
        /// </summary>
        /// <param name="group">new group object</param>
        /// <returns></returns>
        public async Task<GroupsVM> UpdateGroup(long groupId, GroupsModel groupNew, int userId)
        {
            var groupsVM = new GroupsVM();

            var group = await base.context.Groups.FindAsync(groupId);

            if (groupNew != null)
            {
                group.Name = groupNew.Name;
                group.Description = groupNew.Description;
                group.Status = groupNew.Status;
                group.UpdatedOn = DateTime.UtcNow;
                group.UpdatedBy = userId;

                var groupRights = await base.context.GroupRights.Where(x => x.GroupId == groupId).ToListAsync();
                if (groupRights != null && groupRights.Count > 0)
                {
                    base.context.GroupRights.RemoveRange(groupRights);
                }

                if (groupNew.GroupRights != null && groupNew.GroupRights.Count > 0)
                {
                    var listGroupRights = groupNew.GroupRights.Select(x => new GroupRights
                    {
                        GroupId = groupId,
                        GroupRightId = x.GroupRightId,
                        ModuleId = x.ModuleId,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                    }).ToList();
                     
                    base.context.AddRange(listGroupRights);

                    groupsVM.GroupRights = listGroupRights.Select(y => new GroupRightsVM()
                    {
                        GroupRightId = y.GroupRightId,
                        ModuleId = y.ModuleId
                    }).ToList();
                }

                await base.context.SaveChangesAsync();

                groupsVM.GroupId = groupId;
                groupsVM.Description = groupNew.Description;
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
        public async Task<GroupsVM> UpdatePartialGroup(long groupId, GroupsModel groupNew, int userId)
        {
            var groupsVM = new GroupsVM();
            var group = await base.context.Groups.FindAsync(groupId);

            if (groupNew != null)
            {
                group.Name = groupNew.Name;
                group.Description = groupNew.Description;
                group.Status = groupNew.Status;
                group.UpdatedOn = DateTime.UtcNow;
                group.UpdatedBy = userId;

                var groupRights = await base.context.GroupRights.Where(x => x.GroupId == groupId).ToListAsync();
                if (groupRights != null && groupRights.Count > 0)
                {
                    base.context.GroupRights.RemoveRange(groupRights);
                }

                if (groupNew.GroupRights != null && groupNew.GroupRights.Count > 0)
                {
                    var listGroupRights = groupNew.GroupRights.Select(x => new GroupRights
                    {
                        GroupId = groupId,
                        GroupRightId = x.GroupRightId,
                        ModuleId = x.ModuleId,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                    }).ToList();

                    base.context.AddRange(listGroupRights);

                    groupsVM.GroupRights = listGroupRights.Select(y => new GroupRightsVM()
                    {
                        GroupRightId = y.GroupRightId,
                        ModuleId = y.ModuleId
                    }).ToList();
                }

                await base.context.SaveChangesAsync();

                groupsVM.GroupId = groupId;
                groupsVM.Description = groupNew.Description;
                groupsVM.Name = groupNew.Name;
                groupsVM.Status = groupNew.Status;
            }
            return groupsVM;
        }

        /// <summary>
        /// To Delete Group
        /// </summary>
        /// <param name="groupId">The groupId to delete </param>
        /// <returns></returns>
        public async Task<long> DeleteGroup(long groupId)
        {
            long result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var group = await base.context.Groups.FirstOrDefaultAsync(x => x.GroupId == groupId);

                if (group != null)
                {
                    var groupRights = await base.context.GroupRights.Where(x => x.GroupId == groupId).ToListAsync();
                    if (groupRights != null && groupRights.Count > 0)
                    {
                        base.context.GroupRights.RemoveRange(groupRights); 
                    }

                    base.context.Groups.Remove(group); 

                    //Commit the transaction
                    result = await base.context.SaveChangesAsync();
                }
            }

            return result;
        }

    }
}
