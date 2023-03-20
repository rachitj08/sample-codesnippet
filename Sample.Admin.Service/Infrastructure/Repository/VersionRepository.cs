using Sample.Admin.Service.Infrastructure.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Sample.Admin.Service.Helpers;
using Common.Model;
using System.Text;
using Sample.Admin.Model;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class VersionRepository : RepositoryBase<Versions>, IVersionRepository
    {
        public IConfiguration configuration { get; }

        private readonly IMapper mapper;

        public VersionRepository(CloudAcceleratorContext context, IMapper mapper, IConfiguration configuration) : base(context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get All Versions
        /// </summary>
        /// <returns></returns>
        public Task<ResponseResultList<VersionsModel>> GetAllVersions(string ordering, string search, int offset, int pageSize, int pageNumber, bool all) 
        {
            IQueryable<Versions> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("/version/");
            StringBuilder sbPrevious = new StringBuilder("/version/");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string[] searchList = search.Split(",");

                foreach (string item in searchList)
                {
                    Int64.TryParse(item, out long versionId);

                    if (result == null)
                    {
                        result = from versions in base.context.Versions
                                 where versions.VersionId == versionId || versions.DisplayName.Contains(item) 
                                 select versions;
                    }
                    else
                    {
                        result = result.Concat(from versions in base.context.Versions
                                               where versions.VersionId == versionId || versions.DisplayName.Contains(item)
                                               select versions);
                    }
                }

                sbNext.Append("?search=" + search + "&");
                sbPrevious.Append("?search=" + search + "&");
            }
            else
            {
                sbNext.Append("?");
                sbPrevious.Append("?");
            }

            if (!all)
            {
                if (string.IsNullOrWhiteSpace(search))
                    result = from versions in base.context.Versions
                             select versions;

                listCount = result.Count();

                var rowIndex = 0;

                if (pageNumber > 0)
                {
                    rowIndex = (pageNumber - 1) * pageSize;
                    sbNext.Append("PageNumber=" + (pageNumber + 1) + "&PageSize=" + pageSize);
                    sbPrevious.Append("PageNumber=" + (pageNumber - 1) + "&PageSize=" + pageSize);
                }
                else if (offset > 0)
                {
                    rowIndex = offset;
                    sbNext.Append("PageNumber=" + (offset + pageSize) + "&PageSize=" + pageSize);
                    sbPrevious.Append("PageNumber=" + (offset - pageSize) + "&PageSize=" + pageSize);
                }
                else
                {
                    rowIndex = 1;
                    sbNext.Append("PageNumber=" + (rowIndex + 1) + "&PageSize=" + pageSize);
                    sbPrevious.Append("PageNumber=" + (rowIndex - 1) + "&PageSize=" + pageSize);
                }

                result = result.Skip(rowIndex).Take(pageSize);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(search))
                    result = from versions in base.context.Versions
                             select versions;

                listCount = result.Count();
                sbNext.Append("all=" + all);
                sbPrevious.Append("all=" + all);
            }

            if (!string.IsNullOrWhiteSpace(ordering))
            {
                ordering = string.Concat(ordering[0].ToString().ToUpper(), ordering.AsSpan(1));
                if (typeof(Versions).GetProperty(ordering) != null)
                {
                    result = result.OrderBy(m => EF.Property<object>(m, ordering));
                    sbNext.Append("&ordering=" + ordering);
                    sbPrevious.Append("&ordering=" + ordering);
                }
            }
            else
            {
                result = result.OrderByDescending(x => x.CreatedOn);
            }

            List<VersionsModel> listVersionsModels = new List<VersionsModel>();
           
            if (result != null && result.Count() > 0)
            {
                foreach (Versions item in result)
                {
                    listVersionsModels.Add(mapper.Map<VersionsModel>(item));
                }
            }

            var finalResult= new ResponseResultList<VersionsModel>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = listVersionsModels
            };
            return Task.FromResult(finalResult);
        }


        /// <summary>
        /// Get Version By Id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public async Task<Versions> GetVersionById(int versionId)
        {
            return await base.context.Versions
                .Where(m => m.VersionId == versionId) 
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Version Modules By VersionId
        /// </summary>
        /// <param name = "versionId" > The versionId to get version modules</param>
        /// <returns></returns>
        public async Task<List<Modules>> GetVersionModulesByVersionId(int versionId)
        {
            return await base.context.VersionModules
                .Join(base.context.Modules, x => x.ModuleId, y => y.ModuleId, (x, y) => new { VersionModule  = x, Module = y })
                .Where(x => x.VersionModule.VersionId == versionId)
                .Select(x=> x.Module)
                .ToListAsync();
        }

        /// <summary>
        /// To Create Version
        /// </summary>
        /// <param name="version">New Version Object</param>
        /// <returns></returns>
        public async Task<VersionsModel> CreateVersion(VersionsModel version, int userId)
        {
            Versions newVersion = mapper.Map<Versions>(version);
            newVersion.CreatedOn = DateTime.UtcNow;
            newVersion.CreatedBy = userId;
            base.context.Versions.Add(newVersion);
            await base.context.SaveChangesAsync();
            return mapper.Map<VersionsModel>(newVersion);
        }

        /// <summary>
        /// To Update Version
        /// </summary>
        /// <param name="version">new version object</param>
        /// <returns></returns>
        public async Task<VersionsModel> UpdateVersion(long versionId, VersionsModel version, int userId)
        {
            var versionNew = base.context.Versions.Where(x=>x.VersionId == versionId).FirstOrDefault();
            if (versionNew != null)
            {
                versionNew.VersionCode = version.VersionCode;
                versionNew.DisplayName = version.DisplayName;
                versionNew.Description = version.Description;
                versionNew.Status = version.Status;
                versionNew.UpdatedOn = DateTime.UtcNow;
                versionNew.UpdatedBy = userId;
                base.context.Versions.Update(versionNew);
                await base.context.SaveChangesAsync();
                return mapper.Map<VersionsModel>(versionNew);
            }
            return mapper.Map<VersionsModel>(versionNew);
        }

        /// <summary>
        /// To Update Version Partially
        /// </summary>
        /// /// <param name="versionId">Version Id</param>
        /// <param name="version">New version object</param>
        /// <returns></returns>
        public async Task<VersionsModel> UpdatePartialVersion(long versionId, VersionsModel version, int userId)
        {
            var versionNew = await base.context.Versions.Where(x => x.VersionId == versionId).FirstOrDefaultAsync();
            if (versionNew != null)
            {
                versionNew.VersionCode = version.VersionCode;
                versionNew.DisplayName = version.DisplayName;
                versionNew.Description = version.Description;
                versionNew.Status = version.Status;
                versionNew.UpdatedOn = DateTime.UtcNow;
                versionNew.UpdatedBy = userId;
                base.context.Versions.Update(versionNew);
                base.context.SaveChanges();
                return mapper.Map<VersionsModel>(versionNew);
            }
            return mapper.Map<VersionsModel>(versionNew);
        }

        /// <summary>
        /// To Delete Version
        /// </summary>
        /// <param name="versionId">The versionId to delete </param>
        /// <returns></returns>
        public async Task<long> DeleteVersion(long versionId)
        {
            long result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var version = await base.context.Versions.FirstOrDefaultAsync(x => x.VersionId == versionId);

                if (version != null)
                {
                    //Delete that post
                    base.context.Versions.Remove(version);

                    //Commit the transaction
                    result = await base.context.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }














        /// <summary>
        /// Get All Versions
        /// </summary>
        /// <returns></returns>
        //public async Task<IEnumerable<DATAMODEL.Versions>> GetAllVersions()
        //{
        //    var result = base.context.Versions.ToList();
        //    return result;
        //}

        /// <summary>
        /// Get Version Modules By VersionId
        /// </summary>
        /// <param name="VersionId">The Version Id to get Version Modules</param>
        /// <returns></returns>
        //public async Task<IEnumerable<Model.Versions>> GetVersionModulesByVersionId(int VersionId = 0)
        //{
        //    var versionInfo = from version in base.context.Versions
        //                 join versionmodule in base.context.VersionModules on version.VersionId equals versionmodule.VersionId
        //                 join modules in base.context.Modules on versionmodule.ModuleId equals modules.ModuleId
        //                 where version.VersionId == (VersionId == 0 ? version.VersionId : VersionId)
        //                 select new Model.Versions
        //                 {
        //                     VersionId = version.VersionId,
        //                     VersionName = version.DisplayName,
        //                     ModuleId = versionmodule.ModuleId,
        //                     VersionModuleId = versionmodule.VersionModuleId,
        //                     ParentModuleId = modules.ParentModuleId,
        //                     ModuleName = modules.Name,
        //                     ModuleDescription = modules.Description,
        //                     IsNavigationId = modules.IsNavigationItem
        //                 };
        //    return versionInfo;
        //}
        /// <summary>
        /// To get all versions with all features(Modules, Sub Modules, and Permissions)
        /// </summary>
        /// <returns></returns>
        //public async Task<IEnumerable<Model.Versions>>GetVersions()
        //{
        //    var versionInfo = from version in base.context.Versions
        //                 join versionmodule in base.context.VersionModules on version.VersionId equals versionmodule.VersionId
        //                 join modules in base.context.Modules on versionmodule.ModuleId equals modules.ModuleId
        //                 orderby modules.ModuleId
        //                 select new Model.Versions
        //                 {
        //                     VersionId = version.VersionId,
        //                     VersionName = version.DisplayName,
        //                     ModuleId = versionmodule.ModuleId,
        //                     VersionModuleId = versionmodule.VersionModuleId,
        //                     ParentModuleId = modules.ParentModuleId,
        //                     ModuleName = modules.Name,
        //                     ModuleDescription = modules.Description,
        //                     IsNavigationId = modules.IsNavigationItem
        //                 };
        //    return versionInfo;
        //}

        /// <summary>
        ///  To get version details with all features(Modules, Sub Modules, and Permissions) for given Version Id
        /// </summary>
        /// <param name="VersionId">Version Id to get version details</param>
        /// <returns></returns>
        //public async Task<IEnumerable<Model.Versions>> GetVersionByVersionId(int VersionId)
        //{
        //    var versionInfo = from version in base.context.Versions
        //                 join versionmodule in base.context.VersionModules on version.VersionId equals versionmodule.VersionId
        //                 join modules in base.context.Modules on versionmodule.ModuleId equals modules.ModuleId
        //                 where version.VersionId == VersionId
        //                 orderby modules.ModuleId
        //                 select new Model.Versions
        //                 {
        //                     VersionId = version.VersionId,
        //                     VersionName = version.DisplayName,
        //                     ModuleId = versionmodule.ModuleId,
        //                     VersionModuleId = versionmodule.VersionModuleId,
        //                     ParentModuleId = modules.ParentModuleId,
        //                     ModuleName = modules.Name,
        //                     ModuleDescription = modules.Description,
        //                     IsNavigationId = modules.IsNavigationItem
        //                 };
        //    return versionInfo;

        //}
        /// <summary>
        /// Get All Modules
        /// </summary>
        /// <returns></returns>
        //public async Task<IEnumerable<Modules>> GetAllModules()
        //{
        //        var result = base.context.Modules.ToList();
        //        return result;          
        //}

        /// <summary>
        /// Get All Version Modules
        /// </summary>
        /// <param name="VersionId">The VersionId to get Version</param>
        /// <returns></returns>
        //public async Task<IEnumerable<VersionModules>> GetAllVersionModules(int VersionId)
        //{
        //        var result = base.context.VersionModules.Where(x => x.VersionId == VersionId).ToList();
        //        return result;           
        //}

        /// <summary>
        /// To Create Version
        /// </summary>
        /// <param name="versionModel">Version Model to create Version</param>
        /// <returns></returns>
        //public async Task<DATAMODEL.Versions> CreateVersion(DATAMODEL.Versions versionModel)
        //{
        //    base.context.Versions.Add(versionModel);
        //    return versionModel;
        //}

        /// <summary>
        /// To Update Version
        /// </summary>
        /// <param name="version">The new version object</param>
        /// <returns></returns>
        //public async Task<DATAMODEL.Versions> UpdateVersion(DATAMODEL.Versions version)
        //{
        //    base.context.Versions.Update(version);
        //    return version;
        //}

        /// <summary>
        /// To Delete Version
        /// </summary>
        /// <param name="versionId">VersionId to delete Version</param>
        /// <returns></returns>
        //public async Task<int> DeleteVersion(int versionId)
        //{
        //    return 1;            
        //}
    }
}
