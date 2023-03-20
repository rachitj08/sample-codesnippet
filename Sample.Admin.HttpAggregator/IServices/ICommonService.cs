using Common.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Sample.Admin.HttpAggregator.IServices
{
    /// <summary>
    /// Common Service
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// Get All Data
        /// </summary>
        /// <returns></returns>
        Task<ResponseResultList<T>> GetAll<T>(HttpContext httpContext, string ordering, int offset, int pageSize, int pageNumber, bool all);

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<T>> GetById<T>(HttpContext httpContext, long id);

        /// <summary>
        /// To Create New Record
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="model">model object</param>
        /// <returns></returns>
        Task<ResponseResult<T>> Create<T>(HttpContext httpContext, T model);

        /// <summary>
        /// To update existing Record
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="model">model object</param>
        /// <param name="id">unique Id</param>
        /// <returns></returns>
        Task<ResponseResult<T>> Update<T>(HttpContext httpContext, long id, T model);


        ///// <summary>
        ///// To update existing record 
        ///// </summary>
        ///// <param name="httpContext"></param>
        ///// <param name="model">model object</param>
        ///// <param name="id">unique Id</param>
        ///// <returns></returns>
        //Task<ResponseResult<T>> UpdatePartial<T>(HttpContext httpContext, long id, T model);


        /// <summary>
        /// To delete existing record
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="id">id identifier</param>
        /// <returns></returns>
        Task<ResponseResult<bool>> Delete(HttpContext httpContext, long id);
    }
}
