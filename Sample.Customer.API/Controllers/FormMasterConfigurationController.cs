using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Service.ServiceWorker;
using Sample.Customer.Model;

namespace Sample.Customer.API.Controllers
{
    [Route("api/FormMasterConfiguration")]
    [ApiController]
    public class FormMasterConfigurationController : BaseApiController
    {
        private readonly IFormMasterService formsMasterService;
        /// <summary>
        /// FormMasterConfigurationController
        /// </summary>
        /// <param name="formsMasterService"></param>
        /// <param name="logger"></param>
        public FormMasterConfigurationController(IFormMasterService formsMasterService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(formsMasterService), formsMasterService);
            this.formsMasterService = formsMasterService;
        }

        /// <summary>
        /// To Delete Form Data
        /// </summary>
        /// <param name="jsonObj"> Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("SaveFormMasterConfigurationData")]
        [HttpPost]
        public async Task<IActionResult> SaveFormMasterConfigurationData([FromBody] FormsMasterConfigurationRoot objRoot)
        {
            Check.Argument.IsNotNull(nameof(objRoot), objRoot);
            return await Execute(async () =>
            {
                if (objRoot == null || objRoot.data == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError,
                        ResponseCode = ResponseCode.InternalServerError
                    });
                }
                objRoot.data.AccountId = base.loggedInAccountId;
                var result = await this.formsMasterService.SaveFormMasterConfigurationData(objRoot.data);
                if (result == null)
                {
                    return BadRequest(new ResponseResult()
                    {
                        Message = ResponseMessage.SomethingWentWrong,
                        ResponseCode = ResponseCode.SomethingWentWrong
                    });
                }
                return Ok(result);

            });
        }
        /// <summary>
        /// To Delete Form Data
        /// </summary>
        /// <param name="jsonObj"> Form Data in the form of JSON object</param>
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
        /// <param name="jsonObj"> Form Data in the form of JSON object</param>
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
        [HttpGet]
        public async Task<IActionResult> GetAllDataSourceTables()
        {
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetAllDataSourceTables();
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
        /// Save table name into DataSource
        /// </summary>
        /// <param name="tableList">List of table Names</param>
        /// <returns></returns>
        [Route("SaveAllDataSourceTables")]
        [HttpPost]
        public async Task<IActionResult> SaveAllDataSourceTables([FromBody] string[] tableList)
        {
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.SaveAllDataSourceTables(tableList, loggedInAccountId);
                if (result != null && result.ResponseCode == ResponseCode.RecordSaved)
                {
                    return Ok(result);
                }
                return BadRequest(result);

            });
        }
    }
}
