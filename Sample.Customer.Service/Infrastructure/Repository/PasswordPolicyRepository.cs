using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.Repository;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Model;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    public class PasswordPolicyRepository : RepositoryBase<PasswordPolicyModel>,  IPasswordPolicyRepository
    {
        /// <summary>
        /// configuration variable
        /// </summary>
        private readonly IMapper mapper;

        public PasswordPolicyRepository(CloudAcceleratorContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Get PasswordPolicy By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get PasswordPolicy</param>
        /// <returns></returns>
        public async Task<PasswordPolicyModel> GetPasswordPolicyByAccountId(long accountId)
        {
            return await base.context.PasswordPolicies.Where(x=> x.AccountId == accountId)
                .Select(x=> new PasswordPolicyModel()
                {                   
                    AccountId = x.AccountId,
                    OneLowerCase = x.OneLowerCase,
                    OneUpperCase = x.OneUpperCase,
                    OneNumber = x.OneNumber,
                    OneSpecialChar = x.OneSpecialChar,
                    AllowUsersToChangePassword = x.AllowUsersToChangePassword,
                    ExpiryInDays = x.ExpiryInDays,
                    IsEmailVerificationRequired = x.IsEmailVerificationRequired,
                    IsMobileVerificationRequired = x.IsMobileVerificationRequired,
                    MinPasswordLength = x.MinPasswordLength,
                    NoOfFailedAttemptsAllowed = x.NoOfFailedAttemptsAllowed,
                    NoOfPwdToRemember = x.NoOfPwdToRemember,
                    PasswordExpiration = x.PasswordExpiration,
                    PasswordExpirationRequiresAdminReset = x.PasswordExpirationRequiresAdminReset,
                    PreventPasswordReuse = x.PreventPasswordReuse                    
                })
                .FirstOrDefaultAsync();
        }


        /// <summary>
        /// To Create Password Policy
        /// </summary>
        /// <param name="passwordPolicy">New passwordPolicy Object</param>
        /// <returns></returns>
        public async Task<PasswordPolicyModel> CreatePasswordPolicy(PasswordPolicyModel newPasswordPolicy, long userId)
        {
            PasswordPolicies objPasswordPolicyDataModel = mapper.Map<PasswordPolicies>(newPasswordPolicy);
            objPasswordPolicyDataModel.CreatedOn = DateTime.UtcNow;
            objPasswordPolicyDataModel.CreatedBy = userId;
            base.context.PasswordPolicies.Add(objPasswordPolicyDataModel);
            await base.context.SaveChangesAsync();
            return newPasswordPolicy;
        }


        /// <summary>
        /// To Update Password Policy
        /// </summary>
        /// <param name="passwordPolicy">new passwordPolicy object</param>
        /// <returns></returns>
        public async Task<PasswordPolicyModel> UpdatePasswordPolicy(long accountId, PasswordPolicyModel passwordPolicy, long userId)
        {
            var passwordPolicyNew = base.context.PasswordPolicies.Where(x => x.AccountId == accountId).FirstOrDefault();
            if (passwordPolicyNew != null)
            {
                passwordPolicyNew.AllowUsersToChangePassword = passwordPolicy.AllowUsersToChangePassword;
                passwordPolicyNew.ExpiryInDays = passwordPolicy.ExpiryInDays;
                passwordPolicyNew.IsEmailVerificationRequired = passwordPolicy.IsEmailVerificationRequired;
                passwordPolicyNew.IsMobileVerificationRequired = passwordPolicy.IsMobileVerificationRequired;
                passwordPolicyNew.MinPasswordLength = passwordPolicy.MinPasswordLength;
                passwordPolicyNew.NoOfFailedAttemptsAllowed = passwordPolicy.NoOfFailedAttemptsAllowed;
                passwordPolicyNew.OneLowerCase = passwordPolicy.OneLowerCase;
                passwordPolicyNew.OneNumber = passwordPolicy.OneNumber;
                passwordPolicyNew.OneSpecialChar = passwordPolicy.OneSpecialChar;
                passwordPolicyNew.OneUpperCase = passwordPolicy.OneUpperCase;
                passwordPolicyNew.PasswordExpiration = passwordPolicy.PasswordExpiration;
                passwordPolicyNew.PasswordExpirationRequiresAdminReset = passwordPolicy.PasswordExpirationRequiresAdminReset;
                passwordPolicyNew.PreventPasswordReuse = passwordPolicy.PreventPasswordReuse;           
                passwordPolicyNew.NoOfPwdToRemember = passwordPolicy.NoOfPwdToRemember;                
                passwordPolicyNew.UpdatedOn = DateTime.UtcNow;
                passwordPolicyNew.UpdatedBy = userId;
                base.context.Update(passwordPolicyNew);
                await base.context.SaveChangesAsync();
                PasswordPolicyModel objPasswordPolicyModel = mapper.Map<PasswordPolicyModel>(passwordPolicyNew);
                return objPasswordPolicyModel;
            }
            return null;          
        }

        public async Task CreatePasswordPolicy(PasswordPolicyModel passwordPolicy)
        {
            PasswordPolicies objPasswordPolicyDataModel = new PasswordPolicies();
            objPasswordPolicyDataModel.AccountId = passwordPolicy.AccountId;
            objPasswordPolicyDataModel.AllowUsersToChangePassword = passwordPolicy.AllowUsersToChangePassword;
            objPasswordPolicyDataModel.ExpiryInDays = passwordPolicy.ExpiryInDays;
            objPasswordPolicyDataModel.IsEmailVerificationRequired = passwordPolicy.IsEmailVerificationRequired;
            objPasswordPolicyDataModel.IsMobileVerificationRequired = passwordPolicy.IsMobileVerificationRequired;
            objPasswordPolicyDataModel.MinPasswordLength = passwordPolicy.MinPasswordLength;
            objPasswordPolicyDataModel.NoOfFailedAttemptsAllowed = passwordPolicy.NoOfFailedAttemptsAllowed;
            objPasswordPolicyDataModel.OneLowerCase = passwordPolicy.OneLowerCase;
            objPasswordPolicyDataModel.OneNumber = passwordPolicy.OneNumber;
            objPasswordPolicyDataModel.OneSpecialChar = passwordPolicy.OneSpecialChar;
            objPasswordPolicyDataModel.OneUpperCase = passwordPolicy.OneUpperCase;
            objPasswordPolicyDataModel.PasswordExpiration = passwordPolicy.PasswordExpiration;
            objPasswordPolicyDataModel.PasswordExpirationRequiresAdminReset = passwordPolicy.PasswordExpirationRequiresAdminReset;
            objPasswordPolicyDataModel.PreventPasswordReuse = passwordPolicy.PreventPasswordReuse;
            objPasswordPolicyDataModel.NoOfPwdToRemember = passwordPolicy.NoOfPwdToRemember;
            objPasswordPolicyDataModel.CreatedOn = DateTime.UtcNow;
            objPasswordPolicyDataModel.CreatedBy = passwordPolicy.AccountId;
            await base.context.PasswordPolicies.AddAsync(objPasswordPolicyDataModel);
            //await base.context.SaveChangesAsync();
        }
    }
}
