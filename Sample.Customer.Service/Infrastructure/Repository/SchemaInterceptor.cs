using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Sample.Customer.Service
{
    public class SchemaInterceptor : DbCommandInterceptor
    {
        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            //System.Console.WriteLine(command.CommandText);
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            //System.Console.WriteLine(command.CommandText);
            return base.ReaderExecuting(command, eventData, result);
        }
    }
}