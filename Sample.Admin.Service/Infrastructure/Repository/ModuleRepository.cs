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
    public class ModuleRepository : RepositoryBase<Modules>, IModuleRepository
    {
        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration configuration { get; }

        private readonly IMapper mapper;

        public ModuleRepository(CloudAcceleratorContext context, IMapper mapper, IConfiguration configuration) : base(context)
        {
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public Task<List<Modules>> GetAllModules(bool isNavigationItem, long accountId, int serviceId)
        {
             var result = from m in base.context.Modules
                        join ss in base.context.Services on m.ServiceId equals ss.ServiceId
                        join vm in base.context.VersionModules on m.ModuleId equals vm.ModuleId
                        join s in base.context.Subscriptions on vm.VersionId equals s.VersionId
                        where !s.Cancelled && s.AccountId == accountId &&
                            (serviceId < 1 || ss.ServiceId == serviceId) &&
                            s.StartDate.Date <= DateTime.UtcNow.Date && s.EndDate.Date >= DateTime.UtcNow.Date
                        select m;

            if (isNavigationItem) 
                result = result.Where(x => x.IsNavigationItem == isNavigationItem);

            return result.Distinct().ToListAsync();
        }

        /// <summary>
        /// Get All Modules
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<ModulesModel>> GetAllModules(string ordering, string search, int offset, int pageSize, int pageNumber, string serviceName, bool all)
        {
            int serviceId = 0;
            IQueryable<Modules> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("");
            StringBuilder sbPrevious = new StringBuilder("");

            if(!string.IsNullOrEmpty(serviceName))
                serviceId = base.context.Services.Where(w => w.ServiceName.Contains(serviceName)).Select(x=>x.ServiceId).FirstOrDefault();


            if (!string.IsNullOrWhiteSpace(search))
            {
                string[] searchList = search.Split(",");

                foreach (string item in searchList)
                {
                    Int64.TryParse(item, out long moduleId);

                    if (result == null)
                    {
                        result = from modules in base.context.Modules
                                 where modules.ModuleId == moduleId || modules.Name.Contains(item) || modules.DisplayName.Contains(item)
                                 select modules;
                    }
                    else
                    {
                        result = result.Concat(from modules in base.context.Modules
                                               where modules.ModuleId == moduleId || modules.Name.Contains(item) || modules.DisplayName.Contains(item)
                                               select modules);
                    }
                }
            }
            else
            {
                result = from modules in base.context.Modules
                         select modules;
            }
            if (serviceId > 0)
                result = result.Where(x => x.ServiceId == serviceId);

            if (!all)
            {
                listCount = result.Count();

                var rowIndex = 0;

                if (pageNumber > 0)
                {
                    rowIndex = (pageNumber - 1) * pageSize;
                    if (((pageNumber + 1) * pageSize) <= listCount)
                        sbNext.Append("pageNumber=" + (pageNumber + 1) + "&pageSize=" + pageSize);

                    if (pageNumber > 1)
                        sbPrevious.Append("pageNumber=" + (pageNumber - 1) + "&pageSize=" + pageSize);
                }
                else if (offset > 0)
                {
                    rowIndex = offset;

                    if ((offset + pageSize + 1) <= listCount)
                        sbNext.Append("offset=" + (offset + pageSize) + "&pageSize=" + pageSize);

                    if ((offset - pageSize) > 0)
                        sbPrevious.Append("offset=" + (offset - pageSize) + "&pageSize=" + pageSize);
                }
                else
                {
                    if (pageSize < listCount)
                        sbNext.Append("pageNumber=" + (rowIndex + 1) + "&pageSize=" + pageSize);
                }

                result = result.Skip(rowIndex).Take(pageSize);
            }
            else
            {
                listCount = result.Count();
                sbNext.Append("all=" + all);
                sbPrevious.Append("all=" + all);
            }

            if (!string.IsNullOrWhiteSpace(ordering))
            {
                ordering = string.Concat(ordering[0].ToString().ToUpper(), ordering.AsSpan(1));
                if (typeof(Modules).GetProperty(ordering) != null)
                {
                    result = result.OrderBy(m => EF.Property<object>(m, ordering));
                    if (!string.IsNullOrEmpty(sbNext.ToString()))
                        sbNext.Append("&ordering=" + ordering);

                    if (!string.IsNullOrEmpty(sbPrevious.ToString()))
                        sbPrevious.Append("&ordering=" + ordering);
                }
            }
            else
            {
                result = result.OrderByDescending(x => x.ModuleId);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (!string.IsNullOrEmpty(sbNext.ToString()))
                    sbNext.Append("&search=" + search);

                if (!string.IsNullOrEmpty(sbPrevious.ToString()))
                    sbPrevious.Append("&search=" + search);
            }

            var modulesNew = new List<ModulesModel>();
            if (result != null && result.Count() > 0)
            {
                modulesNew = await result.Select(x => new ModulesModel()
                {
                    ModuleId = x.ModuleId,
                    IsNavigationItem = x.IsNavigationItem,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Description = x.Description,
                    Url = x.Url,
                    DisplayOrder = x.DisplayOrder,
                    IsVisible = x.IsVisible,
                    Status = x.Status,
                    ParentModule = x.ParentModuleId,
                    Service = x.ServiceId,
                }).ToListAsync();
            }


            return new ResponseResultList<ModulesModel>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = modulesNew
            };
        }

        /// <summary>
        /// Get Module By Id
        /// </summary>
        /// <returns></returns>
        public async Task<Modules> GetModuleById(long moduleId)
        {
            return await base.context.Modules.Where(m => m.ModuleId == moduleId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Sub Modules By ModuleId
        /// </summary>
        /// <param name="moduleId">The ModuleId to get sub module</param>
        /// <returns></returns>
        public async Task<IEnumerable<Modules>> GetSubModulesByModuleId(long moduleId)
        {
            return await base.context.Modules
                .Where(x => x.ParentModuleId == moduleId)
                .ToListAsync();
        }

        /// <summary>
        /// To Create Module
        /// </summary>
        /// <param name="module">New Module Object</param>
        /// <returns></returns>
        public async Task<long> CreateModule(Modules module)
        {
            base.context.Modules.Add(module);
            var result = await base.context.SaveChangesAsync();
            if(result > 0)
            {
                return module.ModuleId;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// To Update Module
        /// </summary>
        /// <param name="module">new module object</param>
        /// <returns></returns>
        public async Task<int> UpdateModule(Modules module)
        {
            base.context.Update(module);
            return await base.context.SaveChangesAsync(); 
        }
         

        /// <summary>
        /// To Delete Module
        /// </summary>
        /// <param name="moduleId">The moduleId to delete </param>
        /// <returns></returns>
        public async Task<long> DeleteModule(long moduleId)
        {
            long result = 0;
            if (base.context != null)
            {
                //Find the post for specific post id
                var module = await base.context.Modules.FirstOrDefaultAsync(x => x.ModuleId == moduleId);

                if (module != null)
                {
                    //Delete that post
                    base.context.Modules.Remove(module);

                    //Commit the transaction
                    result = await base.context.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<ModulesAllModel>> GetModulesForService(string serviceName)
        {
            return await base.context.Modules
                .Join(base.context.Services, x => x.ServiceId, y => y.ServiceId, (x, y) => new { Module = x, Service = y })
                .Where(x => (string.IsNullOrEmpty(serviceName) || (x.Service.ServiceName.ToLower() == serviceName.ToLower())))
                .Select(x => new ModulesAllModel()
                {
                    ModuleId = x.Module.ModuleId,
                    ModuleDescription = x.Module.Description,
                    DisplayOrder = x.Module.DisplayOrder,
                    ModuleDisplayName = x.Module.DisplayName,
                    IsNavigationItem = x.Module.IsNavigationItem,
                    IsVisible = x.Module.IsVisible,
                    ModuleName = x.Module.Name,
                    ModuleUrl = x.Module.Url,
                    ParentModuleId = x.Module.ParentModuleId,
                    ServiceName = x.Service.ServiceName
                })
                .ToListAsync();
        }


        public async Task<long> GetModuleByName(long accountId, int serviceId, string moduleName)
        {
            var result = from m in base.context.Modules
                         join ss in base.context.Services on m.ServiceId equals ss.ServiceId
                         join vm in base.context.VersionModules on m.ModuleId equals vm.ModuleId
                         join s in base.context.Subscriptions on vm.VersionId equals s.VersionId
                         where !s.Cancelled && s.AccountId == accountId &&
                            m.Name.ToLower() == moduleName.ToLower() &&
                            s.StartDate.Date <= DateTime.UtcNow.Date && s.EndDate.Date >= DateTime.UtcNow.Date &&
                            (serviceId < 0 || ss.ServiceId == serviceId)
                         select m.ModuleId;

            return await result.FirstOrDefaultAsync();
        }
        public async Task<List<Module>> GetModulesByAccountId(long accountId , bool isNavigationItem = true)
        {
            var query = from m in base.context.Modules
                         join ss in base.context.Services on m.ServiceId equals ss.ServiceId
                         join vm in base.context.VersionModules on m.ModuleId equals vm.ModuleId
                         join s in base.context.Subscriptions on vm.VersionId equals s.VersionId
                         where !s.Cancelled && s.AccountId == accountId &&
                            s.StartDate.Date <= DateTime.UtcNow.Date && s.EndDate.Date >= DateTime.UtcNow.Date
                         select new Module()
                         {
                             ModuleId = m.ModuleId,
                             Description = m.Description,
                             DisplayOrder = m.DisplayOrder,
                             DisplayName = m.DisplayName,
                             IsNavigationItem = m.IsNavigationItem,
                             IsVisible = m.IsVisible,
                             Name = m.Name,
                             Url = m.Url,
                             ParentModuleId = m.ParentModuleId,
                             ServiceId = m.ServiceId,
                             DataSourceId = m.DataSourceId
                         };

            if (isNavigationItem)
            {
                query = query.Where(x => x.IsNavigationItem == isNavigationItem);
            }
           

            return await query.Distinct().ToListAsync();
        }
        
    }
}
