using AutoMapper;
using Common.Model;
using Sample.Admin.Service.Infrastructure.DataModels;
using Sample.Admin.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Admin.Service.Infrastructure.Repository
{
    public class VersionModuleRepository : RepositoryBase<VersionModules>, IVersionModuleRepository
    {
        public IConfiguration configuration { get; }
        private readonly IMapper mapper;
        public VersionModuleRepository(CloudAcceleratorContext context, IConfiguration configuration, IMapper mapper) : base(context)
        {
            this.configuration = configuration;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get All Version Modules
        /// </summary>
        /// <returns></returns>
        public Task<ResponseResultList> GetAllVersionModules(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            IQueryable<VersionModules> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("/versionModules/");
            StringBuilder sbPrevious = new StringBuilder("/versionModules/");

            result = from versionmodules in base.context.VersionModules
                     select versionmodules;

            listCount = result.Count();

            if (!all)
            {
                var rowIndex = 0;
                if (pageNumber > 0)
                {
                    rowIndex = (pageNumber - 1) * pageSize;
                    sbNext.Append("?PageNumber=" + (pageNumber + 1) + "&PageSize=" + pageSize);
                    sbPrevious.Append("?PageNumber=" + (pageNumber - 1) + "&PageSize=" + pageSize);
                }
                else if (offset > 0)
                {
                    rowIndex = offset;
                    sbNext.Append("?PageNumber=" + (offset + pageSize) + "&PageSize=" + pageSize);
                    sbPrevious.Append("?PageNumber=" + (offset - pageSize) + "&PageSize=" + pageSize);
                }
                else
                {
                    rowIndex = 1;
                    sbNext.Append("PageNumber=" + (rowIndex + 1) + "&PageSize=" + pageSize);
                    sbPrevious.Append("PageNumber=" + (rowIndex - 1) + "&PageSize=" + pageSize);
                }

                result = base.context.VersionModules.Skip(rowIndex).Take(pageSize);
            }
            else
            {
                sbNext.Append("?all=" + all);
                sbPrevious.Append("?all=" + all);
            }

            if (!string.IsNullOrWhiteSpace(ordering))
            {
                ordering = string.Concat(ordering[0].ToString().ToUpper(), ordering.AsSpan(1));
                if (typeof(VersionModules).GetProperty(ordering) != null)
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

            List<VersionModulesModel> listVersionModulesModels = new List<VersionModulesModel>();
            if (result != null && result.Count() > 0)
            {
                foreach (VersionModules item in result)
                {
                    listVersionModulesModels.Add(mapper.Map<VersionModulesModel>(item));
                }
            }

            var finalResult = new ResponseResultList
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = listVersionModulesModels,
            };
            return Task.FromResult(finalResult);
        }

        public async Task<VersionModulesModel> GetVersionModuleById(long versionModuleId)
        {
            var result = await base.context.VersionModules.Where(m => m.VersionModuleId == versionModuleId).FirstOrDefaultAsync();
            return mapper.Map<VersionModulesModel>(result);
        }

        /// <summary>
        /// Get Version  Modules By VersionId
        /// </summary>
        /// <param name="versionId">The VersionId to get version modules </param>
        /// <returns></returns>
        public async Task<IEnumerable<VersionModules>> GetVersionModulesByVersionId(int versionId)
        {

            var result = from versionmodules in base.context.VersionModules
                         where versionmodules.VersionId == versionId
                         select new VersionModules
                         {
                             VersionModuleId = versionmodules.VersionModuleId,
                             ModuleId = versionmodules.ModuleId,
                             VersionId = versionmodules.VersionId
                         };
            return result;
        }

        /// <summary>
        /// To Create version Module
        /// </summary>
        /// <param name="versionModule">The VersionModule object</param>
        /// <returns></returns>
        public async Task<VersionModulesModel> CreateVersionModule(VersionModulesModel versionModule)
        {
            VersionModules newVersionModule = mapper.Map<VersionModules>(versionModule);
            newVersionModule.CreatedOn = DateTime.UtcNow;
            newVersionModule.CreatedBy = 1;
            base.context.VersionModules.Add(newVersionModule);
            await base.context.SaveChangesAsync();
            return mapper.Map<VersionModulesModel>(newVersionModule);
        }

        public async Task<VersionModulesModel> UpdateVersionModule(long versionModuleId, VersionModulesModel versionModules)
        {
            var versionModuleNew = await base.context.VersionModules.FindAsync(versionModuleId);
            if (versionModuleNew != null)
            {
                //this.mapper.Map<Modules>(moduleNew);
                versionModuleNew.VersionId = versionModules.VersionId;
                versionModuleNew.ModuleId = versionModules.ModuleId;

                await base.context.SaveChangesAsync();
                return mapper.Map<VersionModulesModel>(versionModuleNew);
            }
            return mapper.Map<VersionModulesModel>(versionModuleNew);

        }

        public async Task<VersionModulesModel> UpdatePartialVersionModule(long versionModuleId, VersionModulesModel versionModules)
        {
            var versionModuleNew = await base.context.VersionModules.FindAsync(versionModuleId);
            if (versionModuleNew != null)
            {
                //this.mapper.Map<Modules>(moduleNew);
                versionModuleNew.VersionId = versionModules.VersionId;
                versionModuleNew.ModuleId = versionModules.ModuleId;

                await base.context.SaveChangesAsync();
                return mapper.Map<VersionModulesModel>(versionModuleNew);
            }
            return mapper.Map<VersionModulesModel>(versionModuleNew);

        }

        /// <summary>
        /// To Delete Versions Module
        /// </summary>
        /// <param name="versionModuleId">The versionModuleId to delete versionmodule</param>
        /// <returns></returns>
        public async Task<long> DeleteVersionModules(long versionModuleId)
        {
            long result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var versionModule = await base.context.VersionModules.FirstOrDefaultAsync(x => x.VersionModuleId == versionModuleId);

                if (versionModule != null)
                {
                    //Delete that post
                    base.context.VersionModules.Remove(versionModule);

                    //Commit the transaction
                    result = await base.context.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<long> DeleteVersionModulesByVersionId(long versionId)
        {
            long result = 0;
            if (base.context != null)
            {
                var versionModules = base.context.VersionModules.Where(x => x.VersionId == versionId).ToList(); ;
                if (versionModules != null && versionModules.Count > 0)
                {
                    base.context.VersionModules.RemoveRange(versionModules);
                    result = await base.context.SaveChangesAsync();
                }
            }
            return result;
        }

        
    }
}
