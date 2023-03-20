using Common.Model;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
using Sample.Admin.HttpAggregator.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sample.Admin.HttpAggregator.Controllers
{
    /// <summary>
    /// Common Controller
    /// </summary>
    public class CommonController<T> : BaseApiController where T : class
    {
        private readonly ICommonService service;

        /// <summary>
        /// Group Controller constructor to Inject dependency
        /// </summary>
        /// <param name="service">logger service </param>
        /// <param name="logger">logger service </param>
        public CommonController(ICommonService service, IFileLogger logger) : base(logger: logger)
        {
            Check.Argument.IsNotNull(nameof(service), service);
            this.service = service;
        }

        /// <summary>
        ///  Information for all records
        /// </summary>
        [Route("")]
        [HttpGet]
        public virtual async Task<IActionResult> Get([FromQuery] string ordering, [FromQuery] int offset, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] bool all = false)
        {
            return await Execute(async () =>
            {
                var result = await service.GetAll<T>(HttpContext, ordering, offset, pageSize, pageNumber, all);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  Information for all single records
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return await Execute(async () =>
            {
                var result = await this.service.GetById<T>(HttpContext, id);
                if (result.ResponseCode == ResponseCode.RecordFetched)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  POST api/Controller
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] T model)
        {
            return await Execute(async () =>
            {
                var result = await this.service.Create(HttpContext, model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Created("api/groups/", result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  Update api/Controller/5
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] T model)
        {
            return await Execute(async () =>
            {
                var result = await this.service.Update(HttpContext, id, model);
                if (result.ResponseCode == ResponseCode.RecordSaved)
                    return Ok(result);
                else
                    return BadRequest(result);
            });
        }

        /// <summary>
        ///  DELETE api/Controller/5
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await this.service.Delete(HttpContext, id);
            if (result.ResponseCode == ResponseCode.RecordDeleted)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
