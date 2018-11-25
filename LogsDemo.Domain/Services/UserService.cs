using AutoMapper;
using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using LogsDemo.Models;
using LogsDemo.SharedKernel.Exceptions;
using System;
using System.Threading.Tasks;

namespace LogsDemo.Domain.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository<string> userRepository;

        public UserService(IUserRepository<string> userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }


        public async Task<UserEntity> CreateUserAsync(UserEntity user)
        {
            if (user == null)
                throw new CustomException("User Not Found");

            try
            {

                if (await userRepository.IsExistsAsync(user.ID))
                    throw new CustomException("User Already Exists");

                var userToCreate = Mapper.Map<User<string>>(user);

                var createdUser = await userRepository?.CreateAsync(userToCreate);

                return Mapper.Map<UserEntity>(createdUser);

            }
            catch (Exception)
            {
                throw new CustomException("Failed To Create User ");
            }
        }

        public async Task<UserEntity> GetUserAsync(string userId)
        {
            var user = Mapper.Map<UserEntity>(await userRepository?.GetByIDAsync(userId));

            return user;
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            return await userRepository?.IsExistsAsync(userId);
        }
    }
}
