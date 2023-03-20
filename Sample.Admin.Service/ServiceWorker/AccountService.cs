using AutoMapper;
using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Model;
using Sample.Admin.Model.Account.Domain;
using Sample.Admin.Model.Account.New;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Utilities.EmailHelper;
using DataModel = Sample.Admin.Service.Infrastructure.DataModels;

namespace Sample.Admin.Service.ServiceWorker
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;

        private readonly ISubscriptionRepository subscriptionRepository;

        private readonly IUnitOfWork unitOfWork;

        private readonly IVersionModuleRepository versionModuleRepository;

        private readonly IMapper mapper;
        private readonly IEmailHelper emailHelper;

        private IHostingEnvironment hostingEnvironment;

        private const int userStatus = 3;
        private const string groupDescription = "Administrator";
        private const string emailSubject = "Account Created";
        private const string emailTemplateFilename = "AccountCreationEmailFormat.html";
        /// <summary>
        /// Account Service constructor to inject dependency
        /// </summary>
        /// <param name="accountRepository">account repository</param>
        /// <param name="subscriptionRepository">subscription Repository</param>
        /// <param name="userRepository">user repository</param>
        /// <param name="passwordPolicyRepository">password policy repository</param>
        /// <param name="unitOfWork">unit of work</param>
        /// <param name="versionModuleRepository">version module repository</param>
        /// <param name="groupRepository">group repository</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="hostingEnvironment">HostingEnvironment</param>
        public AccountService(IAccountRepository accountRepository,
            ISubscriptionRepository subscriptionRepository,
            IUnitOfWork unitOfWork,
            IVersionModuleRepository versionModuleRepository,
            IMapper mapper, IEmailHelper emailHelper, IHostingEnvironment hostingEnvironment)
        {
            //As discusses with Sunil, we don't need to check internal object
            this.accountRepository = accountRepository;
            this.subscriptionRepository = subscriptionRepository;
            this.unitOfWork = unitOfWork;
            this.versionModuleRepository = versionModuleRepository;
            this.mapper = mapper;
            this.emailHelper = emailHelper;
            this.hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Get All Accounts
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<AccountsListVM>> GetAllAccounts(string ordering, string search, int offset, int pageSize, int pageNumber, bool all) => await this.accountRepository.GetAllAccount(ordering, search, offset, pageSize, pageNumber, all);


        /// <summary>
        /// Get Account By Id
        /// </summary>
        /// <returns></returns>
        public async Task<AccountViewModel> GetAccountById(long accountId) => await this.accountRepository.GetAccountById(accountId);

        /// <summary>
        /// To Create New Account
        /// </summary>
        /// <param name="newAccount">Account Param for account</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountsCreateVM>> AddAccount(AccountViewModel account, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(account.OrganizationName))
            {
                errorDetails.Add("organizationName", new string[] { "This field may not be blank." });
            }
            else if (account.OrganizationName.Length > 250)
            {
                errorDetails.Add("organizationName", new string[] { "Ensure this field has no more than 250 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.ContactPerson))
            {
                errorDetails.Add("contactPerson", new string[] { "This field may not be blank." });
            }
            else if (account.ContactPerson.Length > 100)
            {
                errorDetails.Add("contactPerson", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.ContactEmail))
            {
                errorDetails.Add("contactEmail", new string[] { "This field may not be blank." });
            }
            else if (account.ContactEmail.Length > 100)
            {
                errorDetails.Add("contactEmail", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingAddress))
            {
                errorDetails.Add("billingAddress", new string[] { "This field may not be blank." });
            }
            else if (account.BillingAddress.Length > 500)
            {
                errorDetails.Add("billingAddress", new string[] { "Ensure this field has no more than 500 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingContactPerson))
            {
                errorDetails.Add("billingContactPerson", new string[] { "This field may not be blank." });
            }
            else if (account.BillingContactPerson.Length > 100)
            {
                errorDetails.Add("billingContactPerson", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingEmailAddress))
            {
                errorDetails.Add("billingEmailAddress", new string[] { "This field may not be blank." });
            }
            else if (account.BillingEmailAddress.Length > 100)
            {
                errorDetails.Add("billingEmailAddress", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Region))
            {
                errorDetails.Add("region", new string[] { "This field may not be blank." });
            }
            else if (account.Region.Length > 100)
            {
                errorDetails.Add("region", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.TimeZone))
            {
                errorDetails.Add("timeZone", new string[] { "This field may not be blank." });
            }
            else if (account.TimeZone.Length > 50)
            {
                errorDetails.Add("timeZone", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Locale))
            {
                errorDetails.Add("locale", new string[] { "This field may not be blank." });
            }
            else if (account.Locale.Length > 50)
            {
                errorDetails.Add("locale", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Language))
            {
                errorDetails.Add("language", new string[] { "This field may not be blank." });
            }
            else if (account.Language.Length > 50)
            {
                errorDetails.Add("language", new string[] { "Ensure this field has no more than 50 characters." });
            }
            //added by Raj 
            if (string.IsNullOrWhiteSpace(account.CurrencyId.ToString()))
            {
                errorDetails.Add("currencyId", new string[] { "This field may not be blank." });
            }
            if (string.IsNullOrWhiteSpace(account.AccountUrl))
            {
                errorDetails.Add("accountUrl", new string[] { "This field may not be blank." });
            }
            else if (account.AccountUrl.Length > 250)
            {
                errorDetails.Add("accountUrl", new string[] { "Ensure this field has no more than 250 characters." });
            }
            if (string.IsNullOrWhiteSpace(account.IsolationType.ToString()))
            {
                errorDetails.Add("isolationType", new string[] { "This field may not be blank." });
            }
            if (string.IsNullOrWhiteSpace(account.AuthenticationCategory))
            {
                errorDetails.Add("authenticationCategory", new string[] { "This field may not be blank." });
            }
            if (errorDetails.Count > 0)
            {
                return new ResponseResult<AccountsCreateVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }
            account.AccountGuid = Guid.NewGuid();
            account.CreatedBy = loggedInUserId;
            return await this.accountRepository.AddAccount(account);
        }

        /// <summary>
        /// To Create New Account
        /// </summary>
        /// <param name="newAccount">Account Param for account</param>
        /// <returns></returns>
        public async Task<ResponseResult> CreateAccount(NewAccount newAccount, int loggedInUserId)
        {
            ResponseResult responseResult = new ResponseResult();
            string ValidationMessage = ValidateAccount(newAccount);
            if (!string.IsNullOrWhiteSpace(ValidationMessage))
            {
                responseResult.Message = ValidationMessage;
                responseResult.ResponseCode = ResponseCode.ValidationFailed;
                return responseResult;
            }

            var account = new DataModel.Accounts();
            account.BillingAddress = newAccount.BillingAddress;
            account.BillingContactPerson = newAccount.BillingContactPerson;
            account.BillingEmailAddress = newAccount.BillingEmailAddress;
            account.ContactEmail = newAccount.ContactEmail;
            account.ContactPerson = newAccount.ContactPerson;
            account.Description = newAccount.Description;
            account.Language = newAccount.Language;
            account.OrganizationName = newAccount.OrganizationName;
            account.Region = newAccount.Region;
            account.TenantCss = newAccount.TenantCss;
            account.TenantLogo = newAccount.TenantLogo;
            account.TimeZone = newAccount.TimeZone;
            account.CurrencyId = newAccount.CurrencyId;
            account.Locale = newAccount.Locale;
            account.Active = true;
            account.AccountUrl = newAccount.Url;
            account.AccountGuid = Guid.NewGuid();
            account.CreatedBy = loggedInUserId;
            account.CreatedOn = DateTime.UtcNow;

            await this.accountRepository.CreateAccount(account);

           

            var subscription = new DataModel.Subscriptions();
            subscription.AccountId = account.AccountId;
            subscription.VersionId = newAccount.Subscription.VersionId;
            subscription.Description = newAccount.Subscription.Description;
            subscription.StartDate = newAccount.Subscription.StartDate;
            subscription.EndDate = newAccount.Subscription.EndDate;
            subscription.Cancelled = false;
            subscription.CancelledReason = string.Empty;
            subscription.CreatedBy = loggedInUserId;
            subscription.CreatedOn = DateTime.UtcNow;
            await this.subscriptionRepository.CreateSubscription(subscription);

            
            DataModel.Groups groups = new DataModel.Groups();
           

            this.unitOfWork.Commit();

            var accountCreated = mapper.Map<Sample.Admin.Model.Account.Domain.Account>(account);

            Dictionary<string, string> replaceText = new Dictionary<string, string>();
            replaceText.Add("{FirstName}", newAccount.User.FirstName);
            replaceText.Add("{UserName}", newAccount.User.UserName);
            //replaceText.Add("{Password}", userPassword);

            var rootPath = hostingEnvironment.ContentRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "Helpers" + Path.DirectorySeparatorChar.ToString()
                + "Docs" + Path.DirectorySeparatorChar.ToString()
                + "EmailTemplate" + Path.DirectorySeparatorChar.ToString();
            string filePath = rootPath + emailTemplateFilename;
            string emailBody = emailHelper.PopulateBody(filePath, replaceText);

            emailHelper.SendMail(newAccount.User.EmailAddress, emailSubject, emailBody);

            responseResult.Message = "Account Created Successfully";
            responseResult.ResponseCode = ResponseCode.RecordSaved;
            responseResult.Data = accountCreated;
            return responseResult;
        }

        /// <summary>
        /// To Update existing Account
        /// </summary>
        /// <param name="account">account object</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountViewModel>> UpdateAccount(long accountId, AccountViewModel account, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(account.OrganizationName))
            {
                errorDetails.Add("organizationName", new string[] { "This field may not be blank." });
            }
            else if (account.OrganizationName.Length > 250)
            {
                errorDetails.Add("organizationName", new string[] { "Ensure this field has no more than 250 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.ContactPerson))
            {
                errorDetails.Add("contactPerson", new string[] { "This field may not be blank." });
            }
            else if (account.ContactPerson.Length > 100)
            {
                errorDetails.Add("contactPerson", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.ContactEmail))
            {
                errorDetails.Add("contactEmail", new string[] { "This field may not be blank." });
            }
            else if (account.ContactEmail.Length > 100)
            {
                errorDetails.Add("contactEmail", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingAddress))
            {
                errorDetails.Add("billingAddress", new string[] { "This field may not be blank." });
            }
            else if (account.BillingAddress.Length > 500)
            {
                errorDetails.Add("billingAddress", new string[] { "Ensure this field has no more than 500 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingContactPerson))
            {
                errorDetails.Add("billingContactPerson", new string[] { "This field may not be blank." });
            }
            else if (account.BillingContactPerson.Length > 100)
            {
                errorDetails.Add("billingContactPerson", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingEmailAddress))
            {
                errorDetails.Add("billingEmailAddress", new string[] { "This field may not be blank." });
            }
            else if (account.BillingEmailAddress.Length > 100)
            {
                errorDetails.Add("billingEmailAddress", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Region))
            {
                errorDetails.Add("region", new string[] { "This field may not be blank." });
            }
            else if (account.Region.Length > 100)
            {
                errorDetails.Add("region", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.TimeZone))
            {
                errorDetails.Add("timeZone", new string[] { "This field may not be blank." });
            }
            else if (account.TimeZone.Length > 50)
            {
                errorDetails.Add("timeZone", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Locale))
            {
                errorDetails.Add("locale", new string[] { "This field may not be blank." });
            }
            else if (account.Locale.Length > 50)
            {
                errorDetails.Add("locale", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Language))
            {
                errorDetails.Add("language", new string[] { "This field may not be blank." });
            }
            else if (account.Language.Length > 50)
            {
                errorDetails.Add("language", new string[] { "Ensure this field has no more than 50 characters." });
            }
            //added by Raj 
            if (string.IsNullOrWhiteSpace(account.CurrencyId.ToString()))
            {
                errorDetails.Add("currencyId", new string[] { "This field may not be blank." });
            }
            if (string.IsNullOrWhiteSpace(account.AccountUrl))
            {
                errorDetails.Add("accountUrl", new string[] { "This field may not be blank." });
            }
            else if (account.AccountUrl.Length > 250)
            {
                errorDetails.Add("accountUrl", new string[] { "Ensure this field has no more than 250 characters." });
            }
            if (string.IsNullOrWhiteSpace(account.IsolationType.ToString()))
            {
                errorDetails.Add("isolationType", new string[] { "This field may not be blank." });
            }
            if (string.IsNullOrWhiteSpace(account.AuthenticationCategory))
            {
                errorDetails.Add("authenticationCategory", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<AccountViewModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            account.UpdatedBy = loggedInUserId;
            return await this.accountRepository.UpdateAccount(accountId, account);


        }

        /// <summary>
        /// To Update Account Partially
        /// </summary>
        /// /// <param name="accountId">Account ID</param>
        /// <param name="account">New account object</param>
        /// <returns></returns>
        public async Task<ResponseResult<AccountViewModel>> UpdatePartialAccount(long accountId, AccountViewModel account, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(account.OrganizationName))
            {
                errorDetails.Add("organizationName", new string[] { "This field may not be blank." });
            }
            else if (account.OrganizationName.Length > 100)
            {
                errorDetails.Add("organizationName", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.ContactPerson))
            {
                errorDetails.Add("contactPerson", new string[] { "This field may not be blank." });
            }
            else if (account.ContactPerson.Length > 100)
            {
                errorDetails.Add("contactPerson", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.ContactEmail))
            {
                errorDetails.Add("contactEmail", new string[] { "This field may not be blank." });
            }
            else if (account.ContactEmail.Length > 100)
            {
                errorDetails.Add("contactEmail", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingAddress))
            {
                errorDetails.Add("billingAddress", new string[] { "This field may not be blank." });
            }
            else if (account.BillingAddress.Length > 500)
            {
                errorDetails.Add("billingAddress", new string[] { "Ensure this field has no more than 500 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingContactPerson))
            {
                errorDetails.Add("billingContactPerson", new string[] { "This field may not be blank." });
            }
            else if (account.BillingContactPerson.Length > 100)
            {
                errorDetails.Add("billingContactPerson", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.BillingEmailAddress))
            {
                errorDetails.Add("billingEmailAddress", new string[] { "This field may not be blank." });
            }
            else if (account.BillingEmailAddress.Length > 100)
            {
                errorDetails.Add("billingEmailAddress", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Region))
            {
                errorDetails.Add("region", new string[] { "This field may not be blank." });
            }
            else if (account.Region.Length > 100)
            {
                errorDetails.Add("region", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.TimeZone))
            {
                errorDetails.Add("timeZone", new string[] { "This field may not be blank." });
            }
            else if (account.TimeZone.Length > 50)
            {
                errorDetails.Add("timeZone", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Locale))
            {
                errorDetails.Add("locale", new string[] { "This field may not be blank." });
            }
            else if (account.Locale.Length > 50)
            {
                errorDetails.Add("locale", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(account.Language))
            {
                errorDetails.Add("language", new string[] { "This field may not be blank." });
            }
            else if (account.Language.Length > 50)
            {
                errorDetails.Add("language", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<AccountViewModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }
            account.UpdatedBy = loggedInUserId;
            return await this.accountRepository.UpdateAccount(accountId, account);
        }

        /// <summary>
        /// To delete existing Account
        /// </summary>
        /// <param name="accountId">account identifier</param>
        /// <returns></returns>
        public async Task<long> DeleteAccount(long accountId) => await this.accountRepository.DeleteAccount(accountId);

        /// <summary>
        /// To Validate Account Input
        /// </summary>
        /// <param name="newAccount">account param for validation</param>
        /// <returns></returns>
        public string ValidateAccount(NewAccount newAccount)
        {
            string message = string.Empty;
            StringBuilder validationMessage = new StringBuilder();
            var existingOrganization = accountRepository.CheckAccountDuplicateOrganisationName(newAccount.OrganizationName);
            var existingUrl = accountRepository.CheckAccountDuplicateUrl(newAccount.Url);
            //var existingUsers = userRepository.GetExistingUsers(newAccount.User.UserName);
            //var existingUserPhone = userRepository.GetExistingUserPhone(newAccount.User.Mobile);
            //var existingUserEmail = userRepository.GetExistingUserEmail(newAccount.User.EmailAddress);
            //await Task.WhenAll(existingOrganization, existingUrl, existingUsers, existingUserPhone, existingUserEmail);
            //if (existingOrganization.Result != null)
            //    validationMessage.Append(", Organization name");
            //if (existingUrl.Result != null)
            //    validationMessage.Append(", Account url");
            //if (existingUsers.Result != null)
            //    validationMessage.Append(", Account User name");
            //if (existingUserPhone.Result != null)
            //    validationMessage.Append(", User Mobile number");
            //if (existingUserEmail.Result != null)
            //    validationMessage.Append(", User Email");
            if (!string.IsNullOrWhiteSpace(validationMessage.ToString()))
                message = $"Validation failed {validationMessage} already exists";
            return message;
        }


        /// <summary>
        /// Get all Account details
        /// </summary>
        /// <param name="accountId">The accountId to get account</param>
        /// <returns></returns>
        public async Task<AccountModel> GetAccount(long accountId)
        {
            return await this.accountRepository.GetAccount(accountId);
        }

        /// <summary>
        /// Get Account For TenantResolver
        /// </summary>
        /// <param name="accountIdentity">The accountIdentity to get account</param>
        /// <returns></returns>
        public async Task<Account> GetAccountForTenantResolver(string accountIdentity)
        {
            return await this.accountRepository.GetAccountForTenantResolver(accountIdentity);
        }

        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="accountIdentity">The account Identity to get account</param>
        /// <returns></returns>
        public async Task<AccountDetail> GetAccountDetails(string accountIdentity)
        {
            return await this.accountRepository.GetAccountDetails(accountIdentity);
        }

        /// <summary>
        /// Get Account List
        /// </summary>
        /// <returns></returns>
        public async Task<List<AccountDetail>> GetAccountList()
        {
            return await this.accountRepository.GetAccountList();
        }
    }
}
