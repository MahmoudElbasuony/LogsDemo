using System;
using AspNetCoreRateLimit;
using LogsDemo.API.App_Start;
using LogsDemo.API.Helpers;
using LogsDemo.Domain.Interfaces;
using LogsDemo.Infrastructure.DI;
using LogsDemo.Infrastructure.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LogsDemo.API
{
    public class Startup
    {
        private readonly Settings settings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Application settings 
            settings = new Settings
            {
                DbName = Configuration["DataBase:Name"] ?? Constants.DefaultDatabaseName
            };

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // pipe line middlware services  
            IocConfig.Config(services, Configuration);

            // register services in structure map container 
            return Bootstraper.Bootstrap(services, settings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ILogSystemUnitOfWork logSystemUnitOfWork, IOptions<ClientRateLimitPolicies> clientRateLimitPolicies)
        {

            // will load user rate limit in memory cache 
            RateLimitHelper.LoadUsersRateLimits(logSystemUnitOfWork, clientRateLimitPolicies);

            //app.UseClientRateLimiting();
            app.UseMiddleware<CustomClientRateLimit>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            app.UseStaticFiles();

            // Config Auto Mapper 
            AutoMapperConfig.Setup();

            // Seed Database with initial data 
            // SeedDBHelper.EnsureDBSeeded(logSystemUnitOfWork, settings);


            // setup swagger ui 
            SwaggerConfig.Config(app);

            app.UseMvc();
        }
    }
}
