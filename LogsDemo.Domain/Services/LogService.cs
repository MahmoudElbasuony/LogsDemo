using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Enums;
using LogsDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using LogsDemo.SharedKernel.Linq;

namespace LogsDemo.Domain.Services
{
    public class LogService : BusinessService<Log>, ILogService
    {
        private readonly ILogRepository logRepository;

        private readonly IUserRepository userRepository;

        public LogService(ILogSystemUnitOfWork logSystemUnitOfWork) : base(logSystemUnitOfWork)
        {
            this.userRepository = base.logSystemUnitOfWork?.UserRepository;
            this.logRepository = base.logSystemUnitOfWork?.LogRepository;
        }

        public async Task<Log> CreateLogAsync(Log log)
        {

            if (log == null)
                throw new ApplicationException("Log Can't Be Null .");

            if (string.IsNullOrWhiteSpace(log.UserId))
                throw new ApplicationException("Log's User Id Required .");

            if (!await userRepository?.IsExistsAsync(log.UserId))
                throw new ApplicationException($"User with ID : {log.UserId} Not Exist .");

            try
            {
                var createdLog = await logRepository?.CreateAsync(log);

                return createdLog;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Failed To Create Log ", e.InnerException);
            }
        }

        public async Task<Log> GetUserLogAsync(string userId, string logId) => (await logRepository?.GetOnAsync(log => log.ID == logId && log.UserId == userId)).SingleOrDefault();

        public async Task<IList<Log>> GetUserLogsAsync(string userId, DateTime? start, DateTime? end, LogSeverity? severity, LogType? type)
        {

            Expression<Func<Log, bool>> filter = (log) => log.UserId == userId;

            if (start.HasValue)
                filter = filter.And(log => log.DateCreated >= start);


            if (end.HasValue)
            {
                end = end.Value.AddMilliseconds(999 - end.Value.Millisecond);
                filter = filter.And(log => log.DateCreated <= end);
            }

            if (severity.HasValue)
                filter = filter.And(log => log.Severity == severity);


            if (type.HasValue)
                filter = filter.And(log => log.Type == type);

            return await logRepository?.GetOnAsync(filter);
        }
    }
}
