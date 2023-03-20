using Common.Model;
using Sample.Customer.HttpAggregator.IServices.Services.CommonUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator.Services.Services.CommonUtility
{
    /// <summary>
    /// Service Common Utility
    /// </summary>
    public class ServiceCommonUtility : IServiceCommonUtility
    {

        /// <summary>
        /// ManageServiceClientResponse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objHttpResponseMessage"></param>
        /// <param name="serviceOperationType"></param>
        /// <returns></returns>
        public ResponseResult<T> ManageServiceClientResponse<T>(HttpResponseMessage objHttpResponseMessage, string serviceOperationType)
        {
            var responseResult = new ResponseResult<T>();
            if (objHttpResponseMessage == null || objHttpResponseMessage.Content == null)
            {
                responseResult.Message = ResponseMessage.HttpResponseNull;
                responseResult.ResponseCode = ResponseCode.HttpResponseNull;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }
            if (!objHttpResponseMessage.IsSuccessStatusCode)
            {
                responseResult.Message = ResponseMessage.HttpResponseNull;
                responseResult.ResponseCode = ResponseCode.HttpResponseNull;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };

                if (objHttpResponseMessage.Content != null)
                {
                    LogResponseView logErrorResponse = JsonConvert.DeserializeObject<LogResponseView>(objHttpResponseMessage.Content.ReadAsStringAsync().Result);
                    if (logErrorResponse != null)
                    {
                        responseResult.Message = logErrorResponse.Type;
                        responseResult.ResponseCode = logErrorResponse.Message;
                        responseResult.Error = new ErrorResponseResult()
                        {
                            Message = logErrorResponse.Message
                        };
                        return responseResult;
                    }
                }
                return responseResult;
            }

            if (objHttpResponseMessage.Content != null)
            {
                var httpResponse = JsonConvert.DeserializeObject<T>(objHttpResponseMessage.Content.ReadAsStringAsync().Result);
                if (httpResponse != null)
                {
                    if (!string.IsNullOrWhiteSpace(serviceOperationType))
                    {
                        if (serviceOperationType == "add" || serviceOperationType == "update")
                        {
                            responseResult.Message = ResponseMessage.RecordSaved;
                            responseResult.ResponseCode = ResponseCode.RecordSaved;
                        }
                        if (serviceOperationType == "edit" || serviceOperationType == "list")
                        {
                            responseResult.Message = ResponseMessage.RecordFetched;
                            responseResult.ResponseCode = ResponseCode.RecordFetched;
                        }

                        if (serviceOperationType == "delete")
                        {
                            responseResult.Message = ResponseMessage.RecordDeleted;
                            responseResult.ResponseCode = ResponseCode.RecordDeleted;
                        }

                    }
                    responseResult.Data = httpResponse;
                }
                else
                {
                    responseResult.Message = ResponseMessage.NoRecordFound;
                    responseResult.ResponseCode = ResponseCode.NoRecordFound;
                    responseResult.Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    };
                    return responseResult;
                }

            }
            return responseResult;
        }

        /// <summary>
        /// ManageServiceClientResponse
        /// </summary>
        /// <param name="objHttpResponseMessage"></param>
        /// <param name="serviceOperationType"></param>
        /// <returns></returns>
        public ResponseResult ManageServiceClientResponse(HttpResponseMessage objHttpResponseMessage, string serviceOperationType)
        {
            var responseResult = new ResponseResult();
            if (objHttpResponseMessage == null || objHttpResponseMessage.Content == null)
            {
                responseResult.Message = ResponseMessage.HttpResponseNull;
                responseResult.ResponseCode = ResponseCode.HttpResponseNull;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };
                return responseResult;
            }
            if (!objHttpResponseMessage.IsSuccessStatusCode)
            {
                responseResult.Message = ResponseMessage.HttpResponseNull;
                responseResult.ResponseCode = ResponseCode.HttpResponseNull;
                responseResult.Error = new ErrorResponseResult()
                {
                    Message = ResponseMessage.InternalServerError
                };

                if (objHttpResponseMessage.Content != null)
                {
                    LogResponseView logErrorResponse = JsonConvert.DeserializeObject<LogResponseView>(objHttpResponseMessage.Content.ReadAsStringAsync().Result);
                    if (logErrorResponse != null)
                    {
                        responseResult.Message = logErrorResponse.Type;
                        responseResult.ResponseCode = logErrorResponse.Message;
                        responseResult.Error = new ErrorResponseResult()
                        {
                            Message = logErrorResponse.Message
                        };
                        return responseResult;
                    }
                }
                return responseResult;
            }

            if (objHttpResponseMessage.Content != null)
            {
                var httpResponse = JsonConvert.DeserializeObject<ResponseResult>(objHttpResponseMessage.Content.ReadAsStringAsync().Result);
                if (httpResponse != null)
                {
                    if (!string.IsNullOrWhiteSpace(serviceOperationType))
                    {
                        if (serviceOperationType == "add" || serviceOperationType == "update")
                        {
                            responseResult.Message = ResponseMessage.RecordSaved;
                            responseResult.ResponseCode = ResponseCode.RecordSaved;
                        }
                        if (serviceOperationType == "edit" || serviceOperationType == "list")
                        {
                            responseResult.Message = ResponseMessage.RecordFetched;
                            responseResult.ResponseCode = ResponseCode.RecordFetched;
                        }

                        if (serviceOperationType == "delete")
                        {
                            responseResult.Message = ResponseMessage.RecordDeleted;
                            responseResult.ResponseCode = ResponseCode.RecordDeleted;
                        }

                    }
                    responseResult.Data = httpResponse;
                }
                else
                {
                    responseResult.Message = ResponseMessage.NoRecordFound;
                    responseResult.ResponseCode = ResponseCode.NoRecordFound;
                    responseResult.Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.NoRecordFound
                    };
                    return responseResult;
                }

            }
            return responseResult;
        }
    }
}
