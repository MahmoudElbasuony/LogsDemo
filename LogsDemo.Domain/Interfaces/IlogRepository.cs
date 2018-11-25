using LogsDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface ILogRepository<TKey> :  IRepository<Log<TKey>>
    {
        Task<Log<TKey>> DeleteAsync(TKey userID);
        Task<Log<TKey>> UpdateAsync(Log<TKey> log);
        Task<Log<TKey>> GetByIDAsync(TKey logId);
        Task<bool> IsExistsAsync(TKey logID);
    }
}
