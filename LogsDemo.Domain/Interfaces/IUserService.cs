using LogsDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Interfaces
{
    public interface IUserService  
    {
        Task<UserEntity> CreateUserAsync(UserEntity user);
        Task<UserEntity> GetUserAsync(string userId);
        Task<bool> UserExistsAsync(string userId);
    }
}
