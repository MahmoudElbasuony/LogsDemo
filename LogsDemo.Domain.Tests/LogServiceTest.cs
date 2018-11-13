using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Enums;
using LogsDemo.Domain.Interfaces;
using LogsDemo.Domain.Services;
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
        private readonly Mock<ILogSystemUnitOfWork> uow;

        private readonly Mock<ILogRepository> logRepo;

        private readonly Mock<IUserRepository> userRepo;

        private readonly List<Log> logs;

        private readonly ILogService logService;

        private const string logId = "5be86278888888888c3f558b";

        private const string userId = "5be862778d65ca616c3f448b";

        private readonly Func<Log, bool> commonQuery;

        private readonly Log newlog;


        public LogServiceTest()
        {
            // Common Arrangement

            commonQuery = u => u.ID == userId;

            newlog = new Log
            {
                ID = logId,
                UserId = userId,
                Message = "New Log"
            };

            logs = new List<Log>
            {
                newlog
            };

            // mock user repository
            userRepo = new Mock<IUserRepository>();

            userRepo.Setup(repo => repo.IsExistsAsync(It.IsAny<string>()))
                .Returns<string>(id => Task.FromResult(userId == id)).Verifiable();


            // mock log repository
            logRepo = new Mock<ILogRepository>();

            logRepo.Setup(repo => repo.CreateAsync(It.IsAny<Log>()))
                .ReturnsAsync(newlog);

            logRepo.Setup(repo => repo.GetOnAsync(It.IsAny<Expression<Func<Log, bool>>>()))
                           .Returns<Expression<Func<Log, bool>>>(query => Task.FromResult<IList<Log>>(logs.Where(query.Compile()).ToList()));


            // mock unit of work 
            uow = new Mock<ILogSystemUnitOfWork>();

            uow.SetupGet(uow => uow.LogRepository)
                .Returns(logRepo.Object).Verifiable();

            uow.SetupGet(uow => uow.UserRepository)
                .Returns(userRepo.Object).Verifiable();


            logService = new LogService(uow.Object);
        }

        [Fact]
        public async Task Create_Log_Async_Test()
        {
            // Arrange 
            var xlog = new Log
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
            userRepo.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.AtMostOnce());

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
            var existedLogs = await logService.GetUserLogsAsync(userId, start, end,severity, null);

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
