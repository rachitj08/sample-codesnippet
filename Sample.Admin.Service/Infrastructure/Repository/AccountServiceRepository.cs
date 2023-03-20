using AutoMapper;
using Common.Model;
using Sample.Admin.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TenantDataIsolation;
using Sample.Admin.Service.Infrastructure.DataModels;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class AccountServiceRepository : RepositoryBase<AccountServices>, IAccountServiceRepository
    {
        public IConfiguration configuration { get; }
        private readonly IMapper mapper;
        public AccountServiceRepository(CloudAcceleratorContext context, IMapper mapper, IConfiguration configuration) : base(context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get All AccountService
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<AccountServicesVM>> GetAllAccountService(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            IQueryable<AccountServices> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("");
            StringBuilder sbPrevious = new StringBuilder("");

            result = (from acs in base.context.AccountServices
                      select acs);

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
                if (typeof(AccountServices).GetProperty(ordering) != null)
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
                result = result.OrderByDescending(x => x.AccountServiceId);
            }

            var accountServicesNew = new List<AccountServicesVM>();
            if (result != null && result.Count() > 0)
            {
                accountServicesNew = await (from acs in result
                                            join ac in base.context.Accounts on acs.AccountId equals ac.AccountId
                                            join sr in base.context.Services on acs.ServiceId equals sr.ServiceId
                                            select new AccountServicesVM
                                            {
                                                AccountServiceId = acs.AccountServiceId,
                                                AccountId = acs.AccountId,
                                                Account = acs.AccountId,
                                                AccountName = ac.OrganizationName,
                                                Service = acs.ServiceId,
                                                ServiceName = sr.ServiceName,
                                                DbServer = acs.DbServer,
                                                Port = acs.Port,
                                                DbName = acs.DbName,
                                                DbSchema = acs.DbSchema,
                                                UserName = acs.UserName,
                                                Password = acs.Password,
                                                IsolationType = ac.IsolationType,
                                            }).ToListAsync();

            }


            return new ResponseResultList<AccountServicesVM>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = accountServicesNew
            };
        }

        /// <summary>
        /// Get AccountService By Id
        /// </summary>
        /// <returns></returns>
        public async Task<AccountServicesVM> GetAccountServiceById(int accountServiceId)
        {
            return await (from acs in base.context.AccountServices
                          join ac in base.context.Accounts on acs.AccountId equals ac.AccountId
                          join sr in base.context.Services on acs.ServiceId equals sr.ServiceId
                          where acs.AccountServiceId == accountServiceId
                          select new AccountServicesVM
                          {
                              AccountServiceId = acs.AccountServiceId,
                              Account = acs.AccountId,
                              AccountId = acs.AccountId,
                              AccountName = ac.OrganizationName,
                              Service = acs.ServiceId,
                              ServiceName = sr.ServiceName,
                              DbServer = acs.DbServer,
                              Port = acs.Port,
                              DbName = acs.DbName,
                              DbSchema = acs.DbSchema,
                              UserName = acs.UserName,
                              Password = acs.Password,
                              IsolationType = ac.IsolationType,
                          }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// To Create accountService
        /// </summary>
        /// <param name="accountService">New AccountService Object</param>
        /// <returns></returns>
        public async Task<AccountServicesVM> AddAccountService(AccountServicesModel accountService, int userId)
        {
            // Check DB and Service
            var accountServicesCount = await base.context.AccountServices
                            .CountAsync(x => x.AccountId == accountService.Account && x.ServiceId == accountService.Service);
            if (accountServicesCount > 0)
            {
                throw new Exception("Already have account service details for same account and schema.");
            }

            var account = (from acc in base.context.Accounts
                          where acc.AccountId == accountService.Account
                          select acc).FirstOrDefault();

            var accountServicesNew = new AccountServices
            {
                DbServer = accountService.DbServer,
                Port = accountService.Port,
                DbName = accountService.DbName,
                DbSchema = accountService.DbSchema,
                UserName = accountService.UserName,
                Password = accountService.Password,
                AccountId = accountService.Account,
                ServiceId = accountService.Service,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = userId
            };
            base.context.AccountServices.Add(accountServicesNew);
            await base.context.SaveChangesAsync();


            return await (from acs in base.context.AccountServices
                            join ac in base.context.Accounts on acs.AccountId equals ac.AccountId
                            join sr in base.context.Services on acs.ServiceId equals sr.ServiceId
                            where acs.AccountServiceId == accountServicesNew.AccountServiceId
                            select new AccountServicesVM
                            {
                                AccountServiceId = acs.AccountServiceId,
                                Account = acs.AccountId,
                                AccountId = acs.AccountId,
                                AccountName = ac.OrganizationName,
                                Service = acs.ServiceId,
                                ServiceName = sr.ServiceName,
                                DbServer = acs.DbServer,
                                Port = acs.Port,
                                DbName = acs.DbName,
                                DbSchema = acs.DbSchema,
                                UserName = acs.UserName,
                                Password = acs.Password,
                                IsolationType = ac.IsolationType,
                            }).FirstOrDefaultAsync();
            
            

        }

        /// <summary>
        /// To Update AccountService
        /// </summary>
        /// <param name="accountService">new accountService object</param>
        /// <returns></returns>
        public async Task<AccountServicesVM> UpdateAccountService(int accountServiceId, AccountServicesModel accountService, int userId)
        {
            var accountServiceNew = await base.context.AccountServices.FindAsync(accountServiceId);
            var accountServicesModel = new AccountServicesVM();
            if (accountServiceNew != null)
            {
                accountServiceNew.AccountId = accountService.Account;
                accountServiceNew.ServiceId = accountService.Service;
                accountServiceNew.DbServer = accountService.DbServer;
                accountServiceNew.Port = accountService.Port;
                accountServiceNew.DbName = accountService.DbName;
                accountServiceNew.DbSchema = accountService.DbSchema;
                accountServiceNew.UserName = accountService.UserName;
                accountServiceNew.Password = accountService.Password;
                accountServiceNew.UpdatedOn = DateTime.UtcNow;
                accountServiceNew.UpdatedBy = userId;

                await base.context.SaveChangesAsync();

                return await (from acs in base.context.AccountServices
                              join ac in base.context.Accounts on acs.AccountId equals ac.AccountId
                              join sr in base.context.Services on acs.ServiceId equals sr.ServiceId
                              where acs.AccountServiceId == accountServiceId
                              select new AccountServicesVM
                              {
                                  AccountServiceId = acs.AccountServiceId,
                                  Account = acs.AccountId,
                                  AccountId = acs.AccountId,
                                  AccountName = ac.OrganizationName,
                                  Service = acs.ServiceId,
                                  ServiceName = sr.ServiceName,
                                  DbServer = acs.DbServer,
                                  Port = acs.Port,
                                  DbName = acs.DbName,
                                  DbSchema = acs.DbSchema,
                                  UserName = acs.UserName,
                                  Password = acs.Password,
                                  IsolationType = ac.IsolationType,
                              }).FirstOrDefaultAsync();
            }
            return accountServicesModel;
        }

        /// <summary>
        /// To Update AccountService Partially
        /// </summary>
        /// /// <param name="accountServiceId">AccountService ID</param>
        /// <param name="accountService">New accountService object</param>
        /// <returns></returns>
        public async Task<AccountServicesVM> UpdatePartialAccountService(int accountServiceId, AccountServicesModel accountService, int userId)
        {
            return await UpdateAccountService(accountServiceId, accountService, userId);
        }

        /// <summary>
        /// To Delete AccountService
        /// </summary>
        /// <param name="accountServiceId">The accountServiceId to delete </param>
        /// <returns></returns>
        public async Task<int> DeleteAccountService(int accountServiceId)
        {
            int result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var accountService = await base.context.AccountServices.FirstOrDefaultAsync(x => x.AccountServiceId == accountServiceId);

                if (accountService != null)
                {
                    //Delete that post
                    base.context.AccountServices.Remove(accountService);

                    //Commit the transaction
                    result = await base.context.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        /// <summary>
        /// Get All Account Services
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<AccountServices>> GetAllAccountServices()
        {
            var result = await base.context.AccountServices.ToListAsync();
            return result;
        }

        /// <summary>
        /// Get Account Services By AccountId
        /// </summary>
        /// <param name="accountId">The Account Id to get Account services</param>
        /// <returns></returns>
        public async Task<IEnumerable<AccountServices>> GetAccountServicesByAccountId(long accountId)
        {
            return await base.context.AccountServices.Where(x=>x.AccountId == accountId).ToListAsync();
        }

        /// <summary>
        /// Get acccount service for an account 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<AccountServicesVM> GetAccountServiceByAccountId(long accountId, int serviceId)
        {
            var result = await (from acs in base.context.AccountServices
                                join services in base.context.Services on acs.ServiceId equals services.ServiceId
                                where acs.AccountId == accountId && services.ServiceId == serviceId
                                select new AccountServicesVM
                                {
                                    AccountServiceId = acs.AccountServiceId,
                                    Account = acs.AccountId,
                                    AccountId = acs.AccountId,
                                    Service = acs.ServiceId,
                                    ServiceName = services.ServiceName,
                                    DbServer = acs.DbServer,
                                    Port = acs.Port,
                                    DbName = acs.DbName,
                                    DbSchema = acs.DbSchema,
                                    UserName = acs.UserName,
                                    Password = acs.Password,
                                }).FirstOrDefaultAsync();
            return result;
        }
        
        /// <summary>
        /// To Create Account Service
        /// </summary>
        /// <param name="accountServices">account services model to save account services</param>
        /// <returns></returns>
        public async Task<AccountServices> CreateAccountService(AccountServices accountServices)
        {
            base.context.AccountServices.Add(accountServices);
            await base.context.SaveChangesAsync();
            return accountServices;
        } 
    }
}
