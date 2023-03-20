using Common.Model;
using System.Threading.Tasks;

namespace Logger
{
    public interface IFileLogger
    {/// <summary>
    /// Write Log Info in Log file
    /// </summary>
    /// <param name="auditLogEntity"></param>
    /// <returns></returns>
          Task WriteInfo(AuditLogEntity auditLogEntity);
    }
}