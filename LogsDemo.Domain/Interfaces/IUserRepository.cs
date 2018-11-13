using LogsDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> DeleteAsync(string userID);
        Task<User> UpdateAsync(User user);
        Task<User> GetByIDAsync(string userID);
        Task<bool> IsExistsAsync(string userID);
    }
}
