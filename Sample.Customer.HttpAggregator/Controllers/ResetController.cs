using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Data;
using Sample.Customer.HttpAggregator.Config;
using Dapper;
using Common.Model;
using System.Threading.Tasks;

namespace Sample.Customer.HttpAggregator.Controllers
{
    public class ResetController : BaseApiController
    {  /// <summary>
       /// Users Controller Constructor to inject services
       /// </summary>

       /// <param name="logger">The file logger</param>


        private readonly ResetDatabase resetDatabase;
        public ResetController(IFileLogger logger, IOptions<ResetDatabase> resetDB) : base(logger: logger)
        {
            resetDatabase = resetDB.Value;
        }
        /// <summary>
        /// SaveTripPaxAndBags
        /// </summary>
        /// <param name="resetRequest"></param>
        /// <returns></returns>
        [Route("resetdatabase")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ResetDatabase(ResetDatabase resetRequest)
        {
            ResponseResult responseResult = new ResponseResult();
            if (resetRequest != null && resetRequest.Key != null && resetRequest.UserId != null && resetRequest.Key.ToLower() == resetDatabase.Key.ToLower() && resetRequest.UserId.ToLower() == resetDatabase.UserId.ToLower())
            {
                long data = await ResetDBSP();
                 responseResult = new ResponseResult()
                {
                    Data = ResetDBSP(),
                    Message = "Database reset successfully.",
                    ResponseCode = ResponseCode.RecordFetched
                };

                return Ok(responseResult);
            }
            responseResult = new ResponseResult()
            {
                Data = 1,
                Message = "Not able to reset database. Please contact to admin.",
                ResponseCode = ResponseCode.InternalServerError
            };

            return BadRequest(responseResult);

        }
        [NonAction]
        private async Task<long> ResetDBSP()
        {
            try
            {
                string sqlQuery = $"call customer.sp_reset_database()";
                long result = 0;
                IDbConnection connection = new NpgsqlConnection(resetDatabase.DBConnection);
                connection.Open();
                result = Convert.ToInt64(await connection.ExecuteScalarAsync(sqlQuery));
                connection.Close();


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
