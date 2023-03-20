using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Service.ServiceWorker;
using Sample.Customer.Model;

namespace Sample.Customer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsMasterController : BaseApiController
    {
        private readonly IFormMasterService formsMasterService;

        public FormsMasterController(IFormMasterService formsMasterService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(formsMasterService), formsMasterService);
            this.formsMasterService = formsMasterService;
        }

        [Route("GetScreenControls")]
        [HttpGet]
        public async Task<IActionResult> GetScreenControls(string screen, string dataSource)
        {
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetScreenControls(screen, dataSource, base.loggedInAccountId);
                return Ok(result);
            });
        }

        [Route("getscreencontrolsdata")]
        [HttpGet]
        public async Task<IActionResult> GetScreenControlsData(string screen, string datasource, long? dataId)
        {
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetScreenControlsData(screen, datasource, base.loggedInAccountId, dataId);
                return Ok(result);
            });
        }

        /// <summary>
        /// To Get Screen controls list search data
        /// </summary>
        /// <param name="jsonObj"> Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("getscreencontrolslistsearchdata")]
        [HttpPost]
        public async Task<IActionResult> GetScreenControlsListSearchData([FromBody] ControlsDataResponseVM jsonObj)
        {
            return await Execute(async () =>
            {
                jsonObj.AccountId = base.loggedInAccountId;
                var result = await this.formsMasterService.GetScreenControlsListSearchData(jsonObj);
                return Ok(result);
            });
        }

        /// <summary>
        /// To Insert Form Data
        /// </summary>
        /// <param name="jsonObj"> Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("insertformdata")]
        [HttpPost]
        public async Task<IActionResult> InsertFormData([FromBody] ControlsDataResponseVM jsonObj)
        {
            Check.Argument.IsNotNull(nameof(jsonObj), jsonObj);
            return await Execute(async () =>
            {
                jsonObj.AccountId = base.loggedInAccountId;
                var result = await this.formsMasterService.InsertUpdateFormData(jsonObj);
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
        /// To Insert Form Data
        /// </summary>
        /// <param name="jsonObj"> Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("updateformdata")]
        [HttpPut]
        public async Task<IActionResult> UpdateFormData([FromBody] ControlsDataResponseVM jsonObj)
        {
            Check.Argument.IsNotNull(nameof(jsonObj), jsonObj);
            return await Execute(async () =>
            {
                jsonObj.AccountId = base.loggedInAccountId;
                var result = await this.formsMasterService.InsertUpdateFormData(jsonObj);
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
        [Route("deleteformdata")]
        [HttpPost]
        public async Task<IActionResult> DeleteFormData([FromBody] ControlsDataResponseVM jsonObj)
        {
            Check.Argument.IsNotNull(nameof(jsonObj), jsonObj);
            return await Execute(async () =>
            {
                jsonObj.AccountId = base.loggedInAccountId;
                var result = await this.formsMasterService.DeleteFormData(jsonObj);
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
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        [Route("GetAllValidationTypesOptions")]
        [HttpGet]
        public async Task<IActionResult> GetAllValidationTypesOptions()
        {
           
            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetAllValidationTypesOptions();
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
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        [Route("GetAllDataSources")]
        [HttpGet]
        public async Task<IActionResult> GetAllDataSources()
        {

            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetAllDataSources();
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
        /// GetAllValidationTypesOptions
        /// </summary>
        /// <returns></returns>
        [Route("GetAllControlTypes")]
        [HttpGet]
        public async Task<IActionResult> GetAllControlTypes()
        {

            return await Execute(async () =>
            {
                var result = await this.formsMasterService.GetAllControlTypes();
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
    }
}
