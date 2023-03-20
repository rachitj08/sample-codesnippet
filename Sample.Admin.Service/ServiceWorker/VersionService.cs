using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public class VersionService : IVersionService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IVersionRepository versionRepository;

        private readonly IVersionModuleRepository versionModuleRepository;

        /// <summary>
        /// Version Service constructor to inject dependency
        /// </summary>
        /// <param name="unitOfWork">unit of work</param>
        /// <param name="versionRepository">version repository</param>
        public VersionService(IUnitOfWork unitOfWork, IVersionRepository versionRepository, IVersionModuleRepository versionModuleRepository)
        {
            this.unitOfWork = unitOfWork;
            this.versionRepository = versionRepository;
            this.versionModuleRepository = versionModuleRepository;
        }

        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Get All Versions
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<VersionsModel>> GetAllVersions(string ordering, string search, int offset, int pageSize, int pageNumber, bool all) => await this.versionRepository.GetAllVersions(ordering, search, offset, pageSize, pageNumber, all);

        /// <summary>
        /// Get Version By Id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public async Task<VersionModel> GetVersionById(int versionId)
        {
            var versionDetails = await this.versionRepository.GetVersionById(versionId);
            var modules = await this.versionRepository.GetVersionModulesByVersionId(versionId);
            if (versionDetails != null)
            {
                var version = new VersionModel()
                {
                    VersionId = versionDetails.VersionId,
                    VersionCode = versionDetails.VersionCode,
                    DisplayName = versionDetails.DisplayName,
                    Description = versionDetails.Description,
                    Status = versionDetails.Status
                };

                if (modules != null)
                {
                    version.Modules = modules.Select(x => new ModulesModel()
                    {
                        ModuleId = x.ModuleId,
                        Name = x.Name,
                        Description = x.Description,
                        DisplayName = x.DisplayName,
                        DisplayOrder = x.DisplayOrder,
                        IsNavigationItem = x.IsNavigationItem,
                        ParentModule = x.ParentModuleId,
                        Service = x.ServiceId,
                        Url = x.Url,
                        Status = x.Status,
                        IsVisible = x.IsVisible,
                    }).ToList();
                }
                return version;
            }
            return null;
        }

        /// <summary>
        /// To Create new  Version
        /// </summary>
        /// <param name="version">version object</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionsModel>> CreateVersion(VersionsModel version, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(version.DisplayName))
            {
                errorDetails.Add("displayName", new string[] { "This field may not be blank." });
            }
            else if (version.DisplayName.Length > 255)
            {
                errorDetails.Add("displayName", new string[] { "Ensure this field has no more than 255 characters." });
            }

            if (string.IsNullOrWhiteSpace(version.VersionCode))
            {
                errorDetails.Add("versionCode", new string[] { "This field may not be blank." });
            }
            else if (version.VersionCode.Length > 100)
            {
                errorDetails.Add("versionCode", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<VersionsModel>()
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

            var result = await this.versionRepository.CreateVersion(version, loggedInUserId);

            if (version.Modules != null)
            {
                foreach (long moduleId in version.Modules)
                {
                    await this.versionModuleRepository.CreateVersionModule(new VersionModulesModel()
                    {
                        VersionId = result.VersionId,
                        ModuleId = moduleId
                    });
                }
            }

            if (result.VersionId > 0)
            {
                return new ResponseResult<VersionsModel>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = result
                };
            }
            else
            {
                return new ResponseResult<VersionsModel>()
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
        /// To Update existing Version
        /// </summary>
        /// <param name="version">version object</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionsModel>> UpdateVersion(long versionId, VersionsModel version, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(version.DisplayName))
            {
                errorDetails.Add("displayName", new string[] { "This field may not be blank." });
            }
            else if (version.DisplayName.Length > 255)
            {
                errorDetails.Add("displayName", new string[] { "Ensure this field has no more than 255 characters." });
            }

            if (string.IsNullOrWhiteSpace(version.VersionCode))
            {
                errorDetails.Add("versionCode", new string[] { "This field may not be blank." });
            }
            else if (version.VersionCode.Length > 100)
            {
                errorDetails.Add("versionCode", new string[] { "Ensure this field has no more than 100 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<VersionsModel>()
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
            
            var result = await this.versionRepository.UpdateVersion(versionId, version, loggedInUserId);
            if (result == null)
            {
                return new ResponseResult<VersionsModel>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }

            await this.versionModuleRepository.DeleteVersionModulesByVersionId(versionId);
            if (version.Modules != null)
            {
                foreach (long moduleId in version.Modules)
                {
                    await this.versionModuleRepository.CreateVersionModule(new VersionModulesModel()
                    {
                        VersionId = result.VersionId,
                        ModuleId = moduleId
                    });
                }
            }

            if (result.VersionId > 0)
            {
                return new ResponseResult<VersionsModel>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = result
                };
            }
            else
            {
                return new ResponseResult<VersionsModel>()
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
        /// To Update Version Partially
        /// </summary>
        /// /// <param name="versionId">Version Id</param>
        /// <param name="version">New version object</param>
        /// <returns></returns>
        public async Task<ResponseResult<VersionsModel>> UpdatePartialVersion(long versionId, VersionsModel version, int loggedInUserId)
        {
            return await UpdateVersion(versionId, version, loggedInUserId);
        }

        /// <summary>
        /// To delete existing Version
        /// </summary>
        /// <param name="versionId">version identifier</param>
        /// <returns></returns>
        public async Task<long> DeleteVersion(long versionId) => await this.versionRepository.DeleteVersion(versionId);























































        /// <summary>
        /// To get version details with all features(Modules, Sub Modules, and Permissions) for given Version Id
        /// </summary>
        /// <param name="VersionId">Version Id</param>
        /// <returns></returns>
        //public async Task<ResponseResult> GetVersionByVersionId(int VersionId)
        //{
        //    ResponseResult responseResult = new ResponseResult();
        //    var versionNavigation = await this.versionRepository.GetVersionByVersionId(VersionId);
        //    if (versionNavigation == null)
        //    {
        //        responseResult.ResponseCode = ResponseCode.NoRecordFound;
        //        responseResult.Message = ResponseMessage.NoRecordFound;
        //    }
        //    else
        //    {
        //        var versionNavigationLookup = versionNavigation.ToLookup(c => c.ParentModuleId);
        //        foreach (var version in versionNavigation)
        //        {
        //            version.versionNavigations = versionNavigationLookup[version.ModuleId].ToList();
        //            //for removing duplicate items
        //            if (version.ParentModuleId != null && version.ParentModuleId != 0)
        //                versionNavigation.ToList().Remove(version);
        //        }

        //        responseResult.ResponseCode = ResponseCode.RecordFetched;
        //        responseResult.Message = ResponseMessage.RecordFetched;
        //        responseResult.Data = versionNavigation;
        //    }
        //    return responseResult;
        //}
        /// <summary>
        /// To get all versions with all features(Modules, Sub Modules, and Permissions) 
        /// </summary>
        /// <returns></returns>
        //public async Task<ResponseResult> GetVersions()
        //{
        //    ResponseResult responseResult = new ResponseResult();
        //    var versionNavigation = await this.versionRepository.GetVersions() as IEnumerable<Versions>;
        //    if (versionNavigation == null)
        //    {
        //        responseResult.ResponseCode = ResponseCode.NoRecordFound;
        //        responseResult.Message = ResponseMessage.NoRecordFound;
        //    }
        //    else
        //    {
        //        var versionNavigationLookup = versionNavigation.ToLookup(c => c.ParentModuleId);

        //        foreach (var version in versionNavigation)
        //        {
        //            version.versionNavigations = versionNavigationLookup[version.ModuleId].ToList();
        //            //for removing duplicate items
        //            if (version.ParentModuleId != null && version.ParentModuleId != 0)
        //                versionNavigation.ToList().Remove(version);
        //        }
        //        responseResult.ResponseCode = ResponseCode.RecordFetched;
        //        responseResult.Message = ResponseMessage.RecordFetched;
        //        responseResult.Data = versionNavigation;
        //    }
        //    return responseResult;
        //}




    }

}
