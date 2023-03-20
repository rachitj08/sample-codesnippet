//using Common.Model;
//using Sample.Admin.Service.Infrastructure.DataModels;
//using Sample.Admin.Model;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sample.Admin.Service.Infrastructure.Repository
//{
//    public class CurrencyRepository : RepositoryBase<Currencies>, ICurrencyRepository
//    {
//        /// <summary>
//        /// configuration variable
//        /// </summary>
//        public IConfiguration configuration { get; }

//        public CurrencyRepository(CloudAcceleratorContext context, IConfiguration configuration) : base(context)
//        {
//            this.configuration = configuration;
//        }

//        /// <summary>
//        /// To Get All Currencies
//        /// </summary>
//        /// <param name="search">Search Fields: (Code, DisplayName)</param>
//        /// <param name="offset">The initial index from which to return the results.</param>
//        /// <param name="ordering">Ordering Fields: (All Fields)</param>
//        /// <param name="pageSize">Number of results to return per page</param>
//        /// <param name="pageNumber">Page Number of results.</param>
//        /// <param name="all">It will return results (all pages) without pagination.</param>
//        /// <returns></returns>
//        public async Task<ResponseResultList<CurrencyModel>> GetAllCurrencies(int pageSize, int pageNumber, string ordering, string search, int offset, bool all)
//        {
//            IQueryable<Currencies> result = null;
//            int listCount;
//            if (pageSize < 1) pageSize = configuration.GetValue("PageSize", 20);
//            StringBuilder sbNext = new StringBuilder("");
//            StringBuilder sbPrevious = new StringBuilder("");

//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                string[] searchList = search.Split(",");

//                foreach (string item in searchList)
//                {
//                    Int64.TryParse(item, out long id);

//                    if(result == null)
//                    {
//                        result = from currency in base.context.Currencies
//                                 where currency.Code.ToLower().Contains(item.ToLower()) || currency.DisplayName.ToLower().Contains(item.ToLower())
//                                 select currency;
//                    }
//                    else
//                    {
//                        result = result.Concat(from currency in base.context.Currencies
//                                               where currency.Code.ToLower().Contains(item.ToLower()) || currency.DisplayName.ToLower().Contains(item.ToLower())
//                                               select currency);
//                    }
//                }
//            }
//            else
//            {
//                result = from services in base.context.Currencies
//                         select services;
//            }

//            if (!all)
//            {
//                listCount = result.Count();

//                var rowIndex = 0;

//                if (pageNumber > 0)
//                {
//                    rowIndex = (pageNumber - 1) * pageSize;
//                    if (((pageNumber + 1) * pageSize) <= listCount)
//                        sbNext.Append("pageNumber=" + (pageNumber + 1) + "&pageSize=" + pageSize);

//                    if (pageNumber > 1)
//                        sbPrevious.Append("pageNumber=" + (pageNumber - 1) + "&pageSize=" + pageSize);
//                }
//                else if (offset > 0)
//                {
//                    rowIndex = offset;

//                    if ((offset + pageSize + 1) <= listCount)
//                        sbNext.Append("offset=" + (offset + pageSize) + "&pageSize=" + pageSize);

//                    if ((offset - pageSize) > 0)
//                        sbPrevious.Append("offset=" + (offset - pageSize) + "&pageSize=" + pageSize);
//                }
//                else
//                {
//                    if (pageSize < listCount)
//                        sbNext.Append("pageNumber=" + (rowIndex + 1) + "&pageSize=" + pageSize);
//                }

//                result = result.Skip(rowIndex).Take(pageSize);
//            }
//            else
//            {
//                listCount = result.Count();
//                sbNext.Append("all=" + all);
//                sbPrevious.Append("all=" + all);
//            }

//            if (!string.IsNullOrWhiteSpace(ordering))
//            {
//                ordering = string.Concat(ordering[0].ToString().ToUpper(), ordering.AsSpan(1));
//                if (typeof(Currencies).GetProperty(ordering) != null)
//                {
//                    result = result.OrderBy(m => EF.Property<object>(m, ordering));
//                    if (!string.IsNullOrEmpty(sbNext.ToString()))
//                        sbNext.Append("&ordering=" + ordering);

//                    if (!string.IsNullOrEmpty(sbPrevious.ToString()))
//                        sbPrevious.Append("&ordering=" + ordering);
//                }
//            }
//            else
//            {
//                result = result.OrderByDescending(x => x.CurrencyId);
//            }

//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                if (!string.IsNullOrEmpty(sbNext.ToString()))
//                    sbNext.Append("&search=" + search);

//                if (!string.IsNullOrEmpty(sbPrevious.ToString()))
//                    sbPrevious.Append("&search=" + search);
//            }

//            var currencies = new List<CurrencyModel>();
//            if (result != null && result.Count() > 0)
//            {
//                currencies = await result.Select(x => new CurrencyModel()
//                {
//                    CurrencyId = x.CurrencyId,
//                    Code = x.Code,
//                    DisplayName = x.DisplayName,
//                    BaseCurrency = x.BaseCurrency,
//                    Description = x.Description
//                }).ToListAsync();
//            }


//            return new ResponseResultList<CurrencyModel>
//            {
//                ResponseCode = ResponseCode.RecordFetched,
//                Message = ResponseMessage.RecordFetched,
//                Count = listCount,
//                Next = sbNext.ToString(),
//                Previous = sbPrevious.ToString(),
//                Data = currencies
//            }; 
//        }

//        /// <summary>
//        /// Get Currency Detail
//        /// </summary>
//        /// <param name="currencyId">Currency Id</param>
//        /// <returns></returns>
//        public async Task<CurrencyModel> GetCurrencyDetail(int currencyId)
//        { 
//            return await base.context.Currencies
//                   .Where(x => x.CurrencyId == currencyId)
//                   .Select(x => new CurrencyModel()
//                   {
//                       CurrencyId = x.CurrencyId,
//                       Code = x.Code,
//                       DisplayName = x.DisplayName,
//                       BaseCurrency = x.BaseCurrency,
//                       Description = x.Description
//                   }).FirstOrDefaultAsync();
//        }

//    }
//}
