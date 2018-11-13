using LogsDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface ILogRepository :  IRepository<Log>
    {
        Task<Log> DeleteAsync(string userID);
        Task<Log> UpdateAsync(Log log);
        Task<Log> GetByIDAsync(string logId);
        Task<bool> IsExistsAsync(string logID);
    }
}
