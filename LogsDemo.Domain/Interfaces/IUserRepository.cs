using LogsDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface IUserRepository<TKey> : IRepository<User<TKey>>
    {
        Task<User<TKey>> DeleteAsync(TKey userID);
        Task<User<TKey>> UpdateAsync(User<TKey> user);
        Task<User<TKey>> GetByIDAsync(TKey userID);
        Task<bool> IsExistsAsync(TKey userID);
    }
}
