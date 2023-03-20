using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sample.Admin.Service.Infrastructure.Repository;

namespace Sample.Admin.Service.ServiceWorker
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IModuleRepository moduleRepository;

        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration configuration { get; }

        /// <summary>
        /// Modules Service constructor to Inject dependency
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="moduleRepository">module repository</param>
        public ModuleService(IUnitOfWork unitOfWork, IModuleRepository moduleRepository, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.moduleRepository = moduleRepository;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get All Modules
        /// </summary>
        /// <returns></returns>
        public async Task<List<Module>> GetModuleList(bool isNavigationItem, long accountId, int serviceId)
        {
            var moduleList = await this.moduleRepository.GetAllModules(isNavigationItem, accountId, serviceId);

            return moduleList.Select(x => new Module()
            {
                Name = x.Name,
                Description = x.Description,
                DisplayName = x.DisplayName,
                DisplayOrder = x.DisplayOrder,
                IsNavigationItem = x.IsNavigationItem,
                IsVisible = x.IsVisible,
                ModuleId = x.ModuleId,
                ParentModuleId = x.ParentModuleId,
                ServiceId = x.ServiceId,
                Status = x.Status,
                Url = x.Url,
                DataSourceId = x.DataSourceId
            }).ToList();
        }


        /// <summary>
        /// Get All Modules
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<ModulesModel>> GetAllModules(string ordering, string search, int offset, int pageSize, int pageNumber, string serviceName, bool all) => await this.moduleRepository.GetAllModules(ordering, search, offset, pageSize, pageNumber, serviceName, all);


        /// <summary>
        /// Get Module By Id
        /// </summary>
        /// <returns></returns>
        public async Task<ModulesModel> GetModuleById(long moduleId)
        {
            var module = await this.moduleRepository.GetModuleById(moduleId);

            if (module != null)
            {
                return new ModulesModel()
                {
                    Description = module.Description,
                    DisplayName = module.DisplayName,
                    DisplayOrder = module.DisplayOrder,
                    IsNavigationItem = module.IsNavigationItem,
                    IsVisible = module.IsVisible,
                    ModuleId = module.ModuleId,
                    Name = module.Name,
                    ParentModule = module.ParentModuleId,
                    Service = module.ServiceId,
                    Status = module.Status,
                    Url = module.Url                    
                };
            }

            return null;
        }

        /// <summary>
        /// Get Sub Modules By ModuleId
        /// </summary>
        /// <param name="moduleId"> module identifier</param>
        /// <returns></returns>
        public async Task<IEnumerable<Modules>> GetSubModulesByModuleId(long moduleId) => await this.moduleRepository.GetSubModulesByModuleId(moduleId);

        /// <summary>
        /// To Create new  Module
        /// </summary>
        /// <param name="module">module object</param>
        /// <returns></returns>
        public async Task<ResponseResult<ModulesModel>> CreateModule(ModulesModel module, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(module.Name))
            {
                errorDetails.Add("name", new string[] { "This field may not be blank." });
            }
            else if (module.Name.Length > 50)
            {
                errorDetails.Add("name", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (string.IsNullOrWhiteSpace(module.DisplayName))
            {
                errorDetails.Add("displayName", new string[] { "This field may not be blank." });
            }
            else if (module.DisplayName.Length > 100)
            {
                errorDetails.Add("displayName", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<ModulesModel>()
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

            var newModule = new Modules()
            {
                Name = module.Name,
                Description = module.Description,
                DisplayName = module.DisplayName,
                DisplayOrder = module.DisplayOrder,
                ModuleId = module.ModuleId,
                Status = module.Status,
                IsNavigationItem = module.IsNavigationItem,
                IsVisible = module.IsVisible,
                ParentModuleId = module.ParentModule,
                ServiceId = module.Service,
                Url = module.Url,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUserId,
            };

            var moduleId = await this.moduleRepository.CreateModule(newModule);
            if (moduleId > 0)
            {
                module.ModuleId = moduleId;
                return new ResponseResult<ModulesModel>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = module
                };
            }
            else
            {
                return new ResponseResult<ModulesModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
        }

        /// <summary>
        /// To Update existing Module
        /// </summary>
        /// <param name="module">module object</param>
        /// <returns></returns>
        public async Task<ResponseResult<ModulesModel>> UpdateModule(long moduleId, ModulesModel module, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(module.Name))
            {
                errorDetails.Add("name", new string[] { "This field may not be blank." });
            }
            else if (module.Name.Length > 255)
            {
                errorDetails.Add("name", new string[] { "Ensure this field has no more than 255 characters." });
            }

            if (string.IsNullOrWhiteSpace(module.DisplayName))
            {
                errorDetails.Add("displayName", new string[] { "This field may not be blank." });
            }
            else if (module.DisplayName.Length > 100)
            {
                errorDetails.Add("displayName", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<ModulesModel>()
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

            var moduleNew = await this.moduleRepository.GetModuleById(moduleId);
            if (moduleNew != null)
            {
                moduleNew.IsNavigationItem = module.IsNavigationItem;
                moduleNew.Name = module.Name;
                moduleNew.DisplayName = module.DisplayName;
                moduleNew.Description = module.Description;
                moduleNew.Url = module.Url;
                moduleNew.DisplayOrder = module.DisplayOrder;
                moduleNew.IsVisible = module.IsVisible;
                moduleNew.Status = module.Status;
                moduleNew.ParentModuleId = module.ParentModule;
                moduleNew.ServiceId = module.Service;
                moduleNew.UpdatedOn = DateTime.UtcNow;
                moduleNew.UpdatedBy = loggedInUserId;

                var data = await this.moduleRepository.UpdateModule(moduleNew);
                if (data > 0)
                {
                    return new ResponseResult<ModulesModel>()
                    {
                        Message = ResponseMessage.RecordSaved,
                        ResponseCode = ResponseCode.RecordSaved,
                        Data = module
                    };
                }
                else
                {
                    return new ResponseResult<ModulesModel>()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError,
                        Error = new ErrorResponseResult()
                        {
                            Message = ResponseMessage.InternalServerError
                        }
                    };
                }
            }
            else
            {
                return new ResponseResult<ModulesModel>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    }
                };
            }
        }

        /// <summary>
        /// To Update Module Partially
        /// </summary>
        /// /// <param name="moduleId">Module ID</param>
        /// <param name="module">New module object</param>
        /// <returns></returns>
        public async Task<ResponseResult<ModulesModel>> UpdatePartialModule(long moduleId, ModulesModel module, int loggedInUserId)
        {
            return await UpdateModule(moduleId, module, loggedInUserId);
        }

        /// <summary>
        /// To delete existing Module
        /// </summary>
        /// <param name="moduleId">module identifier</param>
        /// <returns></returns>
        public async Task<long> DeleteModule(long moduleId) => await this.moduleRepository.DeleteModule(moduleId);

        /// <summary>
        /// Get Modules For Service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public async Task<List<ModulesAllModel>> GetModulesForService(string serviceName)
        {
            var modules = await this.moduleRepository.GetModulesForService(serviceName);

            // Create User Modules Hierarchy
            if (modules != null && modules.Count > 0)
            {
                var subModulsLookup = modules.ToLookup(c => c.ParentModuleId);
                foreach (var nav in modules.ToArray())
                {
                    nav.SubModules = subModulsLookup[nav.ModuleId].OrderBy(x => x.DisplayOrder).ToList();

                    if (nav.ParentModuleId != null && nav.ParentModuleId != 0)
                    {
                        modules.Remove(nav);
                    }
                }
                modules = modules.OrderBy(x => x.DisplayOrder).ToList();
            }

            return modules;
        }

        /// <summary>
        ///  Information for module by name
        /// </summary>
        /// <param name="accountId">account Id</param>
        /// <param name="serviceName">service Name</param>
        /// <param name="moduleName">module Name</param>
        /// <returns></returns>
        public async Task<long> GetModuleByName(long accountId, int serviceId, string moduleName)
        {
            return await this.moduleRepository.GetModuleByName(accountId, serviceId, moduleName);
        }
        public async Task<List<Module>> GetModulesByAccountId(long accountId, bool isNavigationItem = true)
        {
            return await this.moduleRepository.GetModulesByAccountId(accountId, isNavigationItem);
        }
    }
}
