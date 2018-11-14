using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using LogsDemo.SharedKernel.Exceptions;
using System;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Services
{
    public class UserService : BusinessService<User>, IUserService
    {
        
        private readonly IUserRepository userRepository;

        public UserService(ILogSystemUnitOfWork logSystemUnitOfWork) : base(logSystemUnitOfWork)
        {
            this.userRepository = this.logSystemUnitOfWork?.UserRepository;
        }


        public async Task<User> CreateUserAsync(User user) 
        {
            if (user == null)
                throw new CustomException("User Not Found");

            if (await userRepository?.IsExistsAsync(user.ID))
                throw new CustomException("User Already Exists");

            try
            {

                var createdUser = await userRepository?.CreateAsync(user);

                return createdUser;
            }
            catch (Exception)
            {
                throw new CustomException("Failed To Create User ");
            }
        }

        public async Task<User> GetUserAsync(string userId) => await userRepository?.GetByIDAsync(userId);

        public async Task<bool> UserExistsAsync(string userId) => await userRepository?.IsExistsAsync(userId);
    }
}
