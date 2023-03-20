using Common.Enum;
using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.PBKDF2Hashing;
using Utility;

namespace Sample.Admin.Service.ServiceWorker
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
        private readonly IAdminUserRepository userRepository;

        /// <summary>
        /// refreshTokenRepository Private Member
        /// </summary>
        private readonly IRefreshTokenRepository refreshTokenRepository;

        /// <summary>
        /// loginHistoryRepository Private Member
        /// </summary>
        private readonly ILoginHistoryRepository loginHistoryRepository;
         
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
        public AuthenticationService(IUnitOfWork unitOfWork, IAdminUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository, ILoginHistoryRepository loginHistoryRepository, 
            ITokenService tokenService, IOptions<AuthenticationConfig> authenticationConfig, ICommonHelper commonHelper)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.loginHistoryRepository = loginHistoryRepository;
            this.tokenService = tokenService;
            this.commonHelper = commonHelper;
            this.authenticationConfig = authenticationConfig.Value;
        }


        /// <summary>
        /// To Authenticate user
        /// </summary>
        /// <param name="login"> login data for user</param>
        /// <returns></returns>
        public async Task<ResponseResult<LoginAdminUserModel>> Authenticate(LoginModel login, string ipAddress, string requestHeaders)
        {
            if(login == null)
            {
                return new ResponseResult<LoginAdminUserModel>()
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
                return new ResponseResult<LoginAdminUserModel>()
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

            var loginHistory = new LoginHistoryModel()
            {
                Ipaddress = ipAddress,
                RequestHeader = requestHeaders,
                Token = String.Empty
            };
            var user = await this.userRepository.GetUserByUserName(login.UserName);

            // If user not found, add login history and return.
            if (user == null)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult<LoginAdminUserModel>()
                {
                    Message = "User name is not valid",
                    ResponseCode = ResponseCode.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User name is not valid",
                    }

                };
            }

            // If user found but user status not activated, add login history and return.
            if (user.UserStatus != (int)UserStatus.Active)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                return new ResponseResult<LoginAdminUserModel>()
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
            // If user password Expired
            if (user.PasswordExpirationDate != null && DateTime.UtcNow.Date > user.PasswordExpirationDate)
            {
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();

                return new ResponseResult<LoginAdminUserModel>()
                {
                    Message = "User password expired, Please reset your password",
                    ResponseCode = ResponseCode.PasswordExpired,
                    Error = new ErrorResponseResult()
                    {
                        Message = "User password expired, Please reset your password",
                    }
                };
            }

            // Validate Password
            var cryptography = new PBKDF2Cryptography();
            bool password = cryptography.ValidatePassword(login.Password, user.PasswordHash, user.PasswordSalt);

            // If password didn't match, add login history and return.
            if (!password)
            {
                user.FailedLoginAttempts++;
                await userRepository.UpdateUser(user);
                await AddLoginHistory(loginHistory);
                unitOfWork.Commit();
                
                return new ResponseResult<LoginAdminUserModel>()
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

                return new ResponseResult<LoginAdminUserModel>()
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
            user.FailedLoginAttempts = 0;
            await userRepository.UpdateUser(user);

            loginHistory.IsActive = loginHistory.LoginStatus = true;
            await AddLoginHistory(loginHistory);
            unitOfWork.Commit();

            var domainUser = new LoginAdminUserModel()
            {
                Email = user.EmailAddress,
                UserId = user.UserId,
                UserName = user.UserName,
                Token = tokenResponse.Token,
                Refresh = tokenResponse.RefreshToken.Token,
                Status = user.UserStatus
            };

            return new ResponseResult<LoginAdminUserModel>()
            {
                Data = domainUser,
                Message = ResponseMessage.Authenticated,
                ResponseCode = ResponseCode.ValidLogin
            };
        }

        /// <summary>
        /// Add login history (failed or successful).
        /// </summary>
        /// <param name="loginHistoryModel">The login history.</param>
        /// <returns></returns>
        private async Task AddLoginHistory(LoginHistoryModel loginHistoryModel)
        {
            var newLoginHistory = new LoginHistories();
            newLoginHistory.Ipaddress = loginHistoryModel.Ipaddress;
            newLoginHistory.IsActive = loginHistoryModel.IsActive;
            newLoginHistory.LastRequestMade = DateTime.UtcNow;
            newLoginHistory.LoginStatus = loginHistoryModel.LoginStatus;
            newLoginHistory.RequestHeader = loginHistoryModel.RequestHeader;
            newLoginHistory.Token = loginHistoryModel.Token;
            newLoginHistory.CreatedOn = DateTime.UtcNow;
            newLoginHistory.UserId = loginHistoryModel.UserId;
            await loginHistoryRepository.AddLoginHistory(newLoginHistory);
        }

        /// <summary>
        /// Refresh Token Add
        /// </summary>
        /// <param name="model">Refresh Token Model</param>
        /// <returns></returns>
        private async Task AddRefreshToken(RefreshTokenModel model)
        {
            var refreshToken = new AdminRefreshTokens()
            {
                JwtId = model.JwtId,
                Token = model.Token,
                UserId = model.UserId,
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
                Token = storedToken.Token
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
            var user = await userRepository.GetUserByUserId(storedToken.UserId);
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
    }
}
