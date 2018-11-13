using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface ILogService : IBusinessService<Log>
    {
        Task<Log> CreateLogAsync(Log log);
        Task<Log> GetUserLogAsync(string userId, string logId);
        Task<IList<Log>> GetUserLogsAsync(string userId, DateTime? from, DateTime? to, LogSeverity? severity, LogType? type);

    }
}
