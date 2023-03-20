using Common.Model;
using Sample.Customer.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Sample.Customer.HttpAggregator.IServices.FormsMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace Sample.Customer.HttpAggregator.Controllers.FormsMaster
{
    /// <summary>
    /// FormMasterConfigurationController
    /// </summary>
    [Route("api/FormMasterConfiguration")]
    [ApiController]
    public class FormsMasterConfigurationController : BaseApiController
    {
        private readonly IFormMasterService formsMasterService;
        /// <summary>
        /// FormMasterConfigurationController
        /// </summary>
        /// <param name="formsMasterService"></param>
        /// <param name="logger"></param>
        public FormsMasterConfigurationController(IFormMasterService formsMasterService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(formsMasterService), formsMasterService);
            this.formsMasterService = formsMasterService;
        }


        /// <summary>
        /// To Delete Form Data
        /// </summary>
        /// <param name="objFormsMasterConfigurationRoot"> Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("SaveFormMasterConfigurationData")]
        [HttpPost]
        public async Task<IActionResult> SaveFormMasterConfigurationData([FromBody] FormsMasterConfigurationRoot objFormsMasterConfigurationRoot)
        {
            Check.Argument.IsNotNull(nameof(objFormsMasterConfigurationRoot), objFormsMasterConfigurationRoot);
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.SaveFormMasterConfigurationData(objFormsMasterConfigurationRoot);
                if (result == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError
                    });
                }
                return Ok(result);

            });
        }
        /// <summary>
        /// To Delete Form Data
        /// </summary>
        /// <param name="dataSourceID"> Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("GetFormMasterConfigurationData/{dataSourceID}")]
        [HttpGet]
        public async Task<IActionResult> GetFormMasterConfigurationData(int dataSourceID)
        {
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetFormMasterConfigurationData(dataSourceID);
                if (result == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError
                    });
                }
                return Ok(result);

            });
        }

        /// <summary>
        /// To Delete Form Data
        /// </summary>
        /// <param name="dataSourceID"> Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("GetListDataSourceConfigurationData/{dataSourceID}")]
        [HttpGet]
        public async Task<IActionResult> GetListDataSourceConfigurationData(int dataSourceID)
        {
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetListDataSourceConfigurationData(dataSourceID);
                if (result == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError
                    });
                }
                return Ok(result);

            });
        }


        /// <summary>
        /// Get All table list from IAM schema for Data Source
        /// </summary>
        /// <returns></returns>
        [Route("GetAllDataSourceTables")]
        [ProducesResponseType(typeof(ResponseResult<List<string>>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAllDataSourceTables()
        {
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetAllDataSourceTables();
               
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);

            });
        }

        /// <summary>
        /// Save table name into DataSource
        /// </summary>
        /// <param name="tableList">List of table Names</param>
        /// <returns></returns>
        [Route("SaveAllDataSourceTables")]
        [ProducesResponseType(typeof(ResponseResult<string>), 200)]
        [HttpPost]
        public async Task<IActionResult> SaveAllDataSourceTables([FromBody] string[] tableList)
        {
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.SaveAllDataSourceTables(tableList);

                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);

            });
        }
    }
}
