using Sample.Admin.Service.Infrastructure.Repository;
using System.Threading.Tasks;
using Utility;
using Sample.Admin.Model;

namespace Sample.Admin.Service.ServiceWorker
{
    public class ApplicationConfigService : IApplicationConfigService
    {
        /// <summary>
        /// Application Config Repository Private Member
        /// </summary>
        /// 
        private readonly IApplicationConfigRepository applicationConfigRepository;

        /// <summary>
        /// commonHelper Private Member
        /// </summary>
        private readonly ICommonHelper commonHelper;

        public ApplicationConfigService(IApplicationConfigRepository applicationConfigRepository, ICommonHelper commonHelper)
        {
            this.applicationConfigRepository = applicationConfigRepository;
            this.commonHelper = commonHelper;
        }

        /// <summary>
        /// Get Application Config Details
        /// </summary>
        /// <returns></returns>
        public async Task<ApplicationConfig> GetApplicationConfig()
        {
            var applicationConfig = await this.applicationConfigRepository.GetApplicationConfig();

            if (applicationConfig == null)
                return null;

            return new ApplicationConfig()
            {
                ApplicationName = applicationConfig.ApplicationName,
                CopyrightDescription = applicationConfig.CopyrightDescription,
                DefaultCss = commonHelper.GetTenantTheme(applicationConfig.DefaultCss),
                DefaultLogo = applicationConfig.DefaultLogo,
                DefaultOrgName = applicationConfig.DefaultOrgName
            };
        }
    }
}
