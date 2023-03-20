using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utility;
using Sample.Customer.HttpAggregator.Config.OperationsConfig;
using Sample.Customer.HttpAggregator.Config.UrlsConfig;
using Sample.Customer.HttpAggregator.IServices.EmailParse;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Services.EmailParse
{
    /// <summary>
    /// Email Parser
    /// </summary>
    public class EmailParserService : IEmailParserService
    {
        /// <summary>
        /// 
        /// </summary>

        private readonly HttpClient httpClient;

        private readonly ILogger<EmailParserService> logger;

        private readonly ICommonHelper commonHelper;

        private readonly BaseUrlsConfig urls;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="commonHelper"></param>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        /// <param name="kafkaConfig"></param>
        public EmailParserService(HttpClient httpClient, ICommonHelper commonHelper,
           ILogger<EmailParserService> logger,  IOptions<BaseUrlsConfig> config)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(commonHelper), commonHelper);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
           
            this.commonHelper = commonHelper;
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
        }

        /// <summary>
        /// Email Parser CallBack 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="authorizationKey"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> EmailParserCallBack(EmailParserCallBackResponse model, string authorizationKey, long accountId)
        {
            // Validation
            if (model == null || string.IsNullOrWhiteSpace(model.UserData) || string.IsNullOrWhiteSpace(authorizationKey))
            {
                return new ResponseResult<bool>()
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            // Get user credentials from header
            var encryptedData = authorizationKey.Replace("Basic", "").Trim();
            byte[] byteData = Convert.FromBase64String(encryptedData);
            string decodedString = Encoding.UTF8.GetString(byteData);
            if (string.IsNullOrWhiteSpace(decodedString) || !decodedString.Contains(":"))
            {
                return new ResponseResult<bool>()
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = ResponseMessage.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            // Check user is authorized or not
            var authArray = decodedString.Split(":");
            if (string.IsNullOrWhiteSpace(authArray[0]) || string.IsNullOrWhiteSpace(authArray[1]))
            {
                return new ResponseResult<bool>()
                {
                    ResponseCode = ResponseCode.Unauthorized,
                    Message = ResponseMessage.Unauthorized,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.Unauthorized,
                    }
                };
            };

            // Get Message Id from User Data
            if (string.IsNullOrWhiteSpace(model.UserData) || !Guid.TryParse(model.UserData, out var messageId))
            {
                return new ResponseResult<bool>()
                {
                    ResponseCode = ResponseCode.ValidationFailed,
                    Message = "Invalid Message Id",
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }
           
            
            return new ResponseResult<bool>()
            {
                ResponseCode = ResponseCode.RecordSaved,
                Message = ResponseMessage.RecordSaved,
                Data = false
            };
        }


        /// <summary>
        /// Process All Pending Email
        /// </summary>
        /// <param name="authorizationKey"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<ResponseResult<bool>> ProcessAllPendingEmail(string authorizationKey, long accountId)
        {
            //if (!File.Exists(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ProcessAllPendingEmail.txt")))
            //    File.Create(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ProcessAllPendingEmail.txt"));
            //File.AppendAllText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "ProcessAllPendingEmail.txt"), "ProcessAllPendingEmailCalled" + DateTime.Now.ToLongDateString());
            var emailParseDetailsResponse = await this.httpClient.GetAsync(this.urls.CustomerAPI + CustomerAPIOperations.GetAllPendingEmailParserDetails());
            var result = emailParseDetailsResponse.GetResponseResult<List<EmailParseDetails>>();

            if (result.ResponseCode != ResponseCode.RecordFetched || result.Data == null || result.Data.Count < 1)
            {
                return new ResponseResult<bool>()
                {
                    ResponseCode = result.ResponseCode,
                    Message = result.Message,
                    Error = result.Error
                };
            }
            Console.WriteLine($"EmailParserService ==> API called ==> ProcessAllPendingEmail");
            foreach (var item in result.Data)
            {
                //Console.WriteLine($"item.RequestIds{item.RequestIds} | item.MessageId{item.MessageId}");
                var model = new EmailParserCallBackResponse()
                {
                    UserData = item.MessageId.ToString()
                };
                await EmailParserCallBack(model, authorizationKey, accountId).ConfigureAwait(false);
            }

            return new ResponseResult<bool>()
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Data = true
            };
        }
    }
}
