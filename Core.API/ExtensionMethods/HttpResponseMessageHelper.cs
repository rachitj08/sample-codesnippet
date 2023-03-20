using Common.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Core.API.ExtensionMethods
{
    public static class HttpResponseMessageHelper
    {
        ///// <summary>
        ///// Get Response From HttpResponseMessage and return simple object
        ///// </summary>
        ///// <param name="httpResponse">Http Response</param>
        ///// <returns>
        ///// Return value with simple object of T type. For Example
        ///// AccountModel value = httpResponse.GetResponse<AccountModel>();
        ///// </returns> 
        //public static T GetResponse<T>(this HttpResponseMessage httpResponse)
        //{
        //    var result = httpResponse.GetHttpResponse<T>();
        //    return result.value;
        //}

        /// <summary>
        /// Get Response From HttpResponseMessage and return value into ResponseResult
        /// </summary>
        /// <param name="httpResponse">Http Response</param>
        /// <returns>
        /// Return value with ResponseResult<T>. For Example
        /// ResponseResult<AccountModel> value = httpResponse.GetResponseResult<AccountModel>();
        /// </returns> 
        public static ResponseResult<T> GetResponseResult<T>(this HttpResponseMessage httpResponse, string defaultResponseMessage = ResponseMessage.InternalServerError, string defaultResponseCode = ResponseCode.InternalServerError)
        {
            var result = httpResponse.GetHttpResponse<ResponseResult<T>>();
            if (!result.GetResponse || result.Value == null)
            {
                return new ResponseResult<T>()
                {
                    Message = defaultResponseMessage,
                    ResponseCode = defaultResponseCode,
                    Error = new ErrorResponseResult()
                    {
                        Message = defaultResponseMessage
                    },
                };
            }
            
            return result.Value;
        }

        /// <summary>
        /// Get Response From HttpResponseMessage and return value into ResponseResult
        /// </summary>
        /// <param name="httpResponse">Http Response</param>
        /// <returns>
        /// Return value with ResponseResult<T>. For Example
        /// ResponseResult<AccountModel> value = httpResponse.GetResponseResult<AccountModel>();
        /// </returns> 
        public static ResponseResultList<T> GetResponseResultList<T>(this HttpResponseMessage httpResponse, string basePath, string defaultResponseMessage = ResponseMessage.InternalServerError, string defaultResponseCode = ResponseCode.InternalServerError)
        {
            var result = httpResponse.GetHttpResponse<ResponseResultList<T>>();
            if (!result.GetResponse)
            {
                return new ResponseResultList<T>()
                {
                    Message = defaultResponseMessage,
                    ResponseCode = defaultResponseCode,
                    Error = new ErrorResponseResult()
                    {
                        Message = defaultResponseMessage
                    },
                };
            }

            if (result.Value == null)
            {
                return new ResponseResultList<T>()
                {
                    Message = ResponseMessage.NoRecordFound,
                    ResponseCode = ResponseCode.NoRecordFound,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    },
                };
            }

            var detail = result.Value;
            if (!string.IsNullOrEmpty(detail.Next)) detail.Next = basePath + detail.Next;
            if (!string.IsNullOrEmpty(detail.Previous)) detail.Previous = basePath + detail.Previous;
            detail.Message = ResponseMessage.RecordFetched;
            detail.ResponseCode = ResponseCode.RecordFetched;
            return result.Value;
        }


        /// <summary>
        /// Get Response From HttpResponseMessage and return value into ResponseResult
        /// </summary>
        /// <param name="httpResponse">Http Response</param>
        /// <returns>
        /// Return value with ResponseResult<T> with ResponseCode = ResponseCode.RecordFetched. For Example
        /// var value = httpResponse.GetFetchRecords<AccountModel>();
        /// return new ResponseResult<T>() {  Message = ResponseMessage.RecordFetched, ResponseCode = ResponseCode.RecordFetched, Data = value };
        /// </returns> 
        public static ResponseResult<T> GetFetchRecords<T>(this HttpResponseMessage httpResponse, string defaultResponseMessage = ResponseMessage.InternalServerError, string defaultResponseCode = ResponseCode.InternalServerError)
        {
            var result = httpResponse.GetHttpResponse<T>();

            if (!result.GetResponse || result.Value == null)
            {
                return new ResponseResult<T>()
                {
                    Message = defaultResponseMessage,
                    ResponseCode = defaultResponseCode,
                    Error = new ErrorResponseResult()
                    {
                        Message = defaultResponseMessage
                    },
                };
            }

            return new ResponseResult<T>()
            {
                Message = ResponseMessage.RecordFetched,
                ResponseCode = ResponseCode.RecordFetched,
                Data = result.Value
            };
        } 

        public static (bool GetResponse, T Value) GetHttpResponse<T>(this HttpResponseMessage httpResponse)
        {
            Console.WriteLine(httpResponse.Content.ReadAsStringAsync().Result);
            if (httpResponse == null || !httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                return (false, default(T));
            }

            var result = JsonConvert.DeserializeObject<T>(httpResponse.Content.ReadAsStringAsync().Result);
            return (true, result);
        }
    }
}
