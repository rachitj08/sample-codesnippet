using AutoMapper;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Sample.Admin.Model.Account.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class PasswordPolicyRepository : RepositoryBase<PasswordPolicyVM>,  IPasswordPolicyRepository
    {
        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration configuration { get; }

        private readonly IMapper mapper;

        public PasswordPolicyRepository(CloudAcceleratorContext context, IMapper mapper, IConfiguration configuration) : base(context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get PasswordPolicy By AccountId
        /// </summary>
        /// <param name="accountId">The accountId to get PasswordPolicy</param>
        /// <returns></returns>
        public async Task<PasswordPolicyVM> GetPasswordPolicy()
        {
            return await base.context.PasswordPolicies
                .Select(x=> new PasswordPolicyVM()
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
        /// CreatePasswordPolicy for defaultuser stup
        /// </summary>
        /// <param name="passwordPolicy"></param>
        /// <returns></returns>
        public async Task<int> CreateOrUpdatePasswordPolicy(PasswordPolicyVM passwordPolicy)
        {
            int result = 0;
            PasswordPolicies objPasswordPolicyDataModel = mapper.Map<PasswordPolicies>(passwordPolicy);
            objPasswordPolicyDataModel.CreatedOn = DateTime.UtcNow;
            objPasswordPolicyDataModel.UpdatedOn = DateTime.UtcNow;
            var pawwordPolicy = context.PasswordPolicies.FirstOrDefault(x => x.AccountId == passwordPolicy.AccountId);
            if(pawwordPolicy != null)
            {
                pawwordPolicy.MinPasswordLength = passwordPolicy.MinPasswordLength;
                pawwordPolicy.OneUpperCase = passwordPolicy.OneUpperCase;
                pawwordPolicy.OneLowerCase = passwordPolicy.OneLowerCase;
                pawwordPolicy.OneNumber = passwordPolicy.OneNumber;
                pawwordPolicy.OneSpecialChar = passwordPolicy.OneSpecialChar;
                pawwordPolicy.ExpiryInDays = passwordPolicy.ExpiryInDays;
                pawwordPolicy.PasswordExpirationRequiresAdminReset = passwordPolicy.PasswordExpirationRequiresAdminReset;
                pawwordPolicy.AllowUsersToChangePassword = passwordPolicy.AllowUsersToChangePassword;
                pawwordPolicy.PreventPasswordReuse = passwordPolicy.PreventPasswordReuse;
                pawwordPolicy.NoOfPwdToRemember = passwordPolicy.NoOfPwdToRemember;
                pawwordPolicy.IsMobileVerificationRequired = passwordPolicy.IsMobileVerificationRequired;
                pawwordPolicy.IsEmailVerificationRequired = passwordPolicy.IsEmailVerificationRequired;
                pawwordPolicy.NoOfFailedAttemptsAllowed = passwordPolicy.NoOfFailedAttemptsAllowed;
                base.context.PasswordPolicies.Update(pawwordPolicy);
            }
            else
                base.context.PasswordPolicies.Add(objPasswordPolicyDataModel);

            await base.context.SaveChangesAsync();
            result = 1;
            return result;
        }

       
    }
}
