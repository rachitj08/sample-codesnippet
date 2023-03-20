using AutoMapper;
using Common.Enum;
using Common.Enum.StorageEnum;
using Common.Model;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Utilities;
using Utilities.EmailHelper;
using Utilities.PBKDF2Hashing;
using Utility;
using Sample.Customer.Model;
using Sample.Customer.Model.Model;
using Sample.Customer.Model.Model.StorageModel;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Service.Infrastructure.Repository;
using REGEX = System.Text.RegularExpressions;
using User = Sample.Customer.Service.Infrastructure.DataModels.Users;
using UserRight = Sample.Customer.Service.Infrastructure.DataModels.UserRights;

namespace Sample.Customer.Service.ServiceWorker
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserRepository usersRepository;

        private readonly IUserRightsRepository usersRightsRepository;

        private readonly IUserGroupMappingRepository usersGroupMappingRepository;

        private readonly IPasswordPolicyRepository passwordPolicyRepository;

        private readonly IPasswordHistoryRepository passwordHistoryRepository;

        private readonly IGroupRepository groupsRepository;

        private readonly IMapper mapper;

        private readonly ICommonHelper commonHelper;

        private readonly IEmailHelper emailHelper;

        private readonly ResetPasswordConfig resetPasswordConfig;

        private readonly ISMSHelper smsHelper;
        
        private readonly SMSConfig smsConfig;

        private readonly SendVerificationMailConfig sendVerificationMailConfig;
        private readonly IAuthenticationService authenticationService;

        private readonly CustomerBaseUrlsConfig urls;

        /// <summary>
        ///  Load Authentication configuration
        /// </summary>
        private readonly AuthenticationConfig authenticationConfig;
        /// <summary>
        /// Users Service constructor to Inject dependency
        /// </summary>
        /// <param name="unitOfWork"> unit of work member repository</param>
        /// <param name="usersRepository"> user repository for user data</param>
        public UserService(IUnitOfWork unitOfWork, IUserRepository usersRepository,
            IUserRightsRepository usersRightsRepository, IUserGroupMappingRepository userGroupMappingRepository,
            IPasswordPolicyRepository passwordPolicyRepository, IPasswordHistoryRepository passwordHistoryRepository,
            IGroupRepository objIGroupRepository, ISMSHelper smsHelper, IOptions<SMSConfig> smsConfig, 
            IOptions<AuthenticationConfig> authenticationConfig, IMapper mapper, ICommonHelper commonHelper,
            IEmailHelper emailHelper, IOptions<ResetPasswordConfig> resetPasswordConfig, IOptions<SendVerificationMailConfig> sendVerificationMailConfig
            , IOptions<CustomerBaseUrlsConfig> config, IAuthenticationService authenticationService
            )
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(usersRepository), usersRepository);
            Check.Argument.IsNotNull(nameof(passwordPolicyRepository), passwordPolicyRepository);
            Check.Argument.IsNotNull(nameof(passwordHistoryRepository), passwordHistoryRepository);
            Check.Argument.IsNotNull(nameof(resetPasswordConfig), resetPasswordConfig);
            Check.Argument.IsNotNull(nameof(commonHelper), commonHelper);
            Check.Argument.IsNotNull(nameof(emailHelper), emailHelper);
            Check.Argument.IsNotNull(nameof(smsHelper), smsHelper);
            Check.Argument.IsNotNull(nameof(smsConfig), smsConfig);
            Check.Argument.IsNotNull(nameof(authenticationConfig), authenticationConfig);
            Check.Argument.IsNotNull(nameof(sendVerificationMailConfig), sendVerificationMailConfig);
            Check.Argument.IsNotNull(nameof(authenticationService), authenticationService);

            this.unitOfWork = unitOfWork;
            this.usersRepository = usersRepository;
            this.usersRightsRepository = usersRightsRepository;
            this.usersGroupMappingRepository = userGroupMappingRepository;
            this.passwordPolicyRepository = passwordPolicyRepository;
            this.passwordHistoryRepository = passwordHistoryRepository;
            this.mapper = mapper;
            this.commonHelper = commonHelper;
            this.emailHelper = emailHelper;
            this.resetPasswordConfig = resetPasswordConfig.Value;
            this.groupsRepository = objIGroupRepository;
            this.smsHelper = smsHelper;
            this.smsConfig = smsConfig.Value;
            this.authenticationConfig = authenticationConfig.Value;
            this.sendVerificationMailConfig = sendVerificationMailConfig.Value;
            this.urls = config.Value;
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Get All Users Group Mappings
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserGroupMappings>> GetAllUsersGroupMappings()
        {
            return await this.usersRepository.GetAllUsersGroupMappings();
        }

        /// <summary>
        /// User Group for each user Id
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<List<UserGroup>> GetUserGroupsByUserId(long userId)
        {
            return await this.usersRepository.GetUserGroupsByUserId(userId);
        }

        /// <summary>
        /// Check Module Permission By UserId
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="moduleId">module id</param>
        /// <returns></returns>
        public async Task<bool> CheckModulePermissionByUserId(long userId, long moduleId)
        {
            // Fetch data for user modules according to User Rights
            var userRightCount = await this.usersRepository.CheckModuleForUserRight(userId, moduleId);
            if (userRightCount > 0) return true;

            // Fetch data for user modules according to user Rights
            userRightCount = await this.usersRepository.CheckModuleForUserGroup(userId, moduleId);
            return (userRightCount > 0);
        }

        /// <summary>
        /// Build User Navigation By UserId
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public async Task<ResponseResult<List<long>>> BuildUserNavigationByUserId(long userId, long accountId)
        {
            var responseResult = new ResponseResult<List<long>>();
            var userNavigations = new List<long>();
            // Fetch data for user modules according to usergroup mapping
            List<long> userModuleNavigation = await this.usersRepository.GetUserGroupRights(userId, accountId);
            if (userModuleNavigation.Count > 0)
            {
                userNavigations = userModuleNavigation;
            }
            // Fetch data for user modules according to user Rights
            List<UserRightDetail> userRightsNavigation = await this.usersRepository.GetUserRights(userId, accountId);
            long ModuleId;
            if (userRightsNavigation.Count > 0)
            {
                foreach (var userRights in userRightsNavigation)
                {
                    ModuleId = userRights.ModuleId;
                    //Add if IsPermission true in user rights for module
                    if (userRights.IsPermission)
                    {
                        if (!userNavigations.Exists(x => x == ModuleId))
                        {
                            userNavigations.Add(userRights.ModuleId);
                        }
                    }
                    else
                    {
                        //Remove if IsPermission false in user rights for module 
                        if (userNavigations.Exists(x => x == ModuleId))
                        {
                            var navigation = userNavigations.Find(x => x == userRights.ModuleId);
                            userNavigations.Remove(navigation);
                        }
                    }

                }
            }
            if (userNavigations.Count > 0)
            {
                responseResult.Message = ResponseMessage.RecordFetched;
                responseResult.ResponseCode = ResponseCode.RecordFetched;
                responseResult.Data = userNavigations;
            }
            return responseResult;
        }

        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="OldPassword"></param>
        /// <returns>return response result</returns> 
        public async Task<ResponseResult<string>> ChangePassword(ChangePassword changePassword, long loggedInUserId)
        {
            var user = await this.usersRepository.GetUserByUserId(changePassword.AccountId, changePassword.UserId);

            // If user not found, return error.
            if (user == null)
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            // If user found but user status not activated, return error.
            if (user.UserStatus != (int)UserStatus.Active)
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.Unauthorized,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            PBKDF2Cryptography cryptography = new PBKDF2Cryptography();
            // Validate Old Password
            bool password = cryptography.ValidatePassword(changePassword.OldPassword, user.PasswordHash, user.PasswordSalt);

            // If password didn't match, return error.
            if (!password)
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.Unauthorized,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.Unauthorized
                    }
                };
            }

            // Validate New Password
            var validationErrorMsg = await ValidatePassword(cryptography, user.AccountId, user.UserId, changePassword.Password,false,null);

            if (!string.IsNullOrWhiteSpace(validationErrorMsg))
            {
                return new ResponseResult<string>()
                {
                    Message = validationErrorMsg,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = validationErrorMsg
                    }
                };
            }

            //Change Password
            PBKDF2Password hashAccount = cryptography.CreateHash(changePassword.Password);
            user.PasswordHash = hashAccount.Hash;
            user.PasswordSalt = hashAccount.Salt;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = loggedInUserId;
            var passwordHistory = new PasswordHistories()
            {
                AccountId = user.AccountId,
                UserId = user.UserId,
                LastUsedOn = DateTime.UtcNow,
                PasswordHash = hashAccount.Hash,
                PasswordSalt = hashAccount.Salt
            };
            await this.usersRepository.ChangePassword(user);
            await this.passwordHistoryRepository.CreatePasswordHistory(passwordHistory);
            unitOfWork.Commit();

            return new ResponseResult<string>()
            {
                Message = "Password have been changed successfully",
                ResponseCode = ResponseCode.RecordSaved,
                Data = "Password have been changed successfully"
            };
        }

        /// <summary>
        /// Validate Password
        /// </summary>
        /// <param name="cryptography"></param>
        /// <param name="account"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<string> ValidatePassword(PBKDF2Cryptography cryptography, long account, long userId, string password,bool isCalledForDefaultUser, PasswordPolicyModel objPasswordPolicyModel)
        {
            // Get Password Policy.
            PasswordPolicyModel passwordPolicy = new PasswordPolicyModel();
            if(isCalledForDefaultUser && objPasswordPolicyModel != null)
            {
                // this is for default user setup
                passwordPolicy = objPasswordPolicyModel;
            }
            else
            {
                passwordPolicy = await passwordPolicyRepository.GetPasswordPolicyByAccountId(account);
            }
            

            // If Password Policy not found for account, return error.
            if (passwordPolicy == null)
            {
                return ResponseMessage.InternalServerError;
            }

            // If User Allowed To Change Password, return error.
            if (!passwordPolicy.AllowUsersToChangePassword)
            {
                return ResponseMessage.Unauthorized;
            }

            // Check minimum Password Length
            if (passwordPolicy.MinPasswordLength > 0
                && password.Length < passwordPolicy.MinPasswordLength)
            {
                return "Password Length is less than " + passwordPolicy.MinPasswordLength;
            }

            // Check One lower case character in Password
            if (passwordPolicy.OneLowerCase
                && password.Where(char.IsLower).Count() < 1)
            {
                return "Password does not have any lower case character.";
            }

            // Check One Upper case character in Password
            if (passwordPolicy.OneUpperCase
                && password.Where(char.IsUpper).Count() < 1)
            {
                return "Password does not have any Upper case character.";
            }

            // Check One number in Password
            if (passwordPolicy.OneNumber
                && password.Where(char.IsNumber).Count() < 1)
            {
                return "Password does not have any number.";
            }

            // Check One special character in Password
            if (passwordPolicy.OneSpecialChar
                && !REGEX.Regex.IsMatch(password, @"[(\@|\$|\~|\#|\&|\^|\!)]"))
            {
                return "Password does not have any special character.";
            }

            // Check If Password already used
            if (userId > 0 && passwordPolicy.PreventPasswordReuse && passwordPolicy.NoOfPwdToRemember > 0)
            {
                var passwordHistoryList = await this.passwordHistoryRepository.GetPasswordHistoryByUserId(userId, passwordPolicy.NoOfPwdToRemember);
                if (passwordHistoryList != null && passwordHistoryList.Count > 0)
                {
                    foreach (var item in passwordHistoryList)
                    {
                        if (cryptography.ValidatePassword(password, item.PasswordHash, item.PasswordSalt))
                        {
                            return $"Password has been already used. You can not use last {passwordPolicy.NoOfPwdToRemember} password.";
                        }
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<UserVM>> GetAllUsers(long accountId, string ordering, int offset, int pageSize, int pageNumber, bool all) => await this.usersRepository.GetAllUsers(accountId, ordering, offset, pageSize, pageNumber, all);

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <returns></returns>
        public async Task<UserVM> GetUserById(long accountId, long userId)
        {
            var user = await this.usersRepository.GetUser(accountId, userId);

            if (user != null)
            {
                var userDetail = new UserVM()
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    Mobile = user.Mobile,
                    UserName = user.UserName,
                    UserStatus = user.UserStatus,
                    StripeCustomerId = user.StripeCustomerId,
                    UserRights = new List<UserRightModel>(),
                    UserGroups = new List<UserGroupMappingModel>()
                };

                if (user.UserRights != null && user.UserRights.Count > 0)
                {
                    userDetail.UserRights = user.UserRights.Select(x => new UserRightModel()
                    {
                        AccountId = x.AccountId,
                        IsPermission = x.IsPermission,
                        UserRightId = x.UserRightId,
                        ModuleId = x.ModuleId
                    }).ToList();
                }

                if (user.UserGroupMappings != null && user.UserGroupMappings.Count > 0)
                {
                    userDetail.UserGroups = user.UserGroupMappings.Select(x => new UserGroupMappingModel()
                    {
                        AccountId = x.AccountId,
                        GroupId = x.GroupId,
                        Description = x.Group?.Description,
                        Name = x.Group?.Name,
                        Status = (x.Group != null ? x.Group.Status : (Int16)0)
                    }).ToList();
                }
                return userDetail;
            }
            return null;
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <returns></returns>
        public async Task<UserBasicDataModel> GetUserDetails(long accountId, long userId)
        {
            var user = await this.usersRepository.GetUser(accountId, userId);

            if (user != null)
            {
                var userDetail = new UserBasicDataModel()
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    Mobile = user.Mobile,
                    UserName = user.UserName,
                    AccountId = user.AccountId

                };

                return userDetail;
            }
            return null;
        }
        public async Task<List<UserBasicDataModel>> GetUsersByAccountId(long accountId)
        {
            var userDetail = await this.usersRepository.GetUsersByAccountId(accountId);
            var data = userDetail.Select(x => new UserBasicDataModel
            {
                AccountId = x.AccountId,
                UserId = x.UserId,
                UserName = x.UserName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Mobile = x.Mobile,
                EmailAddress = x.EmailAddress,

            }).ToList();
            return data;
        }

        // TODO: Remove It After Initial Testing
        public void KafkaTest(CreateUserModel model)
        {
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            System.Diagnostics.Debug.WriteLine(text);
            //Console.WriteLine(text);
        }


        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserProfileModel>> GetUserProfile(long accountId, long userId)
        {
            var user = await this.usersRepository.GetUserByUserId(accountId, userId);
            if (user == null)
            {
                return new ResponseResult<UserProfileModel>()
                {
                    Message = "Invalid User Details",
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }

            return new ResponseResult<UserProfileModel>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = new UserProfileModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DOB = user.DateOfBirth,
                    MobileCode = user.MobileCode,
                    Mobile = user.Mobile,
                    EmailAddress = user.EmailAddress,
                    IsEmailVerified = user.IsEmailVerified,
                    DrivingLicense = user.DrivingLicense,
                    ImagePath = $@"{this.urls.StorageRootPath}{user.ImagePath}"
                }
            };
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="model">User Profile Model</param>
        /// <param name="accountId">accountId</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> UpdateUserProfile(SaveUserProfileModel model, long accountId, long userId)
        {
            var user = await this.usersRepository.GetUserByUserId(accountId, userId);
            if (user == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                errorDetails.Add("firstName", new string[] { "This field may not be blank." });
            }
            else if (model.FirstName.Length > 50)
            {
                errorDetails.Add("firstName", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<bool>()
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

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DrivingLicense = model.DrivingLicense;
            user.DateOfBirth = model.DOB;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = userId; 
            
            await usersRepository.UpdateUser(user);
            if (unitOfWork.CommitWithStatus() > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = true
                };
            }
            else
            {
                return new ResponseResult<bool>()
                {
                    Message = "User data is not saved",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }

        /// <summary>
        /// Update User Profile Image
        /// </summary>
        /// <param name="imagePath">image Path</param>
        /// <param name="accountId">accountId</param>
        /// <param name="loggedInUserId">userId</param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> UpdateUserProfileImage(UserProfileImageVM model, long accountId, long loggedInUserId)
        {
            ImageResponseView imageResponseView = new ImageResponseView();
            MediaResponseView response = new MediaResponseView();
            string uploadedFileName = string.Empty;
            if (model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = "Invalid request",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(model.ProfileDocument.FileName))
            {
                errorDetails.Add("fileName", new string[] { "This field may not be blank." });
            }

            if (model.ProfileDocument.Filebytes == null || model.ProfileDocument.Filebytes.Length < 1)
            {
                errorDetails.Add("fileByte", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        Detail = errorDetails
                    }
                };
            } 

            var user = await this.usersRepository.GetUserByUserId(accountId, loggedInUserId);
            if (user == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            // Upload Image
            if (model != null && loggedInUserId != 0)
            {
                if (model.ProfileDocument.Filebytes.Length > 0)
                {
                    var mediaUploadView = new MediaUploadView();
                    mediaUploadView.Documents = new List<Document>();
                    mediaUploadView.ReferenceId = loggedInUserId;
                    mediaUploadView.ReferenceType = (int)MediaReferenceEnum.Member;
                    mediaUploadView.CreatedBy = loggedInUserId;
                    mediaUploadView.Documents.Add(model.ProfileDocument);
                    imageResponseView = null;
                    if (imageResponseView != null)
                    {
                        response.Data = imageResponseView.SavedPathList;
                        response.ImageHeight = imageResponseView.ImageHeight;
                        response.ImageWidth = imageResponseView.ImageWidth;

                        response.Message = "Success";
                        response.Type = "Success";
                    }
                    else
                    {
                        response.Data = null;
                        response.Message = "Failed";
                        response.Type = "Failed";
                    }
                    if (response.Type == "Success" && response.Data != null && response.Data.Count > 0)
                    {
                       uploadedFileName = Convert.ToString(response.Data[0]);
                    }
                   
                }
            }

            // Validate Model
            if (string.IsNullOrWhiteSpace(uploadedFileName))
            {
                return new ResponseResult<bool>()
                {
                    Message = "Could not able to save file",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        Detail = errorDetails
                    }
                };
            }

            user.ImagePath = uploadedFileName;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = loggedInUserId;

            await usersRepository.UpdateUser(user);
            if (unitOfWork.CommitWithStatus() > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = true
                };
            }
            else
            {
                return new ResponseResult<bool>()
                {
                    Message = "User data is not saved",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }

        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserVM>> CreateUser(CreateUserModel model, long accountId, long loggedInUserId, string deviceId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            //if (string.IsNullOrWhiteSpace(model.FirstName))
            //{
            //    errorDetails.Add("firstName", new string[] { "This field may not be blank." });
            //}
            //else if (model.FirstName.Length > 50)
            //{
            //    errorDetails.Add("firstName", new string[] { "Ensure this field has no more than 50 characters." });
            //}
            
            if (string.IsNullOrWhiteSpace(model.Mobile))
            {
                errorDetails.Add("mobile", new string[] { "This field may not be blank." });
            }
            else if (await this.usersRepository.GetUserByUserMobile(model.Mobile, accountId) != null)
            {
                errorDetails.Add("mobile", new string[] { "This mobile number is already being used for another user." });
            }

            if (string.IsNullOrWhiteSpace(model.EmailAddress))
            {
                errorDetails.Add("emailAddress", new string[] { "This field may not be blank." });
            }
            else if (await this.usersRepository.GetUserByEmailId(model.EmailAddress, accountId) != null)
            {
                errorDetails.Add("emailAddress", new string[] { "This email address is already being used for another user." });
            }

            var cryptography = new PBKDF2Cryptography();
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var passwordError = await ValidatePassword(cryptography, accountId, 0, model.Password, false, null);
                if (!string.IsNullOrWhiteSpace(passwordError))
                    errorDetails.Add("password", new string[] { passwordError });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<UserVM>()
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

            var userModel = new Users()
            {
                AccountId = accountId,
                UserName = model.EmailAddress,
                EmailAddress = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Mobile = model.Mobile,
                MobileCode = model.MobileCode,
                UserStatus = 1,
                FailedLoginAttempts = 0,
                IsMobileNumberVerified = true,
                IsEmailVerified = false,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUserId,
                IntreastedInLanding = model.IntreastedInLanding,
                IntreastedInRenting = model.IntreastedInRenting,
                ExternalUserId = model.ExternalUserId,
                AuthenticationCategory = model.AuthenticationCategory,
                DeviceId = deviceId,
                AddressId=model.AddressId
            };

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var hashAccount = cryptography.CreateHash(model.Password);
                userModel.PasswordHash = hashAccount.Hash;
                userModel.PasswordSalt = hashAccount.Salt;
            }
            
            var user = await this.usersRepository.CreateUser(userModel);

            if (user == null)
            {
                return new ResponseResult<UserVM>()
                {
                    Message = "Unable to create user.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            if (model.UserRights != null && model.UserRights.Count > 0)
            {
                var userRights = model.UserRights.Select(x =>
                        new UserRights()
                        {
                            AccountId = accountId,
                            UserRightId = x.UserRightId,
                            ModuleId = x.ModuleId,
                            UserId = user.UserId,
                            IsPermission = x.IsPermission,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = loggedInUserId
                        }
                    ).ToList();
                await usersRightsRepository.AddList(userRights, 0);
            }

            // Add Default User Group
            if (!string.IsNullOrWhiteSpace(authenticationConfig.DefaultUserGroup) 
                && (model.UserRights == null || model.UserRights.Count < 1) 
                && (model.Groups == null || model.Groups.Count < 1))
            {
                var group = await this.groupsRepository.GetGroupByName(authenticationConfig.DefaultUserGroup, accountId);

                if (group == null || group.GroupId < 1)
                {
                    return new ResponseResult<UserVM>()
                    {
                        Message = "Customer group is missing",
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError,
                        }
                    };
                }
                model.Groups = new List<long>();
                model.Groups.Add(group.GroupId);
            }

            if (model.Groups != null && model.Groups.Count > 0)
            {
                var groups = model.Groups.Select(x =>
                       new UserGroupMappings()
                       {
                           AccountId = accountId,
                           GroupId = x,
                           UserId = user.UserId,
                           CreatedOn = DateTime.UtcNow,
                           CreatedBy = loggedInUserId,
                       }
                   ).ToList();
                await usersGroupMappingRepository.AddList(groups, 0);
            }
            this.unitOfWork.Commit();

            var response = new UserVM();
            response = mapper.Map<UserVM>(model);
            response.UserId = user != null ? user.UserId : 0;
            return new ResponseResult<UserVM>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = response
            };
        }


        /// <summary>
        /// To Update existing User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserVM>> UpdateUser(long userId, long accountId, UsersModel model, long loggedInUserId)
        {
            var user = await this.usersRepository.GetUser(accountId, userId);
            if (user == null)
            {
                return new ResponseResult<UserVM>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = null
                };
            }

            if (model.DefaultDashboardId.HasValue)
            {

              //  user.DefaultDashboardId = model.DefaultDashboardId.Value;
                user.UpdatedBy = loggedInUserId;
                user.UpdatedOn = DateTime.UtcNow;

                await this.usersRepository.UpdateUser(user);
                this.unitOfWork.Commit();
                var userResponse = mapper.Map<UserVM>(model);
                userResponse.UserId = user != null ? user.UserId : 0;
                return new ResponseResult<UserVM>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = userResponse
                };
            }


            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                errorDetails.Add("userName", new string[] { "This field may not be blank." });
            }
            else if (model.UserName.Length > 255)
            {
                errorDetails.Add("userName", new string[] { "Ensure this field has no more than 255 characters." });
            }

            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                errorDetails.Add("firstName", new string[] { "This field may not be blank." });
            }
            else if (model.FirstName.Length > 100)
            {
                errorDetails.Add("firstName", new string[] { "Ensure this field has no more than 100 characters." });
            }

            var existingUser = await this.usersRepository.GetUserByUserName(model.UserName, accountId);
            if (existingUser != null && existingUser.UserId != userId)
            {
                errorDetails.Add("userName", new string[] { "This user name is already being used for another user." });
            }
            //addded by Raj for validate Email and Mobile
            var existingEmail = await this.usersRepository.GetUserByEmailId(model.EmailAddress, accountId);
            if (existingEmail != null && existingEmail.UserId != userId)
            {
                errorDetails.Add("emailAddress", new string[] { "This email address is already being used for another user." });
            }
            var existingMobile = await this.usersRepository.GetUserByUserMobile(model.Mobile, accountId);
            if (existingMobile != null && existingMobile.UserId != userId)
            {
                errorDetails.Add("mobile", new string[] { "This mobile is already being used for another user." });
            }
            if (errorDetails.Count > 0)
            {
                return new ResponseResult<UserVM>()
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

            UserVM response = new UserVM();


            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.EmailAddress = model.EmailAddress;
            user.Mobile = model.Mobile;
            user.UserName = model.UserName;
            user.UserStatus = (short)model.UserStatus;
            user.UpdatedBy = loggedInUserId;
            user.UpdatedOn = DateTime.UtcNow;

            await this.usersRepository.UpdateUser(user);

            // Group Rights
            var groupsRights = new List<long>();
            // Fetch data for user modules according to usergroup mapping
            List<long> userModuleNavigation = await this.usersRepository.GetUserGroupRights(userId, accountId);
            if (userModuleNavigation.Count > 0)
            {
                groupsRights = userModuleNavigation;
            }

            // User Groups
            var groups = new List<UserGroupMappings>();
            if (model.Groups != null && model.Groups.Count > 0)
            {
                groups = model.Groups.Select(x =>
                       new UserGroupMappings()
                       {
                           AccountId = accountId,
                           GroupId = x,
                           UserId = user.UserId,
                           CreatedOn = DateTime.UtcNow,
                           UpdatedOn = DateTime.UtcNow,
                           CreatedBy = loggedInUserId,
                           UpdatedBy = loggedInUserId
                       }
                   ).ToList();


            }
            await usersGroupMappingRepository.AddList(groups, user.UserId);

            // Update UserRights
            var userRights = new List<UserRight>();
            if (model.UserRights != null && model.UserRights.Count > 0)
            {
                userRights = model.UserRights.Select(x =>
                        new UserRight()
                        {
                            AccountId = accountId,
                            UserRightId = x.UserRightId,
                            ModuleId = x.ModuleId,
                            UserId = user.UserId,
                            IsPermission = x.IsPermission,
                            CreatedOn = DateTime.UtcNow,
                            UpdatedOn = DateTime.UtcNow,
                            CreatedBy = loggedInUserId,
                            UpdatedBy = loggedInUserId
                        }
                    ).ToList();
            }

            var finalUserRights = userRights;

            if (!model.IsProvider)
            {
                foreach (var objgroupsRights in groupsRights)
                {
                    var checkModuleRightExist = userRights.FirstOrDefault(x => x.ModuleId == objgroupsRights);
                    if (checkModuleRightExist == null)
                    {
                        finalUserRights.Add(
                            new UserRight()
                            {
                                AccountId = accountId,
                                ModuleId = objgroupsRights,
                                UserId = user.UserId,
                                IsPermission = false,
                                CreatedOn = DateTime.UtcNow,
                                UpdatedOn = DateTime.UtcNow,
                                CreatedBy = loggedInUserId,
                                UpdatedBy = loggedInUserId
                            });
                    }
                }

                await usersRightsRepository.AddList(finalUserRights, user.UserId);
            }

            this.unitOfWork.Commit();


            response = mapper.Map<UserVM>(model);
            response.UserId = user != null ? user.UserId : 0;
            return new ResponseResult<UserVM>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = response
            };
        }

        /// <summary>
        /// To Update User Partially
        /// </summary>
        /// /// <param name="userId">User ID</param>
        /// <param name="model">New user object</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserVM>> UpdatePartialUser(long userId, long accountId, UsersModel model, long loggedInUserId)
        {
            return await UpdateUser(userId, accountId, model, loggedInUserId);
        }

        /// <summary>
        /// To delete existing User
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<long> DeleteUser(long userId)
        {
            await this.usersRightsRepository.Delete(userId);
            await this.usersGroupMappingRepository.Delete(userId);
            return await this.usersRepository.DeleteUser(userId);
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> ForgotPassword(ForgotPasswordModel model)
        {
            var response = new ResponseResult<SuccessMessageModel>();
            if (model == null || model.AccountId < 1 || string.IsNullOrWhiteSpace(model.Email))
            {
                response.Message = ResponseMessage.ValidationFailed;
                response.ResponseCode = ResponseCode.ValidationFailed;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed
                };
                return response;
            }

            // Check Permissions
            // Get Password Policy.
            var passwordPolicy = await passwordPolicyRepository.GetPasswordPolicyByAccountId(model.AccountId);

            // If Password Policy not found for account, return error.
            if (passwordPolicy == null)
            {
                response.Message = ResponseMessage.InternalServerError;
                response.ResponseCode = ResponseCode.InternalServerError;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return response;
            }

            // If User Allowed To Change Password, return error.
            if (!passwordPolicy.AllowUsersToChangePassword)
            {
                response.Message = ResponseMessage.Unauthorized;
                response.ResponseCode = ResponseCode.Unauthorized;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.Unauthorized
                };
                return response;
            }

            // Get User By Email Id
            var user = await usersRepository.GetUserByEmailId(model.Email, model.AccountId);
            if (user == null)
            {
                response.Message = ResponseMessage.ValidationFailed;
                response.ResponseCode = ResponseCode.ValidationFailed;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed
                };
                return response;
            }

            var lastUpdatedDate = user.CreatedOn;
            if (user.UpdatedOn != null)
            {
                lastUpdatedDate = (DateTime)user.UpdatedOn;
            }

            var token = user.UserId + "~!|" + user.AccountId + "~!|" + lastUpdatedDate.Ticks + "~!|" + DateTime.Now;
            token = HttpUtility.UrlEncode(commonHelper.EncryptString(token, resetPasswordConfig.EncryptKey));
            var uid = HttpUtility.UrlEncode(commonHelper.EncryptString(user.UserId.ToString(), resetPasswordConfig.EncryptKey));
            var setPasswordURL = resetPasswordConfig.URL.Replace("##TOKEN##", token).Replace("##UID##", uid);


            var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            var filePath = Path.Combine("Helpers", "Docs", "EmailTemplate", "ForgotPasswordFormat.html");
            var fileInfo = provider.GetFileInfo(filePath);
            if (!fileInfo.Exists)
            {
                response.Message = ResponseMessage.InternalServerError;
                response.ResponseCode = ResponseCode.InternalServerError;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return response;
            }

            var bodyHTML = "";
            using (var fs = fileInfo.CreateReadStream())
            {
                using (var sr = new StreamReader(fs))
                {
                    bodyHTML = sr.ReadToEnd();
                }
            }

            if (string.IsNullOrWhiteSpace(bodyHTML))
            {
                response.Message = ResponseMessage.InternalServerError;
                response.ResponseCode = ResponseCode.InternalServerError;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return response;
            }

            bodyHTML = bodyHTML.Replace("##url##", setPasswordURL);

            emailHelper.SendMail(model.Email, "Password Reset", bodyHTML);

            response.Message = "Password reset link has been send to the email.";
            response.ResponseCode = ResponseCode.RecordFetched;
            response.Data = new SuccessMessageModel()
            {
                Message = "Password reset link has been send to the email."
            };
            return response;
        }

        /// <summary>
        /// Set Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> SetPassword(SetPasswordModel model, long loggedInUserId)
        {
            var response = new ResponseResult<SuccessMessageModel>();

            // Validate required fields
            if (model == null)
            {
                response.Message = ResponseMessage.ValidationFailed;
                response.ResponseCode = ResponseCode.ValidationFailed;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed
                };
                return response;
            }

            var errors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                errors.Add("password", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(model.Uid))
            {
                errors.Add("uid", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(model.Token))
            {
                errors.Add("token", new string[] { "This field may not be blank." });
            }

            if (errors.Count > 0)
            {
                response.Message = ResponseMessage.ValidationFailed;
                response.ResponseCode = ResponseCode.ValidationFailed;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    Detail = errors
                };
                return response;
            }

            // Decrypt Token & UserId
            var uid = HttpUtility.UrlDecode(commonHelper.DecryptString(model.Uid, resetPasswordConfig.EncryptKey));
            var token = HttpUtility.UrlDecode(commonHelper.DecryptString(model.Token, resetPasswordConfig.EncryptKey));

            if (string.IsNullOrWhiteSpace(uid) || string.IsNullOrWhiteSpace(token)
                || !Int64.TryParse(uid, out var userId))
            {
                response.Message = ResponseMessage.ValidationFailed;
                response.ResponseCode = ResponseCode.ValidationFailed;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed
                };
                return response;
            }

            var tokenValues = token.Split("~!|");

            // Validate Token values
            if (tokenValues.Length < 4 || !Int64.TryParse(tokenValues[0], out var tokenUserId)
                || tokenUserId != userId || !Int64.TryParse(tokenValues[1], out var accountId)
                || accountId < 1 || !Int64.TryParse(tokenValues[2], out var lastUpdatedOn)
                || !DateTime.TryParse(tokenValues[3], out var tokenCreatedOn))
            {
                response.Message = ResponseMessage.Unauthorized;
                response.ResponseCode = ResponseCode.Unauthorized;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.Unauthorized
                };
                return response;
            }

            // Validate Token date
            if (tokenCreatedOn.AddHours(resetPasswordConfig.ExpiryTimeInHours) < DateTime.Now)
            {
                response.Message = "Reset Password link has been expired.";
                response.ResponseCode = ResponseCode.Unauthorized;
                response.Error = new ErrorResponseResult()
                {
                    Message = "Reset Password link has been expired."
                };
                return response;
            }

            // Validate User Last Updated On date
            var user = await usersRepository.GetUserByUserId(accountId, userId);

            if (user == null)
            {
                response.Message = "Reset Password link has been expired.";
                response.ResponseCode = ResponseCode.Unauthorized;
                response.Error = new ErrorResponseResult()
                {
                    Message = "Reset Password link has been expired."
                };
                return response;
            }

            // Validate last updated date
            var userLastUpdatedDate = user.CreatedOn;
            if (user.UpdatedOn != null)
            {
                userLastUpdatedDate = (DateTime)user.UpdatedOn;
            }

            if (lastUpdatedOn != userLastUpdatedDate.Ticks)
            {
                response.Message = "Reset Password link has been expired.";
                response.ResponseCode = ResponseCode.Unauthorized;
                response.Error = new ErrorResponseResult()
                {
                    Message = "Reset Password link has been expired."
                };
                return response;
            }

            // Validate New Password
            var cryptography = new PBKDF2Cryptography();
            var validationErrorMsg = await ValidatePassword(cryptography, user.AccountId, user.UserId, model.Password, false, null);

            if (!string.IsNullOrWhiteSpace(validationErrorMsg))
            {
                response.Message = validationErrorMsg;
                response.ResponseCode = ResponseCode.InternalServerError;
                response.Error = new ErrorResponseResult()
                {
                    Message = validationErrorMsg
                };
                return response;
            }

            //Change Password
            var hashAccount = cryptography.CreateHash(model.Password);
            user.PasswordHash = hashAccount.Hash;
            user.PasswordSalt = hashAccount.Salt;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = loggedInUserId;
            var passwordHistory = new PasswordHistories()
            {
                AccountId = user.AccountId,
                UserId = user.UserId,
                LastUsedOn = DateTime.UtcNow,
                PasswordHash = hashAccount.Hash,
                PasswordSalt = hashAccount.Salt,
            };
            await this.usersRepository.ChangePassword(user);
            await this.passwordHistoryRepository.CreatePasswordHistory(passwordHistory);
            unitOfWork.Commit();

            response.Message = "Password reset complete.";
            response.ResponseCode = ResponseCode.RecordSaved;
            response.Data = new SuccessMessageModel()
            {
                Message = "Password reset complete."
            };
            return response;
        }

        /// <summary>
        /// To Verify Token
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <returns></returns>
        public async Task<ResponseResult> VerifyToken(VerifyTokenModel tokenModel)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(tokenModel.Token))
            {
                errorDetails.Add("token", new string[] { "This field may not be blank." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult()
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

            TokenRefreshModel refreshToken = null;

            if (await this.usersRepository.VerifyToken(tokenModel))
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = refreshToken
                };
            }

            return new ResponseResult()
            {
                Message = ResponseMessage.Unauthorized,
                ResponseCode = ResponseCode.Unauthorized,
                Error = new ErrorResponseResult()
                {
                    Message = "token_not_valid"
                },
                Data = null
            };
        }

        /// <summary>
        /// This api is used for Verify User Email Address for existing User
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult> VerifyUserEmailAddress(long accountID, long userId)
        {
            var user = await this.usersRepository.GetUser(accountID, userId);

            if (user != null && user.UserId > 0)
            {
                user.IsEmailVerified = true;
                user.UpdatedBy = accountID;
                user.UpdatedOn = DateTime.UtcNow;
                await this.usersRepository.UpdateUser(user);
                this.unitOfWork.Commit();

                return new ResponseResult()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = 1
                };
            }
            else
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = 0,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }
        }

        /// <summary>
        /// This api is used for Verify User Mobile Number for existing User
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<ResponseResult> VerifyUserMobileNumber(long accountID, long userId)
        {
            var user = await this.usersRepository.GetUser(accountID, userId);

            if (user != null && user.UserId > 0)
            {
                user.IsMobileNumberVerified = true;
                user.UpdatedBy = accountID;
                user.UpdatedOn = DateTime.UtcNow;
                await this.usersRepository.UpdateUser(user);
                this.unitOfWork.Commit();

                return new ResponseResult()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = 1,
                };
            }
            else
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Data = 0,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }

        }

        /// <summary>
        /// RootUserSetup
        /// </summary>
        /// <param name="rootUserSetupModel"></param>
        /// <returns></returns>
        public async Task<ResponseResult> RootUserSetup(RootUserSetupModel rootUserSetupModel)
        {
            if (rootUserSetupModel != null && rootUserSetupModel.CreatedAccount != null && rootUserSetupModel.PasswordPolicy != null)
            {
                rootUserSetupModel.CreatedAccount.UserName = "root";
                rootUserSetupModel.CreatedAccount.Password = "Root@123";
                var isUserAlreadyExists = await this.usersRepository.GetUserByUserName(rootUserSetupModel.CreatedAccount.UserName, rootUserSetupModel.AccountId);
                if (isUserAlreadyExists == null)
                {
                    #region Default Password Policy
                    rootUserSetupModel.PasswordPolicy.AccountId = rootUserSetupModel.AccountId;
                    await passwordPolicyRepository.CreatePasswordPolicy(rootUserSetupModel.PasswordPolicy);
                    #endregion

                    #region Default User
                    var userResponse = CreateDefaultUser(rootUserSetupModel, rootUserSetupModel.AccountId, rootUserSetupModel.loggedInUserId).Result;
                    #endregion

                    #region Default Group
                    var newGroup = new Groups
                    {
                        Description = "Default Group Created by Admin",
                        AccountId = rootUserSetupModel.AccountId,
                        Name = "root",
                        Status = 1,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = rootUserSetupModel.loggedInUserId // Admin userid
                    };
                    var group = this.groupsRepository.CreateGroup(newGroup);
                    #endregion

                    #region Default Group Rights
                    var newGroupRights = rootUserSetupModel.VersionModules.Select(x => new GroupRights()
                    {
                        Group = newGroup,
                        AccountId = rootUserSetupModel.AccountId,
                        ModuleId = x,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = rootUserSetupModel.loggedInUserId
                    }).ToList();

                    var groupRights = this.groupsRepository.CreateGroupRights(newGroupRights);
                    #endregion

                    #region Group Mapping
                    var newGroupMapping = new UserGroupMappings()
                    {
                        AccountId = rootUserSetupModel.AccountId,
                        Group = newGroup,
                        User = userResponse,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = rootUserSetupModel.loggedInUserId
                    };
                    var groupMapping = this.usersGroupMappingRepository.AddGroupMapping(newGroupMapping);
                    #endregion

                    //await passwordPolicy;
                    await group;
                    await groupRights;
                    await groupMapping;

                    this.unitOfWork.Commit();

                    return new ResponseResult()
                    {
                        Message = ResponseMessage.RecordSaved,
                        ResponseCode = ResponseCode.RecordSaved,
                    };
                }
                else
                {
                    return new ResponseResult()
                    {
                        Message = ResponseMessage.RootUserConfigurationAlreadyExists,
                        ResponseCode = ResponseCode.RootUserConfigurationAlreadyExists,
                    };
                }
             
            }
            else
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.SomethingWentWrong,
                    ResponseCode = ResponseCode.SomethingWentWrong,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.SomethingWentWrong
                    }
                };
            }
        }

        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns></returns>
        public async Task<User> CreateDefaultUser(RootUserSetupModel objRootUserSetupModel, long accountId, long loggedInUserId)
        {
            User objUser = new User();
            AccountInformation objAccountInformation = objRootUserSetupModel.CreatedAccount;
            if (!string.IsNullOrWhiteSpace(objAccountInformation.FirstName) && !string.IsNullOrWhiteSpace(objAccountInformation.UserName) && !string.IsNullOrWhiteSpace(objAccountInformation.Password))
            {
                var cryptography = new PBKDF2Cryptography();
                var passwordError = await ValidatePassword(cryptography, accountId, 0, objAccountInformation.Password,true, objRootUserSetupModel.PasswordPolicy);
                if (string.IsNullOrWhiteSpace(passwordError))
                {
                    var response = new UserVM();
                    var hashAccount = cryptography.CreateHash(objAccountInformation.Password);
                    var userModel = new User()
                    {
                        AccountId = accountId,
                        UserName = objAccountInformation.UserName,
                        EmailAddress = objAccountInformation.EmailAddress,
                        FirstName = objAccountInformation.FirstName,
                        LastName = objAccountInformation.LastName,
                        Mobile = objAccountInformation.Mobile,
                        UserStatus = 1,
                        PasswordHash = hashAccount.Hash,
                        PasswordSalt = hashAccount.Salt,
                        FailedLoginAttempts = 0,
                        IsMobileNumberVerified = true,
                        IsEmailVerified = true,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = loggedInUserId,
                    };

                    objUser = await this.usersRepository.CreateDefaultUser(userModel);
                }
            }
            return objUser;
        }
         
        /// <summary>
        /// SendMobileOTP
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> SendMobileOTP(SendMobileOtpMainVM model, long accountId, long loggedInUserId, string deviceId, string apiName)
        {
            if(accountId < 0 ||  string.IsNullOrWhiteSpace(deviceId) || string.IsNullOrWhiteSpace(apiName) || model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            if(smsConfig == null || smsConfig.SMSProvider == null || smsConfig.OTPConfig == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = "SMS config details are missing.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            
            var errorMessage = new Dictionary<string, string[]>();
            if(string.IsNullOrWhiteSpace(model.CountryCode))
            {
                errorMessage.Add("countryCode", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(model.ContactNumber))
            {
                errorMessage.Add("contactNumber", new string[] { "This field may not be blank." });
            }  
             

            if(errorMessage.Count > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        Detail = errorMessage
                    }
                };
            }

            if (string.IsNullOrWhiteSpace(model.OtpType)) model.OtpType = "SendMobileOTP";

            var otpValue = smsConfig.OTPConfig.DefaultOTP;
            if(string.IsNullOrWhiteSpace(otpValue)) otpValue = Core.CommonHelper.GenerateOTPCode(smsConfig.OTPConfig.OTPLength);

            var otpLog = new OtpLog()
            {
                AccountId = accountId,
                ApiName = apiName,
                ImeiNumber = model.ImeiNumber,
                ContactNumber = model.ContactNumber,
                CountryCode = model.CountryCode,
                DeviceId = deviceId,
                CodeExpiry = DateTime.UtcNow.AddMinutes(smsConfig.OTPConfig.OTPExpiredInMin),
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUserId,
                IsOtpVerified = false,
                Otptype = model.OtpType,
                PassCode = otpValue
            };

            var isOtpSend = false;
            try
            {
                var message = smsConfig.OTPConfig.OTPMessage.Replace("##OTP##", otpValue);
                isOtpSend = smsHelper.SendOTPSMS(smsConfig, model.CountryCode + model.ContactNumber, message);
            }
            catch
            {
                isOtpSend = false;
            }
            otpLog.IsOtpSend = isOtpSend;

            var isSaved = await usersRepository.CreateOtpLog(otpLog);
            if(!isOtpSend || isSaved < 1)
            {
                return new ResponseResult<bool>()
                {
                    Data = false,
                    ResponseCode = ResponseCode.InternalServerError,
                    Message = ResponseMessage.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = (!isOtpSend ? "Could not able to send SMS." : "Could not able to save SMS details.")
                    }
                };
            }
            else
            {
                return new ResponseResult<bool>()
                {
                    Data = true,
                    ResponseCode = ResponseCode.RecordSaved,
                    Message = "OTP has been sent."
                };
            }

        }

        /// <summary>
        /// Verify OTP
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> VerifyOTP(VerifyOtpVM model, long accountId, long loggedInUserId, string deviceId)
        {
            if (accountId < 0 || string.IsNullOrWhiteSpace(deviceId) || model == null 
                || (string.IsNullOrWhiteSpace(model.ContactNumber) && string.IsNullOrWhiteSpace(model.Email)))
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var errorMessage = new Dictionary<string, string[]>();
            
            if (!string.IsNullOrWhiteSpace(model.ContactNumber) && string.IsNullOrWhiteSpace(model.CountryCode))
            {
                errorMessage.Add("countryCode", new string[] { "This field may not be blank." });
            } 

            if (string.IsNullOrWhiteSpace(model.OtpValue))
            {
                errorMessage.Add("otpValue", new string[] { "This field may not be blank." });
            }

            if (errorMessage.Count > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        Detail = errorMessage
                    }
                };
            }

            var optLog = await usersRepository.GetOtpLog(accountId, deviceId, model.CountryCode, model.ContactNumber, model.Email);
            if (optLog == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = "Invalid request",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            if(optLog.PassCode.Trim() != model.OtpValue.Trim())
            {
                return new ResponseResult<bool>()
                {
                    Message = "Invalid OTP",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Please enter valid OTP.",
                    }
                };
            }

            //if (optLog.CodeExpiry > DateTime.UtcNow.AddMinutes(smsConfig.OTPConfig.OTPExpiredInMin))
            //{
            //    return new ResponseResult<bool>()
            //    {
            //        Message = "OTP Expired",
            //        ResponseCode = ResponseCode.ValidationFailed,
            //        Error = new ErrorResponseResult()
            //        {
            //            Message = "OTP has been expired",
            //        }
            //    };
            //}

            optLog.IsOtpVerified = true;
            optLog.UpdatedOn = DateTime.UtcNow;
            optLog.UpdatedBy = loggedInUserId;

            if (await usersRepository.UpdateOtpLog(optLog) > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = "OTP is valid",
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = true
                };
            }
            else
            {
                return new ResponseResult<bool>()
                {
                    Message = "Could not able to save details.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
        }

        public async Task<ResponseResult<SuccessMessageModel>> SendVerificationCode(long accountId,long userId)
        {
            var response = new ResponseResult<SuccessMessageModel>();
            var user = await this.usersRepository.GetUserByUserId(accountId, userId);
           
            if (user !=null && user.IsEmailVerified == true)
            {
                response.Message = ResponseMessage.EmailAlreadyVerified;
                response.ResponseCode = ResponseCode.NoRecordFound;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.EmailAlreadyVerified
                };
                return response;
               
            }
            if (user != null && user.UserId > 0)
            {
                DateTime lastUpdatedDate = user.CreatedOn;
                if (user.UpdatedOn != null)
                {
                    lastUpdatedDate = (DateTime)user.UpdatedOn;
                }
                var token = user.UserId + "~!|" + user.AccountId + "~!|" + lastUpdatedDate.Ticks + "~!|" + DateTime.Now;
                token = HttpUtility.UrlEncode(commonHelper.EncryptString(token, sendVerificationMailConfig.EncryptKey));
                var uid = HttpUtility.UrlEncode(commonHelper.EncryptString(user.UserId.ToString(), sendVerificationMailConfig.EncryptKey));
                var setPasswordURL = sendVerificationMailConfig.URL.Replace("##TOKEN##", token).Replace("##UID##", uid);


                var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
                var filePath = Path.Combine("Helpers", "Docs", "EmailTemplate", "EmailVerification.html");
                var fileInfo = provider.GetFileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    response.Message = ResponseMessage.InternalServerError;
                    response.ResponseCode = ResponseCode.InternalServerError;
                    response.Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    };
                    return response;
                }

                var bodyHTML = "";
                using (var fs = fileInfo.CreateReadStream())
                {
                    using (var sr = new StreamReader(fs))
                    {
                        bodyHTML = sr.ReadToEnd();
                    }
                }

                if (string.IsNullOrWhiteSpace(bodyHTML))
                {
                    response.Message = ResponseMessage.InternalServerError;
                    response.ResponseCode = ResponseCode.InternalServerError;
                    response.Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    };
                    return response;
                }

                bodyHTML = bodyHTML.Replace("##url##", setPasswordURL);

                emailHelper.SendMail(user.EmailAddress, "Email verification", bodyHTML);

                response.Message = "Email verification link has been send to the email.";
                response.ResponseCode = ResponseCode.RecordFetched;
                response.Data = new SuccessMessageModel()
                {
                    Message = "Email verification link has been send to the email."
                };
                return response;

                
            }
            
            else
            {
                response.Message = ResponseMessage.NoRecordFound;
                response.ResponseCode = ResponseCode.NoRecordFound;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.NoRecordFound
                };
                return response;
               
            }
           
        }

        public async Task<User> UpdateStripeCustomerId(long accountId, long userId, string customerId)
        {
            User user =await usersRepository.GetUserByUserId(accountId, userId);
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = userId;
            user.StripeCustomerId = customerId;
            return usersRepository.UpdateStripeId(user);
        }
        public async Task<ResponseResult<string>> ChangePasswordByMobile(ChangePasswordMobile changePassword, long loggedInUserId)
        {
            if (changePassword == null)
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }
            var user = await this.usersRepository.GetUserByUserMobile(changePassword.MobileNo, changePassword.AccountId);

            // If user not found, return error.
            if (user == null)
            {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.Unauthorized,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.Unauthorized
                    }
                };
            }
            
            bool isVerified = await usersRepository.VerifyOTP(changePassword.MobileNo);
            if(!isVerified) {
                return new ResponseResult<string>()
                {
                    Message = ResponseMessage.Unauthorized,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.Unauthorized
                    }
                };
            }
            PBKDF2Cryptography cryptography = new PBKDF2Cryptography();

            // Validate New Password
            var validationErrorMsg = await ValidatePassword(cryptography, user.AccountId, user.UserId, changePassword.Password, false, null);

            if (!string.IsNullOrWhiteSpace(validationErrorMsg))
            {
                return new ResponseResult<string>()
                {
                    Message = validationErrorMsg,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = validationErrorMsg
                    }
                };
            }

            //Change Password
            PBKDF2Password hashAccount = cryptography.CreateHash(changePassword.Password);
            user.PasswordHash = hashAccount.Hash;
            user.PasswordSalt = hashAccount.Salt;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = loggedInUserId;
            user.UserStatus = (int)UserStatus.Active;
            var passwordHistory = new PasswordHistories()
            {
                AccountId = user.AccountId,
                UserId = user.UserId,
                LastUsedOn = DateTime.UtcNow,
                PasswordHash = hashAccount.Hash,
                PasswordSalt = hashAccount.Salt
            };
            await this.usersRepository.ChangePassword(user);
            await this.passwordHistoryRepository.CreatePasswordHistory(passwordHistory);
            unitOfWork.Commit();

            return new ResponseResult<string>()
            {
                Message = "Password have been changed successfully",
                ResponseCode = ResponseCode.RecordSaved,
                Data = "Password have been changed successfully"
            };
        }
        public async Task<ResponseResult<bool>> ForgetPasswordBySMS(SendMobileOtpMainVM model, long accountId, long loggedInUserId, string deviceId, string apiName)
        {
            if (accountId < 0 || string.IsNullOrWhiteSpace(deviceId) || string.IsNullOrWhiteSpace(apiName) || model == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }
            var errorMessage = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(model.CountryCode))
            {
                errorMessage.Add("countryCode", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(model.ContactNumber))
            {
                errorMessage.Add("contactNumber", new string[] { "This field may not be blank." });
            }


            if (errorMessage.Count > 0)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }
            if (smsConfig == null || smsConfig.SMSProvider == null || smsConfig.OTPConfig == null)
            {
                return new ResponseResult<bool>()
                {
                    Message = "SMS configuration is missing.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            User user = await usersRepository.GetUserByUserMobile(model.ContactNumber, accountId);
            if (user == null)
            {
                return new ResponseResult<bool>()
                {
                    Data = false,
                    ResponseCode = ResponseCode.Unauthorized,
                    Message = ResponseMessage.Unauthorized// "If you are a valid user, you will receive OTP on you mobile number."
                };
            }
            

            if (string.IsNullOrWhiteSpace(model.OtpType)) model.OtpType = "ForgetPassword";

            var otpValue = smsConfig.OTPConfig.DefaultOTP;
            if (string.IsNullOrWhiteSpace(otpValue)) otpValue = Core.CommonHelper.GenerateOTPCode(smsConfig.OTPConfig.OTPLength);

            var otpLog = new OtpLog()
            {
                AccountId = accountId,
                ApiName = apiName,
                ImeiNumber = model.ImeiNumber,
                ContactNumber = model.ContactNumber,
                CountryCode = model.CountryCode,
                DeviceId = deviceId,
                CodeExpiry = DateTime.UtcNow.AddMinutes(smsConfig.OTPConfig.OTPExpiredInMin),
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUserId,
                IsOtpVerified = false,
                Otptype = model.OtpType,
                PassCode = otpValue
            };

            var isOtpSend = false;
            try
            {
                var message = smsConfig.OTPConfig.OTPMessage.Replace("##OTP##", otpValue);
                isOtpSend = smsHelper.SendOTPSMS(smsConfig, model.CountryCode + model.ContactNumber, message);
            }
            catch
            {
                isOtpSend = false;
            }
            otpLog.IsOtpSend = isOtpSend;

            var isSaved = await usersRepository.CreateOtpLog(otpLog);
            if (!isOtpSend || isSaved < 1)
            {
                return new ResponseResult<bool>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
            else
            {
                return new ResponseResult<bool>()
                {
                    Data = true,
                    ResponseCode = ResponseCode.RecordSaved,
                    Message = ResponseMessage.RecordSaved// "If you are a valid user, you will receive OTP on you mobile number."
                };
            }

        }

        /// <summary>
        /// Verify OTP
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accountId"></param>
        /// <param name="loggedInUserId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async  Task<ResponseResult> VerifyAndAuthenticate(VerifyOtpVM model, long accountId, long loggedInUserId, string deviceId)
        {
            if (accountId < 0 || string.IsNullOrWhiteSpace(deviceId) || model == null
                || (string.IsNullOrWhiteSpace(model.ContactNumber) && string.IsNullOrWhiteSpace(model.Email)))
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var errorMessage = new Dictionary<string, string[]>();

            if (!string.IsNullOrWhiteSpace(model.ContactNumber) && string.IsNullOrWhiteSpace(model.CountryCode))
            {
                errorMessage.Add("countryCode", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(model.OtpValue))
            {
                errorMessage.Add("otpValue", new string[] { "This field may not be blank." });
            }

            if (errorMessage.Count > 0)
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        Detail = errorMessage
                    }
                };
            }

            var optLog = await usersRepository.GetOtpLog(accountId, deviceId, model.CountryCode, model.ContactNumber, model.Email);
            if (optLog == null)
            {
                return new ResponseResult()
                {
                    Message = "Invalid request",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            if (optLog.PassCode.Trim() != model.OtpValue.Trim())
            {
                return new ResponseResult()
                {
                    Message = "Invalid OTP",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Please enter valid OTP.",
                    }
                };
            }

           

            optLog.IsOtpVerified = true;
            optLog.UpdatedOn = DateTime.UtcNow;
            optLog.UpdatedBy = loggedInUserId;

            if (await usersRepository.UpdateOtpLog(optLog) > 0)
            {
                Users users =await usersRepository.GetUserByUserMobile(model.ContactNumber, accountId);
                if (users == null)
                {
                    return new ResponseResult()
                    {
                        Message = "OTP is valid",
                        ResponseCode = ResponseCode.RecordFetched,
                        Data = null
                    };
                }
                var userData = await authenticationService.AuthenticateWithMobile(users, accountId);
                return new ResponseResult()
                {
                    Message = "OTP is valid",
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = userData
                };
            }
            else
            {
                return new ResponseResult()
                {
                    Message = "Could not able to save details.",
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
        }

        
       
    }
}