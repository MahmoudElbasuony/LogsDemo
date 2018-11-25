using LogsDemo.Domain.Interfaces;
using LogsDemo.Domain.Services;
using LogsDemo.Infrastructure.Helpers;
using LogsDemo.Infrastructure.Mongo.Repositories;
using LogsDemo.Infrastructure.Mongo.Utilities;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using StructureMap.Pipeline;
using System;

namespace LogsDemo.Infrastructure.DI
{
    public static class Bootstraper
    {
        public static IServiceProvider Bootstrap(IServiceCollection services, Settings settings)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(scanner =>
                {
                    scanner.TheCallingAssembly();
                    scanner.WithDefaultConventions();
                });


                // servcies 

                var mongoClient = MongoConnectionFactory.GetConnection(settings.ConnectionString);
                config.For<IUserRepository<string>>().LifecycleIs(new ThreadLocalStorageLifecycle()).Use(MongoUserRepository.Create(mongoClient, settings.DbName));
                config.For<ILogRepository<string>>().LifecycleIs(new ThreadLocalStorageLifecycle()).Use(MongoLogRepository.Create(mongoClient, settings.DbName));

                config.For<ILogService>().Use<LogService>();
                config.For<IUserService>().Use<UserService>();

                // external services 
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}
