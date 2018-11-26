using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using LogsDemo.Domain.Services;
using LogsDemo.Domain.Tests.Helpers;
using LogsDemo.Models;
using LogsDemo.Models.Enums;
using LogsDemo.SharedKernel.Linq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LogsDemo.Domain.Tests
{
    public class LogServiceTest
    {
        private readonly Mock<ILogRepository<string>> logRepo;

        private readonly Mock<IUserRepository<string>> userRepo;

        private readonly List<Log<string>> logs;

        private readonly ILogService logService;

        private const string logId = "5be86278888888888c3f558b";

        private const string userId = "5be862778d65ca616c3f448b";

        private readonly Func<Log<string>, bool> commonQuery;

        private readonly Log<string> newlog;


        public LogServiceTest()
        {
            // Common Arrangement

            AutoMapperConfig.Setup();

            commonQuery = u => u.ID == userId;

            newlog = new Log<string>
            {
                ID = logId,
                UserId = userId,
                Message = "New Log"
            };

            logs = new List<Log<string>>
            {
                newlog
            };

            // mock user repository
            userRepo = new Mock<IUserRepository<string>>();

            userRepo.Setup(repo => repo.IsExistsAsync(It.IsAny<string>()))
                .Returns<string>(id => Task.FromResult(userId == id)).Verifiable();


            // mock log repository
            logRepo = new Mock<ILogRepository<string>>();

            logRepo.Setup(repo => repo.CreateAsync(It.IsAny<Log<string>>()))
                .ReturnsAsync(newlog);

            logRepo.Setup(repo => repo.GetOnAsync(It.IsAny<Expression<Func<Log<string>, bool>>>()))
                           .Returns<Expression<Func<Log<string>, bool>>>(query => Task.FromResult<IList<Log<string>>>(logs.Where(query.Compile()).ToList()));

            logService = new LogService(userRepo.Object, logRepo.Object);
        }

        [Fact]
        public async Task Create_Log_Async_Test()
        {
            // Arrange 
            var xlog = new LogEntity
            {
                ID = "5be8628888888888888f458b",
                Message = "New Log",
                UserId = userId
            };

            // Act
            var createdLog = await logService.CreateLogAsync(xlog);

            // Assert
            Assert.NotNull(createdLog);
            Assert.Equal(createdLog.ID, newlog.ID);

            userRepo.Verify(repo => repo.IsExistsAsync(It.IsAny<string>()), Times.AtLeastOnce());
            userRepo.Verify(repo => repo.CreateAsync(It.IsAny<User<string>>()), Times.AtMostOnce());

        }

        [Fact]
        public async Task Get_User_Log_Async_Test()
        {
            // Act
            var existedLog = await logService.GetUserLogAsync(userId, logId);

            // Assert
            Assert.NotNull(existedLog);
        }

        [Fact]
        public async Task Get_User_Logs_Async_Test()
        {
            // Act
            var existedLogs = await logService.GetUserLogsAsync(userId, null, null, null, null);

            // Assert
            Assert.NotNull(existedLogs);
            Assert.NotEmpty(existedLogs);
        }

        [Fact]
        public async Task Get_User_Logs_By_Date_Window_Async_Test()
        {
            // Arrange 
            DateTime? start = newlog.DateCreated,
                end = newlog.DateCreated;

            // Act
            var existedLogs = await logService.GetUserLogsAsync(userId, start, end, null, null);

            // Assert
            Assert.NotNull(existedLogs);
            Assert.NotEmpty(existedLogs);
        }

        [Fact]
        public async Task Get_User_Logs_By_Date_Window_And_Severity_Async_Test()
        {
            // Arrange 
            DateTime? start = newlog.DateCreated,
                end = newlog.DateCreated;

            LogSeverity? severity = newlog.Severity;

            // Act
            var existedLogs = await logService.GetUserLogsAsync(userId, start, end, severity, null);

            // Assert
            Assert.NotNull(existedLogs);
            Assert.NotEmpty(existedLogs);
        }
        [Fact]
        public async Task Get_User_Logs_By_Date_Window_And_Severity_And_Type_Async_Test()
        {
            // Arrange 
            DateTime? start = newlog.DateCreated,
                end = newlog.DateCreated;

            LogSeverity? severity = newlog.Severity;
            LogType? type = newlog.Type;

            // Act
            var existedLogs = await logService.GetUserLogsAsync(userId, start, end, severity, type);

            // Assert
            Assert.NotNull(existedLogs);
            Assert.NotEmpty(existedLogs);
        }
    }
}
