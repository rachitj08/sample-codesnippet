using Common.Enum;
using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Sample.Admin.Model.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Utilities.EmailHelper;
using Utilities.PBKDF2Hashing;
using Utility;
using DATAMODEL = Sample.Admin.Service.Infrastructure.DataModels;

namespace Sample.Admin.Service.ServiceWorker
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository adminUserRepository;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;

        private readonly ICommonHelper commonHelper;
        private readonly IEmailHelper emailHelper;

        private readonly ResetPasswordConfig resetPasswordConfig;


        /// <summary>
        /// Currency Service constructor to Inject dependency
        /// </summary>
        /// <param name="currencyRepository">currency repository</param>
        public AdminUserService(IAdminUserRepository adminUserRepository, IConfiguration configuration, IUnitOfWork unitOfWork,
            ICommonHelper commonHelper, IEmailHelper emailHelper, IOptions<ResetPasswordConfig> resetPasswordConfig)
        {
            this.adminUserRepository = adminUserRepository;
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.commonHelper = commonHelper;
            this.emailHelper = emailHelper;
            this.resetPasswordConfig = resetPasswordConfig.Value;
        }

        /// <summary>
        /// 
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
            return await this.adminUserRepository.GetAllAdminUsers(ordering, search, pageSize, pageNumber, offset, all);
        }

        public async Task<AdminUsersModel> GetAdminUserDetail(int userId)
        {
            return await this.adminUserRepository.GetAdminUserDetail(userId);
        }


        /// <summary>
        /// To Create new Admin User
        /// </summary>
        /// <param name="module">Admin User object</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserCreationModel>> CreateAdminUser(UserCreationModel adminUser, int loggedInUserId)
        {
            // Check User Name 
            var existingUser = await this.adminUserRepository.GetUserByUserName(adminUser.UserName);
            if (existingUser != null && existingUser.UserId > 0)
            {
                var errorMsg = new Dictionary<string, string[]>();
                errorMsg.Add("userName", new[] { "User Name has been used already." });

                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorMsg
                    }
                };
            }
            //added by Raj for check email and phone no
            var existingEmail = await this.adminUserRepository.GetAdminUserByEmailId(adminUser.EmailAddress);
            if (existingEmail != null && existingEmail.UserId > 0)
            {
                var errorMsg = new Dictionary<string, string[]>();
                errorMsg.Add("emailAddress", new[] { "Email Address has been used already." });

                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorMsg
                    }
                };
            }
            var existingMobile = await this.adminUserRepository.GetAdminUserByMobile(adminUser.Mobile);
            if (existingMobile != null && existingMobile.UserId > 0)
            {
                var errorMsg = new Dictionary<string, string[]>();
                errorMsg.Add("mobile", new[] { "mobile has been used already." });

                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorMsg
                    }
                };
            }
            // Create Model
            var newAdminUsers = new DATAMODEL.AdminUsers();
            newAdminUsers.UserName = adminUser.UserName;
            var cryptography = new PBKDF2Cryptography();
            var hashAccount = cryptography.CreateHash(adminUser.Password);
            
            //newAdminUsers.UserId = adminUser.UserId;
            newAdminUsers.PasswordHash = hashAccount.Hash;
            newAdminUsers.PasswordSalt = hashAccount.Salt;
            newAdminUsers.FirstName = adminUser.FirstName;
            newAdminUsers.LastName = adminUser.LastName;
            newAdminUsers.EmailAddress = adminUser.EmailAddress;
            newAdminUsers.Mobile = adminUser.Mobile;
            newAdminUsers.IsEmailVerified = true;
            newAdminUsers.IsMobileNumberVerified = true;
            newAdminUsers.UserStatus = 1;
            newAdminUsers.MfatypeId = adminUser.MfatypeId;
            newAdminUsers.UserStatus = adminUser.UserStatus;
            newAdminUsers.AuthenticationCategory = adminUser.AuthenticationCategory;
            newAdminUsers.ExternalUserId = adminUser.ExternalUserId;
            newAdminUsers.IsEmailVerified = adminUser.IsEmailVerified;
            newAdminUsers.IsMobileNumberVerified = adminUser.IsMobileNumberVerified;
            newAdminUsers.CreatedOn = DateTime.UtcNow;
            newAdminUsers.CreatedBy = loggedInUserId;
            var result= await this.adminUserRepository.CreateAdminUser(newAdminUsers);


            if (result.UserId > 0)
            {
                adminUser.UserId = result.UserId;

                if (adminUser.Groups != null && adminUser.Groups.Count() > 0)
                {
                    var groupList = adminUser.Groups.Select(x => new UserGroupMappings()
                    {
                        UserId = result.UserId,
                        GroupId = x,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = loggedInUserId
                    }).ToList();

                    await this.adminUserRepository.CreateUserGroupMappings(groupList);
                }

                if (adminUser.UserRights != null && adminUser.UserRights.Count() > 0)
                {
                    var userRights = adminUser.UserRights.Select(x => new UserRights()
                    {
                        UserId = result.UserId,
                        ModuleId = x.ModuleId,
                        IsPermission = x.IsPermission,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = loggedInUserId,                        
                    }).ToList();

                    await this.adminUserRepository.CreateUserRights(userRights);
                }

                this.unitOfWork.Commit();

                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = adminUser
                };
            }
            else
            {
                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }
        }

        /// To Update Admin User
        /// </summary>
        /// /// <param name="userId">user Id</param>
        /// <param name="adminUser">New admin user object</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserCreationModel>> UpdateAdminUser(int userId, UserCreationModel adminUser, int loggedInUserId)
        {
            var existModel = await this.adminUserRepository.GetAdminUserByUserId(userId);
            if (existModel != null)
            {
                // Check User Name 
                var existingUser = await this.adminUserRepository.GetUserByUserName(adminUser.UserName);
                if (existingUser != null && existingUser.UserId != existModel.UserId)
                {
                    var errorMsg = new Dictionary<string, string[]>();
                    errorMsg.Add("userName", new[] { "User Name has been used already." });

                    return new ResponseResult<UserCreationModel>()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        ResponseCode = ResponseCode.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Detail = errorMsg
                        }
                    };
                }
                //added by Raj for check email and phone no
                var existingEmail = await this.adminUserRepository.GetAdminUserByEmailId(adminUser.EmailAddress);
                if (existingEmail != null && existingEmail.UserId != existModel.UserId)
                {
                    var errorMsg = new Dictionary<string, string[]>();
                    errorMsg.Add("emailAddress", new[] { "email address has been used already." });

                    return new ResponseResult<UserCreationModel>()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        ResponseCode = ResponseCode.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Detail = errorMsg
                        }
                    };
                }
                var existingMobile = await this.adminUserRepository.GetAdminUserByMobile(adminUser.Mobile);
                if (existingMobile != null && existingMobile.UserId !=existModel.UserId)
                {
                    var errorMsg = new Dictionary<string, string[]>();
                    errorMsg.Add("mobile", new[] { "mobile has been used already." });

                    return new ResponseResult<UserCreationModel>()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        ResponseCode = ResponseCode.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Detail = errorMsg
                        }
                    };
                }
                var newAdminUsers = new DATAMODEL.AdminUsers();
                newAdminUsers.UserName = adminUser.UserName;
                //PBKDF2Cryptography cryptography = new PBKDF2Cryptography();
                //PBKDF2Password hashAccount = cryptography.CreateHash(adminUser.Password);
                //existModel.PasswordHash = hashAccount.Hash;
                //existModel.PasswordSalt = hashAccount.Salt;
                existModel.FirstName = adminUser.FirstName;
                existModel.LastName = adminUser.LastName;
                existModel.EmailAddress = adminUser.EmailAddress;
                existModel.Mobile = adminUser.Mobile;
                existModel.IsEmailVerified = false;
                existModel.IsMobileNumberVerified = false;
                existModel.MfatypeId = adminUser.MfatypeId;
                existModel.UserStatus = adminUser.UserStatus;
                existModel.AuthenticationCategory = adminUser.AuthenticationCategory;
                existModel.ExternalUserId = adminUser.ExternalUserId;
                existModel.IsEmailVerified = adminUser.IsEmailVerified;
                existModel.IsMobileNumberVerified = adminUser.IsMobileNumberVerified;
                //existModel.UserStatus = 1;
                existModel.UpdatedOn = DateTime.UtcNow;
                existModel.UpdatedBy = loggedInUserId;

                await this.adminUserRepository.UpdateAdminUser(existModel);

                // Update Groups
                var groupList = new List<UserGroupMappings>();
                if (adminUser.Groups != null && adminUser.Groups.Count() > 0)
                {
                    groupList = adminUser.Groups.Select(x => new UserGroupMappings()
                    {
                        UserId = userId,
                        GroupId = x,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = loggedInUserId,
                        UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = loggedInUserId,
                    }).ToList();

                }
                await this.adminUserRepository.CreateUserGroupMappings(groupList, userId);

                // Update Rights
                var userRights = new List<UserRights>();
                if (adminUser.UserRights != null && adminUser.UserRights.Count() > 0)
                {
                    userRights = adminUser.UserRights.Select(x => new UserRights()
                    {
                        UserId = userId,
                        ModuleId = x.ModuleId,
                        IsPermission = x.IsPermission,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = loggedInUserId,
                        UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = loggedInUserId,
                    }).ToList();

                }
                await this.adminUserRepository.CreateUserRights(userRights, userId);

                this.unitOfWork.Commit();
                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = adminUser
                };
            }
            else
            {
                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }
        }

        // <summary>
        /// To Update Admin User Partially
        /// </summary>
        /// /// <param name="userId">user Id</param>
        /// <param name="adminUser">New admin user object</param>
        /// <returns></returns>
        public async Task<ResponseResult<UserCreationModel>> UpdatePartialAdminUser(int userId, UserCreationModel adminUser, int loggedInUserId)
        {
            var existModel = await this.adminUserRepository.GetAdminUserByUserId(userId);
            if (existModel != null)
            {
                // Check User Name 
                var existingUser = await this.adminUserRepository.GetUserByUserName(adminUser.UserName);
                if (existingUser != null && existingUser.UserId != existModel.UserId)
                {
                    var errorMsg = new Dictionary<string, string[]>();
                    errorMsg.Add("userName", new[] { "User Name has been used already." });

                    return new ResponseResult<UserCreationModel>()
                    {
                        Message = ResponseMessage.ValidationFailed,
                        ResponseCode = ResponseCode.ValidationFailed,
                        Error = new ErrorResponseResult()
                        {
                            Detail = errorMsg
                        }
                    };
                }

                var newAdminUsers = new DATAMODEL.AdminUsers();
                newAdminUsers.UserName = adminUser.UserName;
                //var cryptography = new PBKDF2Cryptography();
                //var hashAccount = cryptography.CreateHash(adminUser.Password);
                //existModel.PasswordHash = hashAccount.Hash;
                //existModel.PasswordSalt = hashAccount.Salt;
                existModel.FirstName = adminUser.FirstName;
                existModel.LastName = adminUser.LastName;
                existModel.EmailAddress = adminUser.EmailAddress;
                existModel.Mobile = adminUser.Mobile;
                existModel.IsEmailVerified = false;
                existModel.IsMobileNumberVerified = false;
                existModel.UserStatus = adminUser.UserStatus;
                existModel.MfatypeId = adminUser.MfatypeId;
                existModel.AuthenticationCategory = adminUser.AuthenticationCategory;
                existModel.ExternalUserId = adminUser.ExternalUserId;
                existModel.IsEmailVerified = adminUser.IsEmailVerified;
                existModel.IsMobileNumberVerified = adminUser.IsMobileNumberVerified;
                existModel.UpdatedOn = DateTime.UtcNow;
                existModel.UpdatedBy = loggedInUserId;
                //existModel.UserStatus = 1;

                await this.adminUserRepository.UpdateAdminUser(existModel);

                // Update Groups
                var groupList = new List<UserGroupMappings>();
                if (adminUser.Groups != null && adminUser.Groups.Count() > 0)
                {
                    groupList = adminUser.Groups.Select(x => new UserGroupMappings()
                    {
                        UserId = userId,
                        GroupId = x,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = loggedInUserId,
                        UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = loggedInUserId,
                    }).ToList();

                }
                await this.adminUserRepository.CreateUserGroupMappings(groupList, userId);

                // Update Rights
                var userRights = new List<UserRights>();
                if (adminUser.UserRights != null && adminUser.UserRights.Count() > 0)
                {
                    userRights = adminUser.UserRights.Select(x => new UserRights()
                    {
                        UserId = userId,
                        ModuleId = x.ModuleId,
                        IsPermission = x.IsPermission,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = loggedInUserId,
                        UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = loggedInUserId,
                    }).ToList();

                }
                await this.adminUserRepository.CreateUserRights(userRights, userId);

                this.unitOfWork.Commit();
                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = adminUser
                };
            }
            else
            {
                return new ResponseResult<UserCreationModel>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    }
                };
            }
        }

        /// <summary>
        /// To delete existing Admin User
        /// </summary>
        /// <param name="userId">admin user identifier</param>
        /// <returns></returns>
        public async Task<int> DeleteAdminUser(int userId)
        { 
            return await this.adminUserRepository.DeleteAdminUser(userId);
        }

        /// <summary>
        /// Change password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return response result</returns>
        public async Task<ResponseResult<SuccessMessageModel>> ChangePassword(ChangePasswordModel model, int loggedInUserId)
        {
            var user = await this.adminUserRepository.GetAdminUserByUserId(model.UserId);

            // If user not found, return error.
            if (user == null)
            {
                return new ResponseResult<SuccessMessageModel>()
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
                return new ResponseResult<SuccessMessageModel>()
                {
                    Message = ResponseMessage.Unauthorized,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            // Validate Password
            var cryptography = new PBKDF2Cryptography();
            bool password = cryptography.ValidatePassword(model.OldPassword, user.PasswordHash, user.PasswordSalt);

            // If password didn't match, return error.
            if (!password)
            {
                return new ResponseResult<SuccessMessageModel>()
                {
                    Message = ResponseMessage.Unauthorized,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.Unauthorized
                    }
                };
            }

            //Change Password
            var hashAccount = cryptography.CreateHash(model.Password);
            user.PasswordHash = hashAccount.Hash;
            user.PasswordSalt = hashAccount.Salt;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = loggedInUserId;
            await this.adminUserRepository.ChangePassword(user);

            return new ResponseResult<SuccessMessageModel>()
            {
                Message = "Password have been changed successfully",
                ResponseCode = ResponseCode.RecordSaved,
                Data = new SuccessMessageModel() {
                    Message = "Password have been changed successfully"
                }
            };
        }

        /// <summary>
        ///  Set password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> SetPassword(SetPasswordModel model, int loggedInUserId)
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
                || !int.TryParse(uid, out var userId))
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
            if (tokenValues.Length < 3 || !Int64.TryParse(tokenValues[0], out var tokenUserId)
                || tokenUserId != userId || !Int64.TryParse(tokenValues[1], out var lastUpdatedOn)
                || !DateTime.TryParse(tokenValues[2], out var tokenCreatedOn))
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
            var user = await adminUserRepository.GetUserByUserId(userId);

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

            //Change Password
            var hashAccount = cryptography.CreateHash(model.Password);
            user.PasswordHash = hashAccount.Hash;
            user.PasswordSalt = hashAccount.Salt;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = loggedInUserId;
            await this.adminUserRepository.ChangePassword(user);
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
        ///  Reset password of Admin User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> ForgotPassword(ForgotPasswordRequestModel model)
        {
            var response = new ResponseResult<SuccessMessageModel>();
            if (model == null || string.IsNullOrWhiteSpace(model.Email))
            {
                response.Message = ResponseMessage.ValidationFailed;
                response.ResponseCode = ResponseCode.ValidationFailed;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.ValidationFailed
                };
                return response;
            }

            // Get User By Email Id
            var user = await adminUserRepository.GetAdminUserByEmailId(model.Email);
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

            var token = user.UserId + "~!|" + lastUpdatedDate.Ticks + "~!|" + DateTime.Now;
            token = HttpUtility.UrlEncode(commonHelper.EncryptString(token, resetPasswordConfig.EncryptKey));
            var uid = HttpUtility.UrlEncode(commonHelper.EncryptString(user.UserId.ToString(), resetPasswordConfig.EncryptKey));
            var setPasswordURL = resetPasswordConfig.URL.Replace("##TOKEN##", token).Replace("##UID##", uid);
            var bodyHTML = "";

            var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            var filePath = Path.Combine("Helpers", "Docs", "EmailTemplate", "ForgotPasswordFormat.html");
            var fileInfo = provider.GetFileInfo(filePath);
            if (!fileInfo.Exists)
            {
                response.Message = "Email template file does not exists.";
                response.ResponseCode = ResponseCode.InternalServerError;
                response.Error = new ErrorResponseResult()
                { 
                    Message = ResponseMessage.InternalServerError
                };
                return response;
            }

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

            bool isEmailSended = emailHelper.SendMail(model.Email, "Password Reset", bodyHTML);

            if(isEmailSended)
            {
                response.Message = "Password reset link has been send to the email.";
                response.ResponseCode = ResponseCode.RecordFetched;
                response.Data = new SuccessMessageModel()
                {
                    Message = "Password reset link has been send to the email."
                };
            }
            else
            {
                response.Message = "Email not sent something went wrong, try again";
                response.ResponseCode = ResponseCode.HttpResponseNull;
                response.Data = new SuccessMessageModel()
                {
                    Message = "Email not sent something went wrong, try again."
                };
            }
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

            RefreshTokenModel refreshToken = null;

            if (await this.adminUserRepository.VerifyToken(tokenModel))
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
    }
}
