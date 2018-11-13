using LogsDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface IUserService : IBusinessService<User>
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserAsync(string userId);
        Task<bool> UserExistsAsync(string userId);
    }
}
