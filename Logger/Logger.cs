using Common.Model;
using Serilog;
using System.Threading.Tasks;

namespace Logger
{
    public class FileLogger : IFileLogger
    {
        #region[PRIVATE VARIABLES]
        /// <summary>
        /// Private Method for Logger service
        /// </summary>
        private readonly ILogger logger;
        #endregion
        #region [Constructor]
        /// <summary>
        /// File Logger Constructor
        /// </summary>
        /// <param name="logger"></param>
        public FileLogger(ILogger logger)
        {
            this.logger = logger;
        }
        #endregion
        #region [METHOD]
        /// <summary>
        /// To write Log
        /// </summary>
        /// <param name="auditLogEntity"></param>
        public Task WriteInfo(AuditLogEntity auditLogEntity)
        {
            var logData = new { auditLogEntity.LoggedDate, auditLogEntity.Url, auditLogEntity.Method, auditLogEntity.Message, auditLogEntity.IsSuccess, auditLogEntity.StartDate, auditLogEntity.EndDate, auditLogEntity.ProcessedTime };
            logger.Information("Logg: {logData}", logData);
            return Task.CompletedTask;
        }
        #endregion

    }

}
