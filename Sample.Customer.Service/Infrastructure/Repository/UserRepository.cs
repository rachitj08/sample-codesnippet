using AutoMapper;
using Common.Model;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;
using User = Sample.Customer.Service.Infrastructure.DataModels.Users;
using VerifyTokenModel = Sample.Customer.Model.VerifyTokenModel;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
     

        private readonly IMapper mapper;

        public UserRepository(CloudAcceleratorContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

     
        /// <summary>
        /// Get all users Group Mappings
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserGroupMappings>> GetAllUsersGroupMappings()
        {
            var userGroupMappings = await base.context.UserGroupMappings.ToListAsync();
            return userGroupMappings;
        }

        /// <summary>
        /// Get Users Groups By UserId
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<List<UserGroup>> GetUserGroupsByUserId(long userId)
        {

            var userGroups = from usergroups in base.context.UserGroupMappings
                         join groups in base.context.Groups on usergroups.GroupId equals groups.GroupId                        
                         where usergroups.UserId == userId
                         select new UserGroup
                         {
                             GroupId = usergroups.GroupId,
                             GroupName = groups.Description,
                         };
            return await userGroups.ToListAsync();


        }

       
        /// <summary>
        /// Get User group Rights by User Id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>List<long></returns>
        public async Task<List<long>> GetUserGroupRights(long userId, long accountId)
        {

            var userNavigations = from userGroups in base.context.UserGroupMappings
                                  join groups in base.context.Groups on userGroups.GroupId equals groups.GroupId
                                  join groupRights in base.context.GroupRights on groups.GroupId equals groupRights.GroupId
                                  where userGroups.UserId == userId && userGroups.AccountId == accountId
                                  select groupRights.ModuleId;
            return await userNavigations.ToListAsync();
        }

        /// <summary>
        /// Get User Rights by user id
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>List<UserRightDetail></returns>
        public async Task<List<UserRightDetail>> GetUserRights(long userId, long accountId)
        {
            return await base.context.UserRights.Where(x => x.UserId == userId && x.AccountId == accountId)
                .Select(x => new UserRightDetail()
                {
                    ModuleId = x.ModuleId,
                    IsPermission = x.IsPermission
                }).ToListAsync();
        }

        /// <summary>
        /// Check Module For User Group Right
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>List<long></returns>
        public async Task<int> CheckModuleForUserGroup(long userId, long moduleId)
        {

            var userNavigations = from userGroups in base.context.UserGroupMappings
                                  join groups in base.context.Groups on userGroups.GroupId equals groups.GroupId
                                  join groupRights in base.context.GroupRights on groups.GroupId equals groupRights.GroupId
                                  where userGroups.UserId == userId && groupRights.ModuleId == moduleId
                                  select groupRights.ModuleId;
            return await userNavigations.CountAsync();
        }

        /// <summary>
        /// Check Module For User Right
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <param name="userId">module Id</param>
        /// <returns>List<UserRightDetail></returns>
        public async Task<int> CheckModuleForUserRight(long userId, long moduleId)
        {
            return await base.context.UserRights.Where(x => x.UserId == userId && x.ModuleId == moduleId).CountAsync();
        }

        /// <summary>
        /// To Create  New User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        public Task<Users> CreateDefaultUser(Users userModel)
        {
            var user = base.context.Users.Add(userModel);
            return Task.FromResult(userModel);
        }

        /// <summary>
        /// To Create  New User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        public Task<Users> CreateUser(Users userModel)
        {
            var user = base.context.Users.Add(userModel);
            base.context.SaveChanges();
            return Task.FromResult(userModel);
        }

        /// <summary>
        /// To Get User By User Name
        /// </summary>
        /// <param name="userName">The userName to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns></returns>
        public async Task<User> GetUserByUserName(string userName, long accountId)
        {
            return await base.context.Users.FirstOrDefaultAsync(user => user.UserName==userName && user.AccountId == accountId);
        }
        
        //added by Raj for validate Mobile
        public async Task<User> GetUserByUserMobile(string mobile, long accountId)
        {
            return await base.context.Users.FirstOrDefaultAsync(user => user.Mobile == mobile && user.AccountId == accountId);
        }

        /// <summary>
        /// To Get User details with rights By User Name
        /// </summary>
        /// <param name="accountId">The accountId to get user</param>
        /// <param name="userName">The userName to get user</param>
        /// <returns></returns>
        public async Task<User> GetUserWithRightsByUserName(long accountId, string userName)
        {
            return await base.context.Users.Include(s => s.UserRights)
                    .Include(s => s.UserGroupMappings).ThenInclude(x => x.Group)
                    .FirstOrDefaultAsync(user => user.AccountId.Equals(accountId) && user.UserName.Equals(userName));
        }

        /// <summary>
        /// To Get User By EmailId
        /// </summary>
        /// <param name="emailId">The emailId to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns></returns>
        public async Task<User> GetUserByEmailId(string emailId, long accountId)
        {
            return await base.context.Users.FirstOrDefaultAsync(user => user.EmailAddress == emailId && user.AccountId == accountId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<User> GetUserByMobile(string mobile, long accountId)
        {
            return await base.context.Users.FirstOrDefaultAsync(user => user.Mobile == mobile && user.AccountId == accountId);
        }

        /// <summary>
        /// To Get User By User Id
        /// </summary>
        /// <param name="userId">The userId to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns>User</returns>
        public async Task<User> GetUserByUserId(long accountId, long userId)
        {
            return await base.context.Users.FirstOrDefaultAsync(user => user.UserId == userId && user.AccountId == accountId);
        }

        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="User"></param>
        /// <returns>return User</returns>
        public async Task<Users> ChangePassword(Users user)
        {
            return base.Update(user);
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        public Task<ResponseResultList<UserVM>> GetAllUsers(long accountId, string ordering, int offset, int pageSize, int pageNumber, bool all)
        { 
            int listCount;
            //if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("/users/");
            StringBuilder sbPrevious = new StringBuilder("/users/");
            sbNext.Append("?");
            sbPrevious.Append("?");

            var result = from users in base.context.Users
                             .Include(x => x.UserGroupMappings)
                             .Include(x => x.UserRights)
                         where users.AccountId == accountId
                         select users;

            if (!all)
            { 
                listCount = result.Count();
                var rowIndex = 0;

                if (pageNumber > 0)
                {
                    rowIndex = (pageNumber - 1) * pageSize;
                    sbNext.Append("PageNumber=" + (pageNumber + 1) + "&PageSize=" + pageSize);
                    sbPrevious.Append("PageNumber=" + (pageNumber - 1) + "&PageSize=" + pageSize);
                }
                else if (offset > 0)
                {
                    rowIndex = offset;
                    sbNext.Append("PageNumber=" + (offset + pageSize) + "&PageSize=" + pageSize);
                    sbPrevious.Append("PageNumber=" + (offset - pageSize) + "&PageSize=" + pageSize);
                }
                else
                {
                    rowIndex = 0;
                    sbNext.Append("PageNumber=" + (rowIndex + 1) + "&PageSize=" + pageSize);
                   // sbPrevious.Append("PageNumber=" + (rowIndex - 1) + "&PageSize=" + pageSize);
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
                if (typeof(User).GetProperty(ordering) != null)
                {
                    result = result.OrderBy(m => EF.Property<object>(m, ordering));
                    sbNext.Append("&ordering=" + ordering);
                    sbPrevious.Append("&ordering=" + ordering);
                }
            }
            else
            {
                result = result.OrderByDescending(x => x.CreatedOn);
            }

            List<UserVM> listUsersModel = new List<UserVM>();
            if (result != null && result.Count() > 0)
            {
                foreach (User item in result.ToList())
                {
                    var userModel = new UserVM()
                    {
                        UserId = item.UserId,
                        EmailAddress = item.EmailAddress,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Mobile = item.Mobile,
                        UserName = item.UserName,
                        UserStatus = item.UserStatus,
                        UserGroups = new List<UserGroupMappingModel>(),
                        UserRights = new List<UserRightModel>()
                    };

                    if (item.UserGroupMappings != null)
                    {
                        userModel.UserGroups = item.UserGroupMappings.Select(x => new UserGroupMappingModel()
                        {
                            AccountId = x.AccountId,
                            GroupId = x.GroupId,
                            Description = x.Group?.Description,
                            Name = x.Group?.Name,
                            Status = (x.Group != null ? x.Group.Status : (Int16)0)
                        }).ToList();
                         
                    }

                    if (item.UserRights != null)
                    {
                        userModel.UserRights = item.UserRights.Select(x => new UserRightModel()
                        {
                            AccountId = x.AccountId,
                            IsPermission = x.IsPermission,
                            UserRightId = x.UserRightId,
                            ModuleId = x.ModuleId
                        }).ToList(); 
                    }
                    listUsersModel.Add(userModel);
                }
            }
           
            var finalResult= new ResponseResultList<UserVM>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = listUsersModel
            };
            return Task.FromResult(finalResult);
        }

        /// <summary>
        /// To Update User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        public Task<User> UpdateUser(User userModel)
        {
            base.context.Users.Update(userModel);
            ///base.context.SaveChanges();
            return Task.FromResult(userModel);
        }

        /// <summary>
        /// Get user by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Users> GetUser(long accountId, long userId)
        {
            return base.context.Users
                .Where(x => x.UserId == userId && x.AccountId == accountId)
                .Include(x => x.UserRights)
                .Include(x => x.UserGroupMappings)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetUsersByAccountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Task<List< Users>> GetUsersByAccountId(long accountId)
        {
            return base.context.Users
                .Where(x => x.AccountId == accountId).ToListAsync();
        }

        /// <summary>
        /// To Delete User
        /// </summary>
        /// <param name="userId">The userId to delete </param>
        /// <returns></returns>
        public async Task<long> DeleteUser(long userId)
        {
            long result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var user = await base.context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

                if (user != null)
                {
                    //Delete that post
                    base.context.Users.Remove(user);
                    base.context.SaveChanges();
                    result = userId;
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

        /// <summary>
        /// To Get User By User External Id & authentication category
        /// </summary>
        /// <param name="userExternalId">The Externa ld to get user</param>
        /// <param name="authenticationCategory">The authentication category to get user</param>
        /// <param name="accountId">The accountId to get user</param>
        /// <returns></returns>
        public async Task<Users> GetUserByUserExternalId(long accountId, string userExternalId, string authenticationCategory)
        {
            return await base.context.Users.Include(s => s.UserRights).Include(s => s.UserGroupMappings)
                .FirstOrDefaultAsync(user => user.AccountId.Equals(accountId));
        }

        public async Task<int> CreateOtpLog(OtpLog model)
        {
            base.context.OtpLog.Add(model);
            return await base.context.SaveChangesAsync();
        }


        public async Task<OtpLog> GetOtpLog(long accountId, string deviceId, string countryCode, string contactNumber, string email)
        {
            if (!string.IsNullOrEmpty(contactNumber))
            {
                return await base.context.OtpLog.Where(x => x.IsOtpVerified == false && x.IsOtpSend == true
                    && x.AccountId == accountId && x.DeviceId == deviceId && x.CodeExpiry >= DateTime.UtcNow
                    && x.CountryCode == countryCode && x.ContactNumber == contactNumber)
                    .OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
            }
            else
            {
                return await base.context.OtpLog.Where(x => x.IsOtpVerified == false && x.IsOtpSend == true
                    && x.AccountId == accountId && x.DeviceId == deviceId && x.CodeExpiry >= DateTime.UtcNow
                    && x.Email == email)
                    .OrderByDescending(x => x.CreatedOn).FirstOrDefaultAsync();
            }
        }


        public async Task<int> UpdateOtpLog(OtpLog model)
        {
            base.context.OtpLog.Update(model);
            return await base.context.SaveChangesAsync();
        }
        public  User UpdateStripeId(User user)
        {
            var result=Update(user);
            Save();
            return result;
        }
        public async Task<bool> VerifyOTP(string mobileNo)
        {
             return await context.OtpLog.Where(x => x.ContactNumber == mobileNo && x.IsOtpVerified == true && x.Otptype== "ForgetPassword" && x.UpdatedOn >= DateTime.UtcNow.AddMinutes(-5)).AnyAsync();
        }

        public async Task<User> GetUserDetails(long userId, long accountId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$" SELECT * FROM customer.""Users"" 
                    WHERE ""AccountId"" = {accountId} AND ""UserId"" = {userId}; ";
                var data = await connection.QueryAsync<User>(querySearch);
                return data.FirstOrDefault();
            }
        }

        public async Task<bool> GetApplicationByUser(long userId, long accountId,long applicationId)
        {
            using (IDbConnection connection = new NpgsqlConnection(Bootstrapper.defaultConnectionString))
            {
                var querySearch = @$"select 1 from customer.""UserGroupMappings"" as usr
  JOIN customer.""ApplicationGroupMapping"" as grp
  ON usr.""GroupId"" = grp.""GroupId""
  where ""UserId"" = {userId} and grp.""ApplicationId"" = {applicationId} and grp.""AccountId""={accountId}";
                var data = await connection.QueryAsync<bool>(querySearch);
                return data.Any();
            }
        }
        public async Task<User> UpdateUserNew(User userModel)
        {
           var user=await UpdateUser(userModel);
           await  base.context.SaveChangesAsync();
            return user;
        }
    }
}
