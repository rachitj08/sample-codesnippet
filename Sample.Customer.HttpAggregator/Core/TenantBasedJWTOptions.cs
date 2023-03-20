using Common.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sample.Customer.HttpAggregator.IServices.UserManagement;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Core
{
    /// <summary>
    /// Tenant Based JWT Options
    /// </summary>
    public class TenantBasedJWTOptions : TenantBasedSocialLoginOptionsBase<JwtBearerOptions>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationConfigKeysValuesService _authService;

        /// <summary>
        /// Tenant Based JWT Options
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="sources"></param>
        /// <param name="cache"></param>
        /// <param name="configuration"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="authService"></param>
        public TenantBasedJWTOptions(
            IOptionsFactory<JwtBearerOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<JwtBearerOptions>> sources,
            IOptionsMonitorCache<JwtBearerOptions> cache,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IAuthenticationConfigKeysValuesService authService
            ) : base(factory, sources, cache)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        /// <summary>
        /// Does Current Tenant Has Settings
        /// </summary>
        /// <returns></returns>
        protected override bool DoesCurrentTenantHasSettings()
        {
            if(_httpContextAccessor?.HttpContext?.Request?.Headers != null 
                && _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("authenticationCategory")
                && _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("authenticationTypeId"))
            {
                var authCategory = _httpContextAccessor.HttpContext.Request.Headers["authenticationCategory"];
                var authType = _httpContextAccessor.HttpContext.Request.Headers["authenticationTypeId"];
                return (authCategory == "E" && authType == "A");
            }

            return false;
        }

        /// <summary>
        /// Set Host Settings
        /// </summary>
        /// <param name="options"></param>
        protected override void SetHostSettings(JwtBearerOptions options)
        {
            var settings = GetDefaultTenantDetails();
            SetOptions(options, settings);
        }

        /// <summary>
        /// Set Tenant Settings
        /// </summary>
        /// <param name="options"></param>
        protected override void SetTenantSettings(JwtBearerOptions options)
        {
            var settings = GetCurrentTenantSetting();
            SetOptions(options, settings);
        }

        /// <summary>
        /// Set Options
        /// </summary>
        /// <param name="options"></param>
        /// <param name="settings"></param>
        private void SetOptions(JwtBearerOptions options, TenantJwtBearerSettingsModel settings)
        {
            options.Authority = settings.Authority;
            options.Audience = settings.Audience;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = settings.ValidateIssuer,
                ValidateAudience = settings.ValidateAudience,
                ValidAudiences = settings.ValidAudience,
                ValidIssuers = settings.ValidIssuer
            };
            if (!string.IsNullOrWhiteSpace(settings.IssuerSigningKey))
            {
                options.TokenValidationParameters.RequireExpirationTime = false;
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.IssuerSigningKey));
            }
            options.RequireHttpsMetadata = false;
        }

        private TenantJwtBearerSettingsModel GetCurrentTenantSetting()
        {
            var settings = Task.Run(async ()=> await _authService.GetTenantAuthSettings("A"));
            if (settings == null || settings.Result == null || settings.Result.Data == null 
                || settings.Result.Data.Count < 1 || settings.Result.ResponseCode == null
                || settings.Result.ResponseCode != ResponseCode.RecordFetched
                || !settings.Result.Data.TryGetValue("ClientId", out var clientId) 
                || !settings.Result.Data.TryGetValue("TenantId", out var tenantId))
            {
                return new TenantJwtBearerSettingsModel()
                {
                    Authority = "https://login.microsoftonline.com/common",
                    Audience = "",
                    ValidAudience = new List<string> { "" },
                    ValidIssuer = new List<string> { "" },
                    ValidateIssuer = false,
                    ValidateAudience = true,
                };
            }

            return new TenantJwtBearerSettingsModel()
            {
                Authority = "https://login.microsoftonline.com/" + tenantId,
                Audience = $"api://" + clientId,
                ValidAudience = new List<string> { clientId },
                ValidIssuer = new List<string> { 
                    "https://login.microsoftonline.com/" + tenantId + "/v2.0", 
                    "https://sts.windows.net/" + tenantId + "/"
                },
                ValidateIssuer = true,
                ValidateAudience = false,
            };
        }

        private TenantJwtBearerSettingsModel GetDefaultTenantDetails()
        {
            var iamURL = _configuration["Authentication:JWT_IS:IssuerURL"];
            var key = _configuration["Authentication:JWT_IS:Secret"];
            return new TenantJwtBearerSettingsModel()
            {
                Authority = "",
                Audience = "",
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidAudience = null,
                ValidIssuer = new List<string>() { iamURL },
                IssuerSigningKey = key
            };

        }
    } 
}
