using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using LogsDemo.SharedKernel.Linq;
using LogsDemo.SharedKernel.Exceptions;
using LogsDemo.Models;
using LogsDemo.Models.Enums;
using AutoMapper;

namespace LogsDemo.Domain.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository<string> logRepository;

        private readonly IUserRepository<string> userRepository;

        public LogService(IUserRepository<string> userRepository, ILogRepository<string> logRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        }

        public async Task<LogEntity> CreateLogAsync(LogEntity log)
        {

            if (log == null)
                throw new CustomException("Log Can't Be Null .");

            if (string.IsNullOrWhiteSpace(log.UserId))
                throw new CustomException("Log's User Id Required .");

           
            try
            {
                if (!await userRepository?.IsExistsAsync(log.UserId))
                    throw new CustomException($"User with ID : {log.UserId} Not Exist .");

                var newLog = Mapper.Map<Log<string>>(log);

                var createdLog = await logRepository?.CreateAsync(newLog);

                return Mapper.Map<LogEntity>(createdLog);

            }
            catch (Exception)
            {
                throw new CustomException("Failed To Create Log .");
            }
        }

        public async Task<LogEntity> GetUserLogAsync(string userId, string logId)
        {
            var userLog =  (await logRepository?.GetOnAsync(log => log.ID == logId && log.UserId == userId)).SingleOrDefault();

            return Mapper.Map<LogEntity>(userLog);
        }

        public async Task<IList<LogEntity>> GetUserLogsAsync(string userId, DateTime? start, DateTime? end, LogSeverity? severity, LogType? type)
        {

            Expression<Func<Log<string>, bool>> filter = (log) => log.UserId == userId;

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

            return Mapper.Map<IList<LogEntity>>(await logRepository?.GetOnAsync(filter));
        }
    }
}
