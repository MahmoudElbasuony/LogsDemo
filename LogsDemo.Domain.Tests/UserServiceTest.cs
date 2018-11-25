using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using LogsDemo.Domain.Services;
using LogsDemo.Domain.Tests.Helpers;
using LogsDemo.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace LogsDemo.Domain.Tests
{
    public class UserServiceTest 
    {

        private readonly Mock<IUserRepository<string>> userRepo;

        private readonly List<User<string>> users;

        private readonly IUserService userService;

        private const string userId = "5be862778d65ca616c3f448b";

        private readonly Func<User<string>, bool> commonQuery;

        private readonly User<string> newUser;

        public UserServiceTest()
        {
            // Common Arrangement

            AutoMapperConfig.Setup();

            commonQuery = u => u.ID == userId;

            newUser = new User<string>
            {
                ID = userId,
                Name = "Mahmoud"
            };

            users = new List<User<string>>
            {
                newUser
            };

            // mock user repository
            userRepo = new Mock<IUserRepository<string>>();

            userRepo.Setup(repo => repo.IsExistsAsync(It.IsAny<string>()))
                .Returns<string>(id => Task.FromResult(users.Any(u => u.ID == id))).Verifiable();

            userRepo.Setup(repo => repo.CreateAsync(It.IsAny<User<string>>()))
                .ReturnsAsync(newUser).Verifiable();

            userRepo.Setup(repo => repo.ListAsync())
                .ReturnsAsync(users).Verifiable();

            userRepo.Setup(repo => repo.GetByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(users.FirstOrDefault(u => u.ID == userId)).Verifiable();

            userRepo.Setup(repo => repo.GetOnAsync(It.IsAny<Expression<Func<User<string>, bool>>>()))
                           .ReturnsAsync(users.Where(commonQuery).ToList()).Verifiable();


            // mock unit of work 
            userService = new UserService(userRepo.Object);

        }



        [Fact]
        public async Task Create_New_User_Test()
        {
            // Arrange 
            var xuser = new UserEntity
            {
                ID = "5be8628888888888888f458b",
                Name = "New User"
            };

            // Act
            var createdUser = await userService.CreateUserAsync(xuser);

            // Assert
            Assert.NotNull(createdUser);
            Assert.Equal(createdUser.ID, newUser.ID);

            userRepo.Verify(repo => repo.IsExistsAsync(It.IsAny<string>()), Times.AtLeastOnce());
            userRepo.Verify(repo => repo.CreateAsync(It.IsAny<User<string>>()), Times.AtMostOnce());

        }

        [Fact]
        public async Task User_Exists_Test()
        {
            // Act
            var exists = await userService.UserExistsAsync(userId);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task Get_User_By_Id_Test()
        {
            // Act
            var existedUser = await userService.GetUserAsync(userId);

            // Assert
            Assert.NotNull(existedUser);
        }

    }
}
