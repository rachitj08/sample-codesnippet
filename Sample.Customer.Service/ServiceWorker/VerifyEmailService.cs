using Common.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utilities;
using Utility;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    public class VerifyEmailService : IVerifyEmailService
    {
        private readonly ICommonHelper commonHelper;
        private readonly SendVerificationMailConfig sendVerificationMailConfig;
        private readonly IUserRepository usersRepository;
        private readonly IUnitOfWork unitOfWork;
        public VerifyEmailService(ICommonHelper commonHelper, IOptions<SendVerificationMailConfig> sendVerificationMailConfig, IUserRepository usersRepository, IUnitOfWork unitOfWork)
        {
            Check.Argument.IsNotNull(nameof(commonHelper), commonHelper);
            Check.Argument.IsNotNull(nameof(sendVerificationMailConfig), sendVerificationMailConfig);
            Check.Argument.IsNotNull(nameof(usersRepository), usersRepository);
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);

            this.commonHelper = commonHelper;
            this.sendVerificationMailConfig = sendVerificationMailConfig.Value;
            this.usersRepository = usersRepository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseResult<SuccessMessageModel>> ConfirmEmail(VerifyEmailVM model,long loggedAccountId,long loggedInUserId)
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
            var uid = HttpUtility.UrlDecode(commonHelper.DecryptString(model.Uid, sendVerificationMailConfig.EncryptKey));
            var token = HttpUtility.UrlDecode(commonHelper.DecryptString(model.Token, sendVerificationMailConfig.EncryptKey));

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
            if (tokenCreatedOn.AddHours(sendVerificationMailConfig.ExpiryTimeInHours) < DateTime.Now)
            {
                response.Message = "Email verification link has been expired.";
                response.ResponseCode = ResponseCode.Unauthorized;
                response.Error = new ErrorResponseResult()
                {
                    Message = "Email verification link has been expired."
                };
                return response;
            }
            var user = await usersRepository.GetUserByUserId(loggedAccountId,Convert.ToInt32(uid));

            if (user == null)
            {
                response.Message = "Email verification link has been expired.";
                response.ResponseCode = ResponseCode.Unauthorized;
                response.Error = new ErrorResponseResult()
                {
                    Message = "Email verification link has been expired."
                };
                return response;
            }
            user.IsEmailVerified = true;
            user.UpdatedBy = loggedInUserId;
            user.UpdatedOn = DateTime.Now;
            await this.usersRepository.UpdateUser(user);

            if (unitOfWork.CommitWithStatus() < 1)
            {
                response.Message = ResponseMessage.InternalServerError;
                response.ResponseCode = ResponseCode.InternalServerError;
                response.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return response;
            }

            response.Message = "Email verification complete.";
            response.ResponseCode = ResponseCode.RecordSaved;
            response.Data = new SuccessMessageModel()
            {
                Message = "Email verification complete."
            };
            return response;
        }
    }
}
