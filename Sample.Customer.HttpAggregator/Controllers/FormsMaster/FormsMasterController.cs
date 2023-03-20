using System.Threading.Tasks;
using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Sample.Admin.Model;
using Sample.Customer.HttpAggregator.IServices.FormsMaster;
using Sample.Customer.Model;

namespace Sample.Customer.HttpAggregator.Controllers.FormsMaster
{
    /// <summary>
    /// FormsMaster Controller
    /// </summary>
    [Route("api/formsmaster")]
    [ApiController]
    public class FormsMasterController : BaseApiController
    {
        private readonly IFormMasterService formsMasterService;

        /// <summary>
        /// FormsMaster Controller constructor
        /// </summary>
        public FormsMasterController(IFormMasterService formsMasterService, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(formsMasterService), formsMasterService);
            this.formsMasterService = formsMasterService;
        }

        /// <summary>
        ///  Get all the controls for a screen
        /// </summary>
        /// <param name="screen">Name of the screen</param>
        /// <param name="dataSource">Datasource for all the fields in the screen</param>
        /// <returns></returns>
        [Route("GetScreenControls")]
        [HttpGet]
        public async Task<IActionResult> GetScreenControls(string screen, string dataSource)
        {
            return await Execute(async () =>
            {
                var controls = await formsMasterService.GetScreenControls(HttpContext, screen, dataSource);
                return Ok(controls);
            });
        }
        /// <summary>
        /// To Get data for the screen controls
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="datasource"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>

        [Route("getscreencontrolsdata")]
        [HttpGet]
        public async Task<IActionResult> GetScreenControlsData([FromQuery] string screen, [FromQuery] string datasource, [FromQuery] long? dataId)
        {
            return await Execute(async () =>
            {
                var controls = await formsMasterService.GetScreenControlsData(HttpContext, screen, datasource, dataId);
                return Ok(controls);
            });
        }

        /// <summary>
        /// Get Screen Controls List SearchData
        /// </summary>
        /// <param name="json">Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("getscreencontrolslistsearchdata")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> GetScreenControlsListSearchData([FromBody] ControlsDataResponseVM json)
        {
            Check.Argument.IsNotNull(nameof(json), json);

            return await Execute(async () =>
            {
                var result = await formsMasterService.GetScreenControlsListSearchData(json);
                return Ok(result);
            });
        }

        /// <summary>
        ///Insert Form Data
        /// </summary>
        /// <param name="json">Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("insertformdata")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> InsertFormData([FromBody] ControlsDataResponseVM json)
        {
            Check.Argument.IsNotNull(nameof(json), json);

            return await Execute(async () =>
            {
                var result = await formsMasterService.InsertFormData(json);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///Update Form Data
        /// </summary>
        /// <param name="json">Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("updateformdata")]
        [HttpPut]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        public async Task<IActionResult> UpdateFormData([FromBody] ControlsDataResponseVM json)
        {
            Check.Argument.IsNotNull(nameof(json), json);

            return await Execute(async () =>
            {
                var result = await formsMasterService.UpdateFormData(json);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///Delete Form Data
        /// </summary>
        /// <param name="json">Form Data in the form of JSON object</param>
        /// <returns></returns>
        [Route("deleteformdata")]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseResult), 200)]
        //[HasPermission(PermissionType.Delete)]
        public async Task<IActionResult> DeleteFormData([FromBody] ControlsDataResponseVM json)
        {
            Check.Argument.IsNotNull(nameof(json), json);

            return await Execute(async () =>
            {
                var result = await formsMasterService.DeleteFormData(json);
                if (result.ResponseCode == ResponseCode.RecordDeleted)
                    return Ok(result);
                else
                    return BadRequest(result);
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
