using AutoMapper;
using LogsDemo.Domain.Entities;
using LogsDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Domain.Tests.Helpers
{
    public static class AutoMapperConfig
    {
        public static void Setup()
        {
            Mapper.Reset();
            Mapper.Initialize(config =>
            {

                // Domain  <=> Infrastructure
                config.CreateMap<LogEntity, Log<string>>();
                config.CreateMap<UserEntity, User<string>>();
            });
        }
    }
}
