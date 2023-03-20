using AutoMapper;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model.Account.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;
using Microsoft.Extensions.Configuration;
using Common.Model;
using System.Text;
using Sample.Admin.Model;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class AccountRepository : RepositoryBase<Accounts>, IAccountRepository
    {
        /// <summary>
        /// commonHelper Private Member
        /// </summary>
        private readonly ICommonHelper commonHelper;
        public IConfiguration configuration { get; }

        private readonly IMapper mapper;
        public AccountRepository(ICommonHelper commonHelper, CloudAcceleratorContext context, IMapper mapper, IConfiguration configuration) : base(context)
        {
            this.mapper = mapper;
            this.commonHelper = commonHelper;
            this.configuration = configuration;
        }

        ///// <summary>
        ///// Get Account List
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<AccountDetail>> GetAccountList()
        //{
        //    return await base.context.Accounts.Where(x => x.Active == true)
        //            .Select(x => new AccountDetail()
        //            {
        //                AccountId = x.AccountId,
        //                AccountGUID = x.AccountGuid,
        //                OrganizationName = x.OrganizationName,
        //                Description = x.Description,
        //                TenantCSS = commonHelper.GetTenantTheme(x.TenantCss),
        //                TenantLogo = x.TenantLogo,
        //                TimeZone = x.TimeZone
        //            }).ToListAsync();
        //}

        /// <summary>
        /// Get Account List
        /// </summary>
        /// <returns></returns>
        public async Task<List<AccountDetail>> GetAccountList()
        {
            return await base.context.Accounts.Where(x => x.Active == true)
                    .Select(x => new AccountDetail()
                    {
                        AccountId = x.AccountId,
                        AccountGUID = x.AccountGuid,
                        OrganizationName = x.OrganizationName,
                        Description = x.Description,
                        TenantCSS = commonHelper.GetTenantTheme(x.TenantCss),
                        TenantLogo = x.TenantLogo,
                        TimeZone = x.TimeZone,
                        AuthenticationCategory = x.AuthenticationCategory,
                        AuthenticationTypeId = x.AuthenticationTypeId ?? 0
                    }).ToListAsync();
        }

        /// <summary>
        /// Get All Account
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<AccountsListVM>> GetAllAccount(string ordering, string search, int offset, int pageSize, int pageNumber, bool all)
        {
            IQueryable<Accounts> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("");
            StringBuilder sbPrevious = new StringBuilder("");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string[] searchList = search.Split(",");

                foreach (string item in searchList)
                {
                    Int64.TryParse(item, out long accountId);

                    if (result == null)
                    {
                        result = from accounts in base.context.Accounts
                                 where accounts.AccountId == accountId || accounts.OrganizationName.Contains(item) || accounts.ContactPerson.Contains(item) || accounts.ContactEmail.Contains(item)
                                 select accounts;
                    }
                    else
                    {
                        result = result.Concat(from accounts in base.context.Accounts
                                               where accounts.AccountId == accountId || accounts.OrganizationName.Contains(item) || accounts.ContactPerson.Contains(item) || accounts.ContactEmail.Contains(item)
                                               select accounts);
                    }
                }
            }
            else
            {
                result = from accounts in base.context.Accounts
                         select accounts;
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
                if (typeof(Accounts).GetProperty(ordering) != null)
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
                result = result.OrderByDescending(x => x.AccountId);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (!string.IsNullOrEmpty(sbNext.ToString()))
                    sbNext.Append("&search=" + search);

                if (!string.IsNullOrEmpty(sbPrevious.ToString()))
                    sbPrevious.Append("&search=" + search);
            }

            var accountNew = new List<AccountsListVM>();
            if (result != null && result.Count() > 0)
            {
                accountNew = await result.Select(x => new AccountsListVM()
                {
                    AccountId = x.AccountId,
                    OrganizationName = x.OrganizationName,
                    Description = x.Description,
                    TenantCss = x.TenantCss,
                    TenantLogo = x.TenantLogo,
                    TimeZone = x.TimeZone,
                    AccountGuid = x.AccountGuid,
                    IsolationType = x.IsolationType,
                }).ToListAsync();
            }


            return new ResponseResultList<AccountsListVM>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = accountNew
            };
        }

        /// <summary>
        /// Get Account By Id
        /// </summary>
        /// <returns></returns>
        public async Task<AccountViewModel> GetAccountById(long accountId)
        {
            return await base.context.Accounts
                .Where(m => m.AccountId == accountId)
                .Select(x => new AccountViewModel()
                {
                    AccountGuid = x.AccountGuid,
                    AccountId = x.AccountId,
                    AccountUrl = x.AccountUrl,
                    AuthenticationType = Convert.ToInt32(x.AuthenticationTypeId),
                    AuthenticationCategory = x.AuthenticationCategory,
                    OrganizationName = x.OrganizationName,
                    IsolationType = x.IsolationType,
                    TenantCSS = x.TenantCss,
                    TenantLogo = x.TenantLogo,
                    Description = x.Description,

                    Active = x.Active,
                    BillingAddress = x.BillingAddress,
                    BillingContactPerson = x.BillingContactPerson,
                    BillingEmailAddress = x.BillingEmailAddress,

                    ContactEmail = x.ContactEmail,
                    ContactPerson = x.ContactPerson,
                    CurrencyId = x.CurrencyId ?? 0,
                    Language = x.Language,
                    Locale = x.Locale,
                    Region = x.Region,
                    TimeZone = x.TimeZone,

                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn = x.UpdatedOn,
                })
                .FirstOrDefaultAsync(); 
        }

        /// <summary>
        /// To Create account
        /// </summary>
        /// <param name="account">New Account Object</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountsCreateVM>> AddAccount(AccountViewModel model)
        {
            if (base.context.Accounts.Any(x => x.AccountGuid == model.AccountGuid))
            {
                var errorMsg = new Dictionary<string, string[]>();
                errorMsg.Add("accountGuid", new[] { "Account GUID is not unique." });
                return new ResponseResult<AccountsCreateVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorMsg
                    }
                };
            }
            else if (base.context.Accounts.Any(x => x.AccountUrl == model.AccountUrl))
            {
                var errorMsg = new Dictionary<string, string[]>();
                errorMsg.Add("accountUrl", new[] { "Account Url is not unique." });
                return new ResponseResult<AccountsCreateVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorMsg
                    }
                };
            }
            //added by Raj for validate organization name and contact email
            else if (base.context.Accounts.Any(x => x.OrganizationName == model.OrganizationName))
            {
                var errorMsg = new Dictionary<string, string[]>();
                errorMsg.Add("organizationName", new[] { "Organization Name is not unique." });
                return new ResponseResult<AccountsCreateVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorMsg
                    }
                };
            }

            else if (base.context.Accounts.Any(x => x.ContactEmail.Contains(model.ContactEmail)))
            {
                var errorMsg = new Dictionary<string, string[]>();
                errorMsg.Add("contactEmail", new[] { "Contact Email is not unique." });
                return new ResponseResult<AccountsCreateVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorMsg
                    }
                };
            }
            var newAccount = new Accounts()
            {
                
                OrganizationName = model.OrganizationName,
                Description = model.Description,
                ContactPerson = model.ContactPerson,
                ContactEmail = model.ContactEmail,
                BillingAddress = model.BillingAddress,
                BillingContactPerson = model.BillingContactPerson,
                BillingEmailAddress = model.BillingEmailAddress,
                Active = model.Active,
                TenantCss = model.TenantCSS,
                TenantLogo = model.TenantLogo,
                Region = model.Region,
                TimeZone = model.TimeZone,
                Locale = model.Locale,
                Language = model.Language,
                CurrencyId = model.CurrencyId,
                AccountGuid = model.AccountGuid,
                AccountUrl = model.AccountUrl,
                AuthenticationCategory = model.AuthenticationCategory,
                AuthenticationTypeId = model.AuthenticationType,
                IsolationType = model.IsolationType,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,                
            };

            newAccount.CreatedBy = 1;
            newAccount.CreatedOn = DateTime.UtcNow;
            base.context.Accounts.Add(newAccount);
            if (await base.context.SaveChangesAsync() < 1)
            {
                return new ResponseResult<AccountsCreateVM>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            return new ResponseResult<AccountsCreateVM>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = mapper.Map<AccountsCreateVM>(newAccount)
            };
        }

        /// <summary>
        /// To Create account
        /// </summary>
        /// <param name="account">New Account Object</param>
        /// <returns></returns>
        public async Task<Accounts> CreateAccount(Accounts account)
        {
            await base.context.Accounts.AddAsync(account);
            return account;
        }

        /// <summary>
        /// To Update Account
        /// </summary>
        /// <param name="account">new account object</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountViewModel>> UpdateAccount(long accountId, AccountViewModel account)
        {
            var accountNew = await base.context.Accounts.FindAsync(accountId);
            //added by Raj for validate organization name and contact email and accountUrl
            
            if (accountNew != null)
            {
                if (base.context.Accounts.Any(x => x.AccountUrl == account.AccountUrl && x.AccountId != accountId))
                {
                    var errorMsg = new Dictionary<string, string[]>();
                    errorMsg.Add("accountUrl", new[] { "Account Url is not unique." });
                    return new ResponseResult<AccountViewModel>()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        ResponseCode = ResponseCode.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Detail = errorMsg
                        }
                    };
                }
                else if (base.context.Accounts.Any(x => x.OrganizationName == account.OrganizationName && x.AccountId != accountId))
                {
                    var errorMsg = new Dictionary<string, string[]>();
                    errorMsg.Add("organizationName", new[] { "Organization Name is not unique." });
                    return new ResponseResult<AccountViewModel>()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        ResponseCode = ResponseCode.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Detail = errorMsg
                        }
                    };
                }

                else if (base.context.Accounts.Any(x => x.ContactEmail.Contains(account.ContactEmail) && x.AccountId != accountId))
                {
                    var errorMsg = new Dictionary<string, string[]>();
                    errorMsg.Add("contactEmail", new[] { "Contact Email is not unique." });
                    return new ResponseResult<AccountViewModel>()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        ResponseCode = ResponseCode.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Detail = errorMsg
                        }
                    };
                }

                accountNew.OrganizationName = account.OrganizationName;
                accountNew.Description = account.Description;
                accountNew.ContactPerson = account.ContactPerson;
                accountNew.ContactEmail = account.ContactEmail;
                accountNew.BillingAddress = account.BillingAddress;
                accountNew.BillingContactPerson = account.BillingContactPerson;
                accountNew.BillingEmailAddress = account.BillingEmailAddress;
                accountNew.Active = account.Active;
                accountNew.TenantCss = account.TenantCSS;
                accountNew.TenantLogo = account.TenantLogo;
                accountNew.Region = account.Region;
                accountNew.TimeZone = account.TimeZone;
                accountNew.Locale = account.Locale;
                accountNew.Language = account.Language;
                accountNew.CurrencyId = account.CurrencyId;
                accountNew.UpdatedOn = DateTime.UtcNow;
                accountNew.UpdatedBy = account.UpdatedBy;
                // accountNew.AccountGuid = account.AccountGuid;
                accountNew.AccountUrl = account.AccountUrl;
                accountNew.AuthenticationCategory = account.AuthenticationCategory;
                accountNew.AuthenticationTypeId = account.AuthenticationType;
                accountNew.IsolationType = account.IsolationType;
                base.context.Accounts.Update(accountNew);
               await base.context.SaveChangesAsync();
                account.AccountGuid = accountNew.AccountGuid;
                //return account;
            }
            return new ResponseResult<AccountViewModel>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = account
            };
        }


        /// <summary>
        /// To Delete Account
        /// </summary>
        /// <param name="accountId">The accountId to delete </param>
        /// <returns></returns>
        public async Task<long> DeleteAccount(long accountId)
        {
            long result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var account = await base.context.Accounts.FirstOrDefaultAsync(x => x.AccountId == accountId);

                if (account != null)
                {
                    //Delete that post
                    base.context.Accounts.Remove(account);

                    //Commit the transaction
                    result = await base.context.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        /// <summary>
        /// To check if Organisation exists with given Organisation name
        /// </summary>
        /// <param name="organisationName">Organisation Name</param>
        /// <returns></returns>
        public async Task<Accounts> CheckAccountDuplicateOrganisationName(string organisationName)
        {
            return await base.context.Accounts.FirstOrDefaultAsync(x => x.OrganizationName.Equals(organisationName));
        }

        /// <summary>
        /// To check if Account url already exists
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Accounts> CheckAccountDuplicateUrl(string url)
        {
            return await base.context.Accounts.FirstOrDefaultAsync(x => x.AccountUrl.Equals(url));
        }

        /// <summary>
        /// Get Account For TenantResolver
        /// </summary>
        /// <param name="accountIdentity">The accountIdentity to get account</param>
        /// <returns></returns>
        public async Task<Account> GetAccountForTenantResolver(string accountIdentity)
        {
            bool isAccountId = Int64.TryParse(accountIdentity, out var accountId);
            bool isGuid = Guid.TryParse(accountIdentity, out var accountGuid);
            var query = from accounts in base.context.Accounts
                        where accounts.Active == true
                        select new Account
                        {
                            AccountId = accounts.AccountId,
                            OrganizationName = accounts.OrganizationName,
                            Description = accounts.Description,
                            AccountUrl = accounts.AccountUrl,
                            ContactPerson = accounts.ContactPerson,
                            ContactEmail = accounts.ContactEmail,
                            BillingAddress = accounts.BillingAddress,
                            BillingContactPerson = accounts.BillingContactPerson,
                            BillingEmailAddress = accounts.BillingEmailAddress,
                            Active = accounts.Active,
                            TenantCss = accounts.TenantCss,
                            TenantLogo = accounts.TenantLogo,
                            TimeZone = accounts.TimeZone,
                            Region = accounts.Region,
                            Locale = accounts.Locale,
                            CurrencyId = accounts.CurrencyId ?? 0,
                            Language = accounts.Language,
                            AccountGuid = accounts.AccountGuid,
                            AuthenticationCategory = accounts.AuthenticationCategory,
                            AuthenticationTypeId = accounts.AuthenticationTypeId ?? 0,
                        };
            if (isGuid)
                query = query.Where(x => x.AccountGuid.Equals(accountGuid));
            else if (isAccountId)
                query = query.Where(x => x.AccountId.Equals(accountId));
            else
                query = query.Where(x => x.AccountUrl.Equals(accountIdentity));
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Account For TenantResolver
        /// </summary>
        /// <param name="accountIdentity">The accountIdentity to get account</param>
        /// <returns>AccountModel</returns>
        public async Task<AccountModel> GetAccount(long accountId)
        {
            return await base.context.Accounts.Where(x => x.Active == true && x.AccountId == accountId)
                .Select(x => new AccountModel()
                {
                    AccountId = x.AccountId,
                    OrganizationName = x.OrganizationName,
                    Description = x.Description,
                    AccountUrl = x.AccountUrl,
                    ContactPerson = x.ContactPerson,
                    ContactEmail = x.ContactEmail,
                    BillingAddress = x.BillingAddress,
                    BillingContactPerson = x.BillingContactPerson,
                    BillingEmailAddress = x.BillingEmailAddress,
                    Active = x.Active,
                    TenantCss = x.TenantCss,
                    TenantThemeCSS = commonHelper.GetTenantTheme(x.TenantCss),
                    TenantLogo = x.TenantLogo,
                    TimeZone = x.TimeZone,
                    Region = x.Region,
                    Locale = x.Locale,
                    CurrencyId = x.CurrencyId ?? 0,
                    Language = x.Language,
                    AccountGuid = x.AccountGuid
                }).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="accountIdentity">The account Identity to get account</param>
        /// <returns></returns>
        public async Task<AccountDetail> GetAccountDetails(string accountIdentity)
        {
            bool isAccountId = Int64.TryParse(accountIdentity, out var accountId);
            bool isGuid = Guid.TryParse(accountIdentity, out var accountGuid);
            var query = from x in base.context.Accounts
                        where x.Active == true
                        select x;

            if (isGuid)
                query = query.Where(x => x.AccountGuid.Equals(accountGuid));
            else if (isAccountId)
                query = query.Where(x => x.AccountId.Equals(accountId));
            else
                query = query.Where(x => x.AccountUrl.Equals(accountIdentity));

            return await query.Select(x => new AccountDetail()
            {
                AccountId = x.AccountId,
                AccountGUID = x.AccountGuid,
                OrganizationName = x.OrganizationName,
                Description = x.Description,
                TenantCSS = commonHelper.GetTenantTheme(x.TenantCss),
                TenantLogo = x.TenantLogo,
                TimeZone = x.TimeZone,
                IsRentingTrunOn=x.IsRentingTrunOn,
                TourVideoURL=x.TourVideoUrl,
                GetStartVideoURL=x.GetStartVideoUrl,
                AuthenticationCategory = x.AuthenticationCategory,
                AuthenticationTypeId = x.AuthenticationTypeId ?? 0
            }).FirstOrDefaultAsync();
        }
    }
}