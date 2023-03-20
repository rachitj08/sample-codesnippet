using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public class VersionModuleService : IVersionModuleService
    {

        private readonly IUnitOfWork unitOfWork;

        private readonly IVersionModuleRepository versionModulesRepository;


        /// <summary>
        /// Version Module Service constructor to Inject dependency
        /// </summary>
        /// <param name="unitOfWork">unit of work</param>
        /// <param name="versionModuleRepository">version module repository</param>
        public VersionModuleService(IUnitOfWork unitOfWork, IVersionModuleRepository versionModulesRepository)
        {
            this.unitOfWork = unitOfWork;
            this.versionModulesRepository = versionModulesRepository;
        }

        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Get All Version Module
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList> GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all) => await this.versionModulesRepository.GetAllVersionModules(ordering, offset, pageSize, pageNumber, all);

        public async Task<VersionModulesModel> GetVersionModuleById(long versionModuleId) => await this.versionModulesRepository.GetVersionModuleById(versionModuleId);

        /// <summary>
        /// Get Version  Modules By VersionId
        /// </summary>
        /// <param name="versionId">version identifier</param>
        /// <returns></returns>
        public async Task<IEnumerable<VersionModules>> GetVersionModulesByVersionId(int versionId) => await this.versionModulesRepository.GetVersionModulesByVersionId(versionId);

        /// <summary>
        /// To Create new version Module
        /// </summary>
        /// <param name="versionModule">version module object</param>
        /// <returns></returns>
        public async Task<VersionModulesModel> CreateVersionModule(VersionModulesModel versionModule) {
            return await this.versionModulesRepository.CreateVersionModule(versionModule);
        }

        public async Task<VersionModulesModel> UpdateVersionModule(long versionModuleId, VersionModulesModel versionModule) => await this.versionModulesRepository.UpdateVersionModule(versionModuleId, versionModule);

        public async Task<VersionModulesModel> UpdatePartialVersionModule(long versionModuleId, VersionModulesModel versionModule) => await this.versionModulesRepository.UpdateVersionModule(versionModuleId, versionModule);

        /// <summary>
        /// To Delete Versions Module
        /// </summary>
        /// <param name="versionModuleId">version module identifier</param>
        /// <returns></returns>
        public async Task<long> DeleteVersionModules(long versionModuleId) => await this.versionModulesRepository.DeleteVersionModules(versionModuleId);

    }
}
