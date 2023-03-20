using Common.Model.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public interface IRequestLogger
    {
        Task AddRequestLog(RequestLog requestLog);
    }
}
