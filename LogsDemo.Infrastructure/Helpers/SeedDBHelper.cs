using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Infrastructure.Helpers
{
    public static class SeedDBHelper
    {
        public static void EnsureDBSeeded(ILogSystemUnitOfWork logSystemUnitOfWork, Settings settings = null)
        {
            if (logSystemUnitOfWork != null)
                SeedUsers(logSystemUnitOfWork, settings);

        }


        private async static void SeedUsers(ILogSystemUnitOfWork logSystemUnitOfWork, Settings settings = null)
        {
            var userRepo = logSystemUnitOfWork.UserRepository;


            var users = new List<User>
            {
                new User
                {
                     ID = Guid.Parse("3709d18a-260c-4eff-841c-3cd59a2d1742").ToString("N"),
                     Name = "Mahmoud",
                     ThrottlingLimit = 5,
                       ThrottlingPeriod = "5m"
                },
                new User
                {
                     ID = Guid.Parse("933055fa-908e-46c6-aadf-2c22603b6e8c").ToString("N"),
                     Name = "Ali",
                      ThrottlingLimit = 5,
                       ThrottlingPeriod = "5m"
                },
                new User
                {
                     ID = Guid.Parse("29e73381-110e-4825-86ba-a9d87c207966").ToString("N"),
                     Name = "Kamal",
                      ThrottlingLimit = 5,
                       ThrottlingPeriod = "5m"
                },
            };

            foreach (var user in users)
                if (!await userRepo.IsExistsAsync(user.ID))
                    await userRepo.CreateAsync(user);

        }
    }
}
