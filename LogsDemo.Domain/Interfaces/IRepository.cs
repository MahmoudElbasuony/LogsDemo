using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        Task<T> CreateAsync(T Entity);
        Task<IList<T>> ListAsync();

        Task<IList<T>> GetOnAsync(Expression<Func<T,bool>> expression);
    }
}
