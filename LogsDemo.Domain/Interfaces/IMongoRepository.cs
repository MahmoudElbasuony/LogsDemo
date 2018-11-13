using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface IMongoRepository<T> : IRepository<T> where T : class
    {
        Task<T> Update(T Entity);
    }
}
