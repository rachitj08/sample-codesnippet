using Common.Enum;
using Common.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Customer.Service.Infrastructure.Repository;
using Sample.Customer.Service.Infrastructure.DataModels;
using Sample.Customer.Model;
using Utilities.PBKDF2Hashing;
using Utility;
using User = Sample.Customer.Model.User;
using UserRight = Sample.Customer.Model.UserRight;

namespace Sample.Customer.Service.ServiceWorker
{
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// commonHelper Private Member
        /// </summary>
        private readonly ICommonHelper commonHelper;
       
        /// <summary>
        /// usersRepository Private Member
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// userGroupMappingRepository Private Member
        /// </summary>
        private readonly IUserGroupMappingRepository userGroupMappingRepository;

        /// <summary>
        /// groupRepository Private Member
        /// </summary>
        private readonly IGroupRepository groupRepository;

        /// <summary>
        /// refreshTokenRepository Private Member
        /// </summary>
        private readonly IRefreshTokenRepository refreshTokenRepository;

        /// <summary>
        /// loginHistoryRepository Private Member
        /// </summary>
        private readonly ILoginHistoryRepository loginHistoryRepository;
        /// <summary>
        /// passwordPolicyRepository Private Member
        /// </summary>
        private readonly IPasswordPolicyRepository passwordPolicyRepository;
        /// <summary>
        /// Unit of work Private Member
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Token service to generate user token.
        /// </summary>
        private readonly ITokenService tokenService;

        /// <summary>
        ///  Load Authentication configuration
        /// </summary>
        private readonly AuthenticationConfig authenticationConfig;

        /// <summary>
        /// Login Service constructor to Inject dependency
        /// </summary>
        /// <param name="unitOfWork">unit of work repository</param>
        /// <param name="usersRepository"> user repository</param>
        public AuthenticationService(IUnitOfWork unitOfWork, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository,
            ILoginHistoryRepository loginHistoryRepository, IPasswordPolicyRepository passwordPolicyRepository,
            ITokenService tokenService, IOptions<AuthenticationConfig> authenticationConfig, ICommonHelper commonHelper,
            IUserGroupMappingRepository userGroupMappingRepository, IGroupRepository groupRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.loginHistoryRepository = loginHistoryRepository;
            this.passwordPolicyRepository = passwordPolicyRepository;
            this.tokenService = tokenService;
            this.authenticationConfig = authenticationConfig.Value;
            this.commonHelper = commonHelper;
            this.userGroupMappingRepository = userGroupMappingRepository;
            this.groupRepository = groupRepository;
        }


        /// <summary>
        /// To Authenticate user
        /// </summary>
        /// <param name="login"> login data for user</param>
        /// <param name="ipAddress">IP Address</param>
        /// <param name="requestHeaders">Request Headers</param>
        /// <returns></returns>
        public async Task<ResponseResult> Authenticate(Login login, string ipAddress, string requestHeaders)
        {
            if (login == null || login.AccountId < 1)
            {
                return new ResponseResult()
                {
                    Message = "Invalid request",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Invalid request",
                    }

                };
            }

            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(login.UserName))
            {
                errorDetails.Add("userName", new string[] { "This field may not be blank." });
            }

            if (string.IsNullOrWhiteSpace(login.Password))
            {
                errorDetails.Add("password", new string[] { "This field may not be blank." });
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

            var loginHistory = new LoginHistory()
            {
                Ipaddress = ipAddress,
                RequestHeader = requestHeaders,
                Token = string.Empty
            };
           
            var user = await this.userRepository.GetUserWithRightsByUserName(login.AccountId, login.UserName);

            // If user not found, add login history and return.
            if (user == null)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "Email address is not valid",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Email address is not valid",
                    }
                };
            }

            bool application = await this.userRepository.GetApplicationByUser(user.UserId, login.AccountId, login.ApplicationId);
            if (!application)
            {
               
                return new ResponseResult()
                {
                    Message = "Unautherize access.",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.Unauthorized,
                    }
                };
            }
            // If user found but user status not activated, add login history and return.
            if (user.UserStatus != (int)UserStatus.Active)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "User status is not Active",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User status is not Active",
                    }
                };
            }

            if (!string.IsNullOrWhiteSpace(user.ExternalUserId) && !string.IsNullOrWhiteSpace(user.AuthenticationCategory))
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "This email address is registered from " + (user.AuthenticationCategory == "F" ? "Facebook account" : "Google account"),
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "This email address is registered from " + (user.AuthenticationCategory == "F" ? "Facebook account" : "Google account")
                    }
                };
            }

            loginHistory.UserId = user.UserId;
            var passwordPolicy = await passwordPolicyRepository.GetPasswordPolicyByAccountId(user.AccountId);
            var loginAttemptsApplicable = passwordPolicy?.NoOfFailedAttemptsAllowed > 0;

            // If user password Expired
            if (user.PasswordExpirationDate != null && DateTime.UtcNow.Date > user.PasswordExpirationDate)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                if (passwordPolicy.PasswordExpirationRequiresAdminReset)
                {
                    return new ResponseResult()
                    {
                        Message = "User password expired",
                        ResponseCode = ResponseCode.Unauthorized,
                        Error = new ErrorResponseResult()
                        {
                            Message = "User password expired",
                        }
                    };
                }
                return new ResponseResult()
                {
                    Message = "User password expired, Please reset your password",
                    ResponseCode = ResponseCode.PasswordExpired,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User password expired, Please reset your password",
                    }
                };
            }

            // If mobile verification required but user mobile not verified, add login history and return.
            if (passwordPolicy.IsMobileVerificationRequired && !user.IsMobileNumberVerified)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "User mobile not verified",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User mobile not verified",
                    }
                };
            }

            // If Email verification required but user email not verified, add login history and return.
            if (passwordPolicy.IsEmailVerificationRequired && !user.IsEmailVerified)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "User email not verified",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User email not verified",
                    }
                };
            }

            // Validate Password
            var cryptography = new PBKDF2Cryptography();
            bool password = cryptography.ValidatePassword(login.Password, user.PasswordHash, user.PasswordSalt);

            // If password didn't match, add login history and return.
            if (!password)
            {
                if (loginAttemptsApplicable)
                {
                    user.FailedLoginAttempts++;
                    //To de-activate user here.
                    if (user.FailedLoginAttempts > passwordPolicy.NoOfFailedAttemptsAllowed)
                        user.UserStatus = (int)UserStatus.FailedAttemptsExceeded;
                    await userRepository.UpdateUser(user);
                }
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                if (loginAttemptsApplicable && user.FailedLoginAttempts > passwordPolicy.NoOfFailedAttemptsAllowed)
                {
                    return new ResponseResult()
                    {
                        Message = "Exceeded maximum number of login attempts",
                        ResponseCode = ResponseCode.Unauthorized,
                        Error = new ErrorResponseResult()
                        {
                            Message = "Exceeded maximum number of login attempts",
                        }
                    };
                }
                return new ResponseResult()
                {
                    Message = ResponseMessage.Unauthorized,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.Unauthorized,
                    }
                };
            }

            var tokenResponse = tokenService.GenerateAuthToken(user);
            
            // If token not generated, add login history and return.
            if (tokenResponse == null || !tokenResponse.Success || string.IsNullOrWhiteSpace(tokenResponse.Token) 
                || tokenResponse.RefreshToken == null || string.IsNullOrWhiteSpace(tokenResponse.RefreshToken.Token))
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();

                return new ResponseResult()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            loginHistory.Token = tokenResponse.Token;
            await AddRefreshToken(tokenResponse.RefreshToken);

            // Update user, add login history and return successful login.
            if (loginAttemptsApplicable)
            {
                user.FailedLoginAttempts = 0;
                await userRepository.UpdateUser(user);
            }

            loginHistory.IsActive = loginHistory.LoginStatus = true;
            await AddLoginHistory(loginHistory);
            unitOfWork.Commit();

            var userRights = new List<UserRight>();
            if (user.UserRights != null && user.UserRights.Count > 0)
            {
                userRights = user.UserRights.Select(x => new UserRight()
                {
                    UserRightId = x.UserRightId,
                    AccountId = x.AccountId,
                    ModuleId = x.ModuleId,
                    IsPermission = x.IsPermission,
                }).ToList();
            }


            var userGroups = new List<UserGroupDetail>();
            if (user.UserGroupMappings != null && user.UserGroupMappings.Count > 0)
            {
                userGroups = user.UserGroupMappings.Select(x => new UserGroupDetail()
                {
                    UserGroupMappingId = x.UserGroupMappingId,
                    AccountId = x.AccountId,
                    GroupId = x.GroupId,
                    GroupName = x.Group?.Name
                }).ToList();


                var userGroupModules = await this.userRepository.GetUserGroupRights(user.UserId, login.AccountId);
                if (userGroupModules.Count > 0)
                {
                    var userGroupRights = userGroupModules.Where(x => !userRights.Select(y => y.ModuleId).Contains(x))
                        .Select(x => new UserRight()
                        {
                            AccountId = login.AccountId,
                            ModuleId = x,
                            IsPermission = true
                        });

                    if (userGroupRights.Count() > 0)
                    {
                        userRights.AddRange(userGroupRights);
                    }
                }
            }
             
            var domainUser = new User()
            {
                AccountId = user.AccountId,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                UserId = user.UserId,
                UserName = user.UserName,
                Token = tokenResponse.Token,
                Refresh = tokenResponse.RefreshToken.Token,
                UserStatus = user.UserStatus,
                UserRights = userRights,
                UserGroups = userGroups,
                ParkingProvidersLocationId = user.ParkingProvidersLocationId
            };
            return new ResponseResult()
            {
                Data = domainUser,
                Message = ResponseMessage.Authenticated,
                ResponseCode = ResponseCode.ValidLogin
            };
        }


        /// <summary>
        /// To Authenticate External User
        /// </summary>
        /// <param name="login"> login data for user</param>
        /// <param name="ipAddress">IP Address</param>
        /// <param name="requestHeaders">Request Headers</param>
        /// <returns></returns>
        public async Task<ResponseResult> AuthenticateExternalUser(ExternalUserVM login, string ipAddress, string requestHeaders)
        {
            if (login == null || login.AccountId < 1 || string.IsNullOrWhiteSpace(login.ExternalUserId))
            {
                return new ResponseResult()
                {
                    Message = "Invalid request",
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = "Invalid request",
                    }

                };
            }

            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(login.Email))
            {
                errorDetails.Add("email", new string[] { "This field may not be blank." });
            }

            //if (string.IsNullOrWhiteSpace(login.MobileNo))
            //{
            //    errorDetails.Add("mobileNo", new string[] { "This field may not be blank." });
            //} 

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

            var loginHistory = new LoginHistory()
            {
                Ipaddress = ipAddress,
                RequestHeader = requestHeaders,
                Token = string.Empty
            };
            var user = await this.userRepository.GetUserWithRightsByUserName(login.AccountId, login.UserName);

            // If user not found, add login history and return.
            if (user == null)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "There is no user in database. Please create profile",
                    ResponseCode = ResponseCode.NoUser,
                    Error = new ErrorResponseResult()
                    {
                        Message = "There is no user in database. Please create profile",
                    }
                };
            }

            // If user found but user status not activated, add login history and return.
            if (user.UserStatus != (int)UserStatus.Active)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "User status is not Active",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User status is not Active",
                    }
                };
            }

            loginHistory.UserId = user.UserId;
            var passwordPolicy = await passwordPolicyRepository.GetPasswordPolicyByAccountId(user.AccountId);
            var loginAttemptsApplicable = passwordPolicy?.NoOfFailedAttemptsAllowed > 0;

            // If user password Expired
            if (user.PasswordExpirationDate != null && DateTime.UtcNow.Date > user.PasswordExpirationDate)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                if (passwordPolicy.PasswordExpirationRequiresAdminReset)
                {
                    return new ResponseResult()
                    {
                        Message = "User password expired",
                        ResponseCode = ResponseCode.Unauthorized,
                        Error = new ErrorResponseResult()
                        {
                            Message = "User password expired",
                        }
                    };
                }
                return new ResponseResult()
                {
                    Message = "User password expired, Please reset your password",
                    ResponseCode = ResponseCode.PasswordExpired,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User password expired, Please reset your password",
                    }
                };
            }

            // If mobile verification required but user mobile not verified, add login history and return.
            if (passwordPolicy.IsMobileVerificationRequired && !user.IsMobileNumberVerified)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "User mobile not verified",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User mobile not verified",
                    }
                };
            }

            // If Email verification required but user email not verified, add login history and return.
            if (passwordPolicy.IsEmailVerificationRequired && !user.IsEmailVerified)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult()
                {
                    Message = "User email not verified",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User email not verified",
                    }
                };
            }

            var tokenResponse = tokenService.GenerateAuthToken(user);

            // If token not generated, add login history and return.
            if (tokenResponse == null || !tokenResponse.Success || string.IsNullOrWhiteSpace(tokenResponse.Token)
                || tokenResponse.RefreshToken == null || string.IsNullOrWhiteSpace(tokenResponse.RefreshToken.Token))
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();

                return new ResponseResult()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            loginHistory.Token = tokenResponse.Token;
            await AddRefreshToken(tokenResponse.RefreshToken);

            // Update user, add login history and return successful login.
            if (loginAttemptsApplicable)
            {
                user.FailedLoginAttempts = 0;
                await userRepository.UpdateUser(user);
            }

            loginHistory.IsActive = loginHistory.LoginStatus = true;
            await AddLoginHistory(loginHistory);
            unitOfWork.Commit();

            var userRights = new List<UserRight>();
            if (user.UserRights != null && user.UserRights.Count > 0)
            {
                userRights = user.UserRights.Select(x => new UserRight()
                {
                    UserRightId = x.UserRightId,
                    AccountId = x.AccountId,
                    ModuleId = x.ModuleId,
                    IsPermission = x.IsPermission,
                }).ToList();
            }


            var userGroups = new List<UserGroupDetail>();
            if (user.UserGroupMappings != null && user.UserGroupMappings.Count > 0)
            {
                userGroups = user.UserGroupMappings.Select(x => new UserGroupDetail()
                {
                    UserGroupMappingId = x.UserGroupMappingId,
                    AccountId = x.AccountId,
                    GroupId = x.GroupId,
                    GroupName = x.Group?.Name
                }).ToList();


                var userGroupModules = await this.userRepository.GetUserGroupRights(user.UserId, login.AccountId);
                if (userGroupModules.Count > 0)
                {
                    var userGroupRights = userGroupModules.Where(x => !userRights.Select(y => y.ModuleId).Contains(x))
                        .Select(x => new UserRight()
                        {
                            AccountId = login.AccountId,
                            ModuleId = x,
                            IsPermission = true
                        });

                    if (userGroupRights.Count() > 0)
                    {
                        userRights.AddRange(userGroupRights);
                    }
                }
            }

            var domainUser = new User()
            {
                AccountId = user.AccountId,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                UserId = user.UserId,
                UserName = user.UserName,
                Token = tokenResponse.Token,
                Refresh = tokenResponse.RefreshToken.Token,
                UserStatus = user.UserStatus,
                UserRights = userRights,
                UserGroups = userGroups,
                ParkingProvidersLocationId = user.ParkingProvidersLocationId
            };
            return new ResponseResult()
            {
                Data = domainUser,
                Message = ResponseMessage.Authenticated,
                ResponseCode = ResponseCode.ValidLogin
            };
        }


        /// <summary>
        /// Add login history (failed or successful).
        /// </summary>
        /// <param name="loginHistory">The login history.</param>
        /// <returns></returns>
        private async Task AddLoginHistory(LoginHistory loginHistory)
        {
            var newLoginHistory = new LoginHistories();
            newLoginHistory.Ipaddress = loginHistory.Ipaddress;
            newLoginHistory.IsActive = loginHistory.IsActive;
            newLoginHistory.LastRequestMade = DateTime.UtcNow;
            newLoginHistory.LoginStatus = loginHistory.LoginStatus;
            newLoginHistory.RequestHeader = loginHistory.RequestHeader;
            newLoginHistory.Token = loginHistory.Token;
            newLoginHistory.CreatedOn = DateTime.UtcNow;
            newLoginHistory.UserId = loginHistory.UserId;
            await loginHistoryRepository.AddLoginHistory(newLoginHistory);
        }

        /// <summary>
        /// Refresh Token Add
        /// </summary>
        /// <param name="model">Refresh Token Model</param>
        /// <returns></returns>
        private async Task AddRefreshToken(RefreshTokenModel model)
        {
            var refreshToken = new RefreshTokens()
            {
                JwtId = model.JwtId,
                Token = model.Token,
                UserId = model.UserId,
                AccountId = model.AccountId,
                IsUsed = model.IsUsed,
                IsRevorked = model.IsRevorked,
                CreatedOn = DateTime.UtcNow,
                ExpiryDate = model.ExpiryDate,
                JwtToken = model.JwtToken
            };
            await refreshTokenRepository.AddRefreshTokens(refreshToken);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <param name="model">Token Request Model</param>
        /// <returns></returns>
        public async Task<ResponseResult<SuccessMessageModel>> Logout(TokenRequestModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                new ResponseResult<SuccessMessageModel>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed
                    }
                };
            }

            var storedToken = await refreshTokenRepository.GetDetails(model.RefreshToken);
            
            if (storedToken == null)
            {
                new ResponseResult()
                {
                    Message = ResponseCode.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseCode.InternalServerError
                    }
                };
            }
            storedToken.IsRevorked = true;
            await refreshTokenRepository.UpdateDetail(storedToken);

            if (!string.IsNullOrWhiteSpace(model.Token))
            {                
                var loginHistory = await loginHistoryRepository.GetLoginHistory(model.Token.Replace("Bearer ", ""));

                if (loginHistory != null)
                {
                    loginHistory.IsActive = false;
                    loginHistory.LogoutTime = DateTime.UtcNow;
                    loginHistory.LoginStatus = false;
                    await loginHistoryRepository.UpdateLoginHistory(loginHistory);
                }
            }

            return new ResponseResult<SuccessMessageModel>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = new SuccessMessageModel()
                {
                    Message = "User has been successfully logged out."
                }
            };
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="refreshToken">Refresh Token</param>
        /// <returns></returns>
        public async Task<ResponseResult<RefreshTokenResultModel>> RefreshToken(string refreshToken)
        {
            var storedToken = await refreshTokenRepository.GetDetails(refreshToken);
            if (storedToken == null)
            {
                return new ResponseResult<RefreshTokenResultModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            var tokenDetails = new RefreshTokenModel()
            {
                UserId = storedToken.UserId,
                ExpiryDate = storedToken.ExpiryDate,
                IsRevorked = storedToken.IsRevorked,
                IsUsed = storedToken.IsUsed,
                JwtId = storedToken.JwtId,
                Token = storedToken.Token,
                AccountId = storedToken.AccountId,
                JwtToken = storedToken.JwtToken
            };

            var response = tokenService.VerifyToken(tokenDetails);

            if (!string.IsNullOrEmpty(response))
            {
                new ResponseResult<RefreshTokenResultModel>()
                {
                    Message = response,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = response
                    }
                };
            }

            // Generate a new token
            var user = await userRepository.GetUserByUserId(storedToken.AccountId, storedToken.UserId);
            if (user == null)
            {
                response = "Account and/or UserId are/is not valid ";
                new ResponseResult<RefreshTokenResultModel>()
                {
                    Message = response,
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = response
                    }
                };
            }

            var token = tokenService.GenerateJwtToken(user, out var tokenId);

            if (string.IsNullOrWhiteSpace(token))
            {
                // Update current token
                storedToken.IsUsed = true;
                await refreshTokenRepository.UpdateDetail(storedToken);

                return new ResponseResult<RefreshTokenResultModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                    }
                };
            }

            // Update current token
            storedToken.ExpiryDate = DateTime.UtcNow.AddHours(authenticationConfig.RefreshExpiryTime);
            storedToken.JwtId = tokenId;
            storedToken.JwtToken = token;
            await refreshTokenRepository.UpdateDetail(storedToken);

            return new ResponseResult<RefreshTokenResultModel>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = new RefreshTokenResultModel()
                {
                    Access = token
                }
            };
        }

        /// <summary>
        /// To Verify Token
        /// </summary>
        /// <param name="tokenModel">Verify Token Model</param>
        /// <param name="accountId">AccountId</param>
        /// <returns></returns>
        public async Task<ResponseResult> VerifyToken(VerifyTokenModel tokenModel, long accountId)
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

            var verifyResult = tokenService.VerifyJwtToken(tokenModel.Token);
            if (verifyResult.ResponseCode != ResponseCode.RecordFetched)
            {
                return new ResponseResult()
                {
                    Message = verifyResult.Message,
                    ResponseCode = verifyResult.ResponseCode,
                    Data = verifyResult.Data,
                    Error = verifyResult.Error
                };
            } 

            if (await this.refreshTokenRepository.VerifyToken(tokenModel.Token, verifyResult.Data, accountId))
            {
                return new ResponseResult()
                {
                    Message = ResponseMessage.RecordFetched,
                    ResponseCode = ResponseCode.RecordFetched,
                    Data = tokenModel.Token
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
        /// To Authenticate user
        /// </summary>
        /// <param name="login"> login data for user</param>
        /// <param name="ipAddress">IP Address</param>
        /// <param name="requestHeaders">Request Headers</param>
        /// <returns></returns>
        public async Task<User> AuthenticateWithMobile(Users user, long accountId)
        {
            

            var tokenResponse = tokenService.GenerateAuthToken(user);
            var loginHistory = new LoginHistory()
            {
                Ipaddress = "",
                RequestHeader = "",
                Token = string.Empty
            };

            loginHistory.UserId = user.UserId;

            // If token not generated, add login history and return.
            if (tokenResponse == null || !tokenResponse.Success || string.IsNullOrWhiteSpace(tokenResponse.Token)
                || tokenResponse.RefreshToken == null || string.IsNullOrWhiteSpace(tokenResponse.RefreshToken.Token))
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();

                return null;
            }

            loginHistory.Token = tokenResponse.Token;
            await AddRefreshToken(tokenResponse.RefreshToken);

           

            loginHistory.IsActive = loginHistory.LoginStatus = true;
            await AddLoginHistory(loginHistory);
            unitOfWork.Commit();

            var userRights = new List<UserRight>();
            if (user.UserRights != null && user.UserRights.Count > 0)
            {
                userRights = user.UserRights.Select(x => new UserRight()
                {
                    UserRightId = x.UserRightId,
                    AccountId = x.AccountId,
                    ModuleId = x.ModuleId,
                    IsPermission = x.IsPermission,
                }).ToList();
            }


            var userGroups = new List<UserGroupDetail>();
            if (user.UserGroupMappings != null && user.UserGroupMappings.Count > 0)
            {
                userGroups = user.UserGroupMappings.Select(x => new UserGroupDetail()
                {
                    UserGroupMappingId = x.UserGroupMappingId,
                    AccountId = x.AccountId,
                    GroupId = x.GroupId,
                    GroupName = x.Group?.Name
                }).ToList();


                var userGroupModules = await this.userRepository.GetUserGroupRights(user.UserId, accountId);
                if (userGroupModules.Count > 0)
                {
                    var userGroupRights = userGroupModules.Where(x => !userRights.Select(y => y.ModuleId).Contains(x))
                        .Select(x => new UserRight()
                        {
                            AccountId = accountId,
                            ModuleId = x,
                            IsPermission = true
                        });

                    if (userGroupRights.Count() > 0)
                    {
                        userRights.AddRange(userGroupRights);
                    }
                }
            }

            var domainUser = new User()
            {
                AccountId = user.AccountId,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                UserId = user.UserId,
                UserName = user.UserName,
                Token = tokenResponse.Token,
                Refresh = tokenResponse.RefreshToken.Token,
                UserStatus = user.UserStatus,
                UserRights = userRights,
                UserGroups = userGroups,
                ParkingProvidersLocationId = user.ParkingProvidersLocationId
            };
            return domainUser;
        }
    }
}
