using Common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.DataModels;

namespace Sample.Customer.Service.Infrastructure.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class AgreementTypeRepository : RepositoryBase<AgreementType>, IAgreementTypeRepository
    {
        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration configuration { get; }

        public AgreementTypeRepository(CloudAcceleratorContext context, IConfiguration configuration) : base(context)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Get All Records
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<AgreementTypeVM>> GetAllRecords(string ordering, int offset, int pageSize, int pageNumber, bool all)
        {
            IQueryable<AgreementType> result = null;
            int listCount;
            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
            StringBuilder sbNext = new StringBuilder("");
            StringBuilder sbPrevious = new StringBuilder("");

            result = (from g in base.context.AgreementType select g);

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
                if (typeof(AgreementType).GetProperty(ordering) != null)
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
                result = result.OrderByDescending(x => x.AgreementTypeId);
            }

            var finalRecordList = new List<AgreementTypeVM>();
            if (result != null && result.Count() > 0)
            { 
                finalRecordList = await result.Select(x => new AgreementTypeVM()
                {
                    AgreementTypeId = x.AgreementTypeId,
                    Name = x.Name
                }).ToListAsync();
            }

            return new ResponseResultList<AgreementTypeVM>
            {
                ResponseCode = ResponseCode.RecordFetched,
                Message = ResponseMessage.RecordFetched,
                Count = listCount,
                Next = sbNext.ToString(),
                Previous = sbPrevious.ToString(),
                Data = finalRecordList,
            };
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <returns></returns> 
        public async Task<AgreementType> GetRecordById(long id)
        {
            return await base.context.AgreementType.Where(x=>x.AgreementTypeId == id).FirstOrDefaultAsync();  
        }

        /// <summary>
        /// To Create Record
        /// </summary>
        /// <param name="model">New Object</param>
        /// <returns></returns>
        public async Task<AgreementType> CreateRecord(AgreementType model)
        { 
            base.context.AgreementType.Add(model);
            await base.context.SaveChangesAsync();  
            return model;
        }

        /// <summary>
        /// To Update Record
        /// </summary>
        /// <param name="id">Id to Record </param>
        /// <param name="model">Data model object</param>
        /// <returns></returns>
        public async Task<int> UpdateRecord(long id, AgreementType model)
        {
            var record = await base.context.AgreementType.FindAsync(id);

            if (record == null) return 0;

            record.Name = model.Name;
            record.UpdatedOn = model.UpdatedOn;
            record.UpdatedBy = model.UpdatedBy;

            return await base.context.SaveChangesAsync();
        }

        /// <summary>
        /// To Delete Record
        /// </summary>
        /// <param name="id">Id to Record </param>
        /// <returns></returns>
        public async Task<int> DeleteRecord(long id)
        { 
            //Find the post for specific post id
            var record = await base.context.AgreementType.FirstOrDefaultAsync(x => x.AgreementTypeId == id);
            if (record == null) return 0;
            base.context.AgreementType.Remove(record);
            return await base.context.SaveChangesAsync();
        }
    }
}
