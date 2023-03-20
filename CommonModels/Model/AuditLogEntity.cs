using System;

namespace Common.Model
{
    public class AuditLogEntity
    {
        /// <summary>
        /// LoggedDate when  file logged 
        /// </summary>
        public DateTime LoggedDate { get; set; }

        /// <summary>
        /// Logfile start Date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Logfile start Date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The absolute uri for redirection on urls
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Status code for successful logged
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Request Method
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Time taken for log file processing converted in ms
        /// </summary>
        public string ProcessedTime { get; set; }

        /// <summary>
        /// User identifier for user
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Status Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///  Log for Module created
        /// </summary>
        public short ModuleId { get; set; }
    }
}
