using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using LogsDemo.Domain.Services;
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
        private readonly Mock<ILogSystemUnitOfWork> uow;

        private readonly Mock<IUserRepository> userRepo;

        private readonly List<User> users;

        private readonly IUserService userService;

        private const string userId = "5be862778d65ca616c3f448b";

        private readonly Func<User, bool> commonQuery;

        private readonly User newUser;

        public UserServiceTest()
        {
            // Common Arrangement

            commonQuery = u => u.ID == userId;

            newUser = new User
            {
                ID = userId,
                Name = "Mahmoud"
            };

            users = new List<User>
            {
                newUser
            };

            // mock user repository
            userRepo = new Mock<IUserRepository>();

            userRepo.Setup(repo => repo.IsExistsAsync(It.IsAny<string>()))
                .Returns<string>(id => Task.FromResult(users.Any(u => u.ID == id))).Verifiable();

            userRepo.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(newUser).Verifiable();

            userRepo.Setup(repo => repo.ListAsync())
                .ReturnsAsync(users).Verifiable();

            userRepo.Setup(repo => repo.GetByIDAsync(It.IsAny<string>()))
                .ReturnsAsync(users.FirstOrDefault(u => u.ID == userId)).Verifiable();

            userRepo.Setup(repo => repo.GetOnAsync(It.IsAny<Expression<Func<User, bool>>>()))
                           .ReturnsAsync(users.Where(commonQuery).ToList()).Verifiable();


            // mock unit of work 
            uow = new Mock<ILogSystemUnitOfWork>();
            uow.SetupGet(uow => uow.UserRepository)
                .Returns(userRepo.Object).Verifiable();


            userService = new UserService(uow.Object);

        }



        [Fact]
        public async Task Create_New_User_Test()
        {
            // Arrange 
            var xuser = new User
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
            userRepo.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.AtMostOnce());

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
