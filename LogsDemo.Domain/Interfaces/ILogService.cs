
using LogsDemo.Domain.Entities;
using LogsDemo.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface ILogService
    {
        Task<LogEntity> CreateLogAsync(LogEntity log);
        Task<LogEntity> GetUserLogAsync(string userId, string logId);
        Task<IList<LogEntity>> GetUserLogsAsync(string userId, DateTime? from, DateTime? to, LogSeverity? severity, LogType? type);

    }
}
