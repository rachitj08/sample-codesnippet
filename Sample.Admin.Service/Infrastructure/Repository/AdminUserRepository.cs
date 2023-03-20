using AutoMapper;
using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Sample.Admin.Model.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    /// <summary>
    /// AdminUserRepository
    /// </summary>
    public class AdminUserRepository : RepositoryBase<AdminUsers>, IAdminUserRepository
    {
        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration configuration { get; }
        private readonly IMapper mapper;
        public AdminUserRepository(CloudAcceleratorContext context, IConfiguration configuration, IMapper mapper) : base(context)
        {
            //TODO
            this.configuration = configuration;
            this.mapper = mapper;
        }

        /// <summary>
        /// To Create  New User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        public async Task<AdminUsers> CreateUser(AdminUsers userModel)
        {

            base.context.AdminUsers.Add(userModel);
            return userModel;

        }
        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<AdminUsers> UpdateUser(AdminUsers user)
        {
            return base.Update(user);
        }

        /// <summary>
        /// To Get User By User Name
        /// </summary>
        /// <param name="userName">The userName to get user</param>
        /// <returns></returns>
        public async Task<AdminUsers> GetUserByUserName(string userName)
        {
            return await base.context.AdminUsers.FirstOrDefaultAsync(user => user.UserName.Equals(userName));
        }
        

        /// <summary>
        /// To Get User By User Name
        /// </summary>
        /// <param name="userId">The user Id to get user</param>
        /// <returns></returns>
        public async Task<AdminUsers> GetUserByUserId(int userId)
        {
            return await base.context.AdminUsers.FirstOrDefaultAsync(user => user.UserId.Equals(userId));
        }

        /// <summary>
        /// Get All AdminUsers
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="search"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="offset"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public async Task<ResponseResultList<AdminUsersModel>> GetAllAdminUsers(string ordering, string search, int pageSize, int pageNumber, int offset, bool all)
        {
            IQueryable<AdminUsers> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("");
            StringBuilder sbPrevious = new StringBuilder("");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string[] searchList = search.Split(",");

                foreach (string item in searchList)
                {
                    Int64.TryParse(item, out long id);

                    if (result == null)
                    {
                        result = from adminUsers in base.context.AdminUsers
                                 where adminUsers.UserId.ToString().ToLower().Contains(item.ToLower()) || adminUsers.UserName.ToLower().Contains(item.ToLower()) ||
                                 adminUsers.EmailAddress.ToLower().Contains(item.ToLower())
                                 select adminUsers;
                    }
                    else
                    {
                        result = result.Concat(from adminUsers in base.context.AdminUsers
                                               where adminUsers.UserId.ToString().ToLower().Contains(item.ToLower()) || adminUsers.UserName.ToLower().Contains(item.ToLower()) ||
                                                 adminUsers.EmailAddress.ToLower().Contains(item.ToLower())
                                               select adminUsers);
                    }
                }
            }
            else
            {
                result = from adminUsers in base.context.AdminUsers
                         select adminUsers;
            }

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
                if (typeof(AdminUsers).GetProperty(ordering) != null)
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
                result = result.OrderByDescending(x => x.UserId);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (!string.IsNullOrEmpty(sbNext.ToString()))
                    sbNext.Append("&search=" + search);

                if (!string.IsNullOrEmpty(sbPrevious.ToString()))
                    sbPrevious.Append("&search=" + search);
            }

            var adminUsersList = new List<AdminUsersModel>();
            if (result != null && result.Count() > 0)
            {
                adminUsersList = await result.Select(x => new AdminUsersModel()
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Mobile = x.Mobile,
                    EmailAddress = x.EmailAddress,
                    FailedLoginAttempts = x.FailedLoginAttempts,
                    PasswordExpirationDate = x.PasswordExpirationDate,
                    IsEmailVerified = x.IsEmailVerified,
                    IsMobileNumberVerified = x.IsMobileNumberVerified,
                    UserStatus = x.UserStatus,
                    AuthenticationCategory = x.AuthenticationCategory,
                    ExternalUserId = x.ExternalUserId,
                    MfatypeId = x.MfatypeId
                }).ToListAsync();
            }


            return new ResponseResultList<AdminUsersModel>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = adminUsersList
            };
        }

        /// <summary>
        /// Get Admin User Detail
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns></returns>
        public async Task<AdminUsersModel> GetAdminUserDetail(int userId)
        {
            var user = await base.context.AdminUsers
                   .Where(x => x.UserId == userId)
                   .Select(x => new AdminUsersModel()
                   {
                       UserId = x.UserId,
                       UserName = x.UserName,
                       FirstName = x.FirstName,
                       LastName = x.LastName,
                       Mobile = x.Mobile,
                       EmailAddress = x.EmailAddress,
                       FailedLoginAttempts = x.FailedLoginAttempts,
                       PasswordExpirationDate = x.PasswordExpirationDate,
                       IsEmailVerified = x.IsEmailVerified,
                       IsMobileNumberVerified = x.IsMobileNumberVerified,
                       UserStatus = x.UserStatus,
                       AuthenticationCategory = x.AuthenticationCategory,
                       ExternalUserId = x.ExternalUserId,
                       MfatypeId = x.MfatypeId
                   }).FirstOrDefaultAsync();

            if(user != null)
            {
                user.UserRights = await base.context.UserRights
                    .Where(x => x.UserId == userId)
                    .Select(x => new AdminUserRight()
                    {
                        UserRightId = x.UserRightId,
                        ModuleId = x.ModuleId,
                        IsPermission = x.IsPermission
                    }).ToListAsync();

                user.UserGroups = await base.context.UserGroupMappings
                    .Where(x => x.UserId == userId)
                    .Select(x => new AdminUserGroup()
                    {
                        UserGroupMappingId = x.UserGroupMappingId,
                        GroupId = x.GroupId,
                        UserId = x.UserId
                    }).ToListAsync();
            }
            return user;
        }

        /// <summary>
        /// To Get Admin User By User Id
        /// </summary>
        /// <param name="userId">The userId to get admin user</param>
        /// <returns>User</returns>
        public async Task<AdminUsers> GetAdminUserByUserId(int userId)
        {
            return await base.context.AdminUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// To Get Admin User ByAdmin User email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AdminUsers> GetAdminUserByEmailId(string email)
        {
            return await base.context.AdminUsers.Where(x => x.EmailAddress.Contains(email)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// To Create new Admin User
        /// </summary>
        /// <param name="module">Admin User object</param>
        public async Task<AdminUsers> CreateAdminUser(AdminUsers adminUser)
        {
            await base.context.AdminUsers.AddAsync(adminUser);
            await base.context.SaveChangesAsync();
            return adminUser;
        }


        /// <summary>
        /// To Create User Group Mappings
        /// </summary>
        /// <param name="adminUserGroups">Admin User Groups</param>
        /// <param name="updateUserId">Admin User Id</param>
        public async Task CreateUserGroupMappings(List<UserGroupMappings> adminUserGroups, int updateUserId = 0)
        {
            if (updateUserId > 0)
            {
                var userGroups = base.context.UserGroupMappings.Where(x => x.UserId == updateUserId);

                if (userGroups != null && userGroups.Count() > 0)
                {
                    base.context.UserGroupMappings.RemoveRange(userGroups);
                }
            }

            if (adminUserGroups != null && adminUserGroups.Count > 0)
                await base.context.UserGroupMappings.AddRangeAsync(adminUserGroups); 
        }

        /// <summary>
        /// To Create User Rights
        /// </summary>
        /// <param name="adminUserRights">Admin User Rights</param>
        /// <param name="updateUserId">Admin User Id</param>
        public async Task CreateUserRights(List<UserRights> adminUserRights, int updateUserId = 0)
        {
            if (updateUserId > 0)
            {
                var userRights = base.context.UserRights.Where(x => x.UserId == updateUserId);

                if (userRights != null && userRights.Count() > 0)
                {
                    base.context.UserRights.RemoveRange(userRights);
                }
            }

            if (adminUserRights != null && adminUserRights.Count > 0)
                await base.context.UserRights.AddRangeAsync(adminUserRights);
        }

        /// To Update Admin User Partially
        /// </summary>
        /// <param name="adminUser">New admin user object</param>
        /// <returns></returns>
        public async Task<AdminUsers> UpdateAdminUser(AdminUsers adminUser)
        {
            base.context.AdminUsers.Update(adminUser);           
            return adminUser;
        }

        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="User"></param>
        /// <returns>return Admin User</returns>
        public async Task<AdminUsers> ChangePassword(AdminUsers adminUser)
        {
            base.context.AdminUsers.Update(adminUser);
            await base.context.SaveChangesAsync();
            return adminUser;
        }

        /// <summary>
        /// To Delete Admin User
        /// </summary>
        /// <param name="userId">The userId to delete</param>
        /// <returns></returns>
        public async Task<int> DeleteAdminUser(int userId)
        {
            int result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var adminUser = await base.context.AdminUsers.FirstOrDefaultAsync(x => x.UserId == userId);

                if (adminUser != null)
                {

                    // Delete User Rights
                    var userRights = base.context.UserRights.Where(x => x.UserId == userId);

                    if (userRights != null && userRights.Count() > 0)
                    {
                        base.context.UserRights.RemoveRange(userRights);
                    }


                    // Delete User Group Mappings
                    var userGroups = base.context.UserGroupMappings.Where(x => x.UserId == userId);

                    if (userGroups != null && userGroups.Count() > 0)
                    {
                        base.context.UserGroupMappings.RemoveRange(userGroups);
                    }

                    //Delete that post
                    base.context.AdminUsers.Remove(adminUser);
                    //Commit the transaction
                    result = await base.context.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        /// <summary>
        /// To Verify Token
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        public async Task<Boolean> VerifyToken(VerifyTokenModel tokenModel)
        {
            var result = await base.context.LoginHistories.Where(x => x.Token == tokenModel.Token && x.IsActive == true).FirstOrDefaultAsync();

            if (result != null)
            {
                return true;
            }

            return false;
        }
        public async Task<AdminUsers> GetAdminUserByMobile(string mobile)
        {
            return await base.context.AdminUsers.FirstOrDefaultAsync(user => user.Mobile.Equals(mobile));
        }
        
    }
}
