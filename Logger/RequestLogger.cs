using Common.Model.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class RequestLogger : IRequestLogger
    {
        public async Task AddRequestLog(RequestLog requestLog)
        {
            var connectionString = AppConfiguration.CommonConnectionString;
            string queryText = "INSERT INTO iam.\"RequestLogs\"(\"UserId\", \"DeviceId\", \"DeviceType\", \"DeviceModel\"," +
                " \"DeviceManufacture\", \"DeviceOS\", \"DeviceOSVersion\", \"RequestedFrom\", \"UserAgent\", \"APIName\"," +
                "\"AppVersion\", \"EncryptedToken\", \"IPAddress\", \"AccountId\", \"CreatedOn\") "
                          + " VALUES(@UserId, @DeviceId, @DeviceType, @DeviceModel, @DeviceManufacture, @DeviceOS," +
                          " @DeviceOSVersion, @RequestedFrom, @UserAgent, @APIName, @AppVersion, @EncryptedToken, " +
                          " @IPAddress, @AccountId, @CreatedOn);";

            // create connection and command
            using (NpgsqlConnection cn = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(queryText, cn))
                {
                    //cmd.CommandText = queryText;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new NpgsqlParameter("@UserId", requestLog.UserId));
                    cmd.Parameters.Add(new NpgsqlParameter("@DeviceType", requestLog.DeviceType));
                    cmd.Parameters.Add(new NpgsqlParameter("@DeviceId", (!string.IsNullOrWhiteSpace(requestLog.DeviceId) ? requestLog.DeviceId: "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@DeviceModel", (!string.IsNullOrWhiteSpace(requestLog.DeviceModel) ? requestLog.DeviceModel: "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@DeviceManufacture", (!string.IsNullOrWhiteSpace(requestLog.DeviceManufacture) ? requestLog.DeviceManufacture: "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@DeviceOS", (!string.IsNullOrWhiteSpace(requestLog.DeviceOs) ? requestLog.DeviceOs : "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@DeviceOSVersion", (!string.IsNullOrWhiteSpace(requestLog.DeviceOsversion) ? requestLog.DeviceOsversion : "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@RequestedFrom", (!string.IsNullOrWhiteSpace(requestLog.RequestedFrom) ? requestLog.RequestedFrom : "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@UserAgent", (!string.IsNullOrWhiteSpace(requestLog.UserAgent) ? requestLog.UserAgent : "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@APIName", (!string.IsNullOrWhiteSpace(requestLog.Apiname) ? requestLog.Apiname : "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@AppVersion", (!string.IsNullOrWhiteSpace(requestLog.AppVersion) ? requestLog.AppVersion : "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@EncryptedToken", (!string.IsNullOrWhiteSpace(requestLog.EncryptedToken) ? requestLog.EncryptedToken : "")));

                    cmd.Parameters.Add(new NpgsqlParameter("@IPAddress", (!string.IsNullOrWhiteSpace(requestLog.Ipaddress) ? requestLog.Ipaddress : "")));
                    cmd.Parameters.Add(new NpgsqlParameter("@AccountId", (requestLog.AccountId != null ? requestLog.AccountId : 0)));
                    cmd.Parameters.Add(new NpgsqlParameter("@CreatedOn", DateTime.UtcNow));

                    if (cn != null)
                    {
                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                        cn.Close();
                    }
                    // open connection, execute INSERT, close connection
                   
                }
            }
        }
    }
}
