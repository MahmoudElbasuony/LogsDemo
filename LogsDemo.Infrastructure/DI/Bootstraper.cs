using LogsDemo.Domain.Interfaces;
using LogsDemo.Domain.Services;
using LogsDemo.Infrastructure.Contexts;
using LogsDemo.Infrastructure.Helpers;
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

                config.For<ILogSystemUnitOfWork>().LifecycleIs(new ThreadLocalStorageLifecycle()).Use(MongoUnitOfWork.Create(settings.DbName));
                config.For<ILogService>().Use<LogService>();
                config.For<IUserService>().Use<UserService>();

                // external services 
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}
