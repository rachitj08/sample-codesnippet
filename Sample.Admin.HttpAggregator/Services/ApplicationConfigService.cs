using Common.Model;
using Core.API.ExtensionMethods;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities;
using Sample.Admin.HttpAggregator.Config.OperationsConfig;
using Sample.Admin.HttpAggregator.Config.UrlsConfig;
using Sample.Admin.HttpAggregator.IServices;
using Sample.Admin.Model;

namespace Sample.Admin.HttpAggregator.Services
{
    /// <summary>
    /// ApplicationConfig Service Class
    /// </summary>
    public class ApplicationConfigService : IApplicationConfigService
    {
        private readonly HttpClient httpClient;

        private readonly ILogger<ApplicationConfigService> logger;
        private IConfiguration configuration { get; }

        private readonly BaseUrlsConfig urls;

        /// <summary>
        /// Application Config service constructor to inject required dependency
        /// </summary>
        /// <param name="httpClient">The httpClient</param>
        /// <param name="logger">The logger service</param>
        /// <param name="config">The Base Urls config</param>
        /// <param name="configuration"></param>
        public ApplicationConfigService(HttpClient httpClient,
            ILogger<ApplicationConfigService> logger,
            IOptions<BaseUrlsConfig> config, IConfiguration configuration)
        {
            Check.Argument.IsNotNull(nameof(httpClient), httpClient);
            Check.Argument.IsNotNull(nameof(logger), logger);
            Check.Argument.IsNotNull(nameof(config), config);
            this.httpClient = httpClient;
            this.logger = logger;
            this.urls = config.Value;
            this.configuration = configuration;
        }


        /// <summary>
        /// Get Application Config Details
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResult<ApplicationConfig>> GetApplicationConfig()
        {
            var httpResponse = await this.httpClient.GetAsync(this.urls.AdminAPI + AdminOperations.GetApplicationConfig()); 
            return httpResponse.GetFetchRecords<ApplicationConfig>();
        }

        ///<summary>
        /// Get Database Details
        ///</summary>
        ///<returns></returns>
        public async Task<ResponseResult<DatabaseDetails>> GetDatabaseDetails()
        {            
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                return new ResponseResult<DatabaseDetails>()
                {
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    },
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                };
            }
            var connectionData = connectionString.Split(";");

            if(connectionData.Length < 1)
            {
                return new ResponseResult<DatabaseDetails>()
                {
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    },
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                };
            }

            var connectionValues = connectionData.Select(x => x.Split("=")).Where(x=> x.Length == 2).Select(x => new { Key = x[0].Trim(), Value = x[1].Trim() });

            if(connectionValues == null || connectionValues.Count() < 1)
            {
                return new ResponseResult<DatabaseDetails>()
                {
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    },
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                };
            }

            var databaseDetails = new DatabaseDetails();
            var portNoValue = connectionValues.Where(x => x.Key == "Port").Select(x => x.Value).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(portNoValue)) portNoValue = "5432";
            databaseDetails.ServerName = connectionValues.Where(x => x.Key == "Server").Select(x => x.Value).FirstOrDefault();
            databaseDetails.PortNo = int.Parse(portNoValue);
            databaseDetails.DatabaseName = connectionValues.Where(x => x.Key == "Database").Select(x => x.Value).FirstOrDefault();
            databaseDetails.UserName = connectionValues.Where(x => x.Key == "user id").Select(x => x.Value).FirstOrDefault();
            databaseDetails.Password = connectionValues.Where(x => x.Key == "password").Select(x => x.Value).FirstOrDefault();
            databaseDetails.UserManagementServiceSchema = configuration.GetValue<string>("ServiceSchemaNames:UserManagement", "");
            databaseDetails.ReportServiceSchema = configuration.GetValue<string>("ServiceSchemaNames:Report", "");

            if (string.IsNullOrWhiteSpace(databaseDetails.ServerName) || databaseDetails.PortNo < 1
                || string.IsNullOrWhiteSpace(databaseDetails.DatabaseName) 
                || string.IsNullOrWhiteSpace(databaseDetails.UserName) 
                || string.IsNullOrWhiteSpace(databaseDetails.Password) 
                || string.IsNullOrWhiteSpace(databaseDetails.UserManagementServiceSchema)
                || string.IsNullOrWhiteSpace(databaseDetails.ReportServiceSchema))
            {
                return new ResponseResult<DatabaseDetails>()
                {
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound,
                    },
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                };
            }

            return new ResponseResult<DatabaseDetails>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = databaseDetails
            };
        }
    }
}
