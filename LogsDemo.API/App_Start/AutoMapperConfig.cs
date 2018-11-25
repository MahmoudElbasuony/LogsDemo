using LogsDemo.API.Models;
using LogsDemo.Domain.Entities;
using LogsDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsDemo.API.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Setup()
        {
            
            AutoMapper.Mapper.Initialize(config =>
            {

                // Api <=> Domain
                config.CreateMap<LogCreateDto, LogEntity>();
                config.CreateMap<LogDto, LogEntity>();

                config.CreateMap<UserCreateDto, UserEntity>();
                config.CreateMap<UserDto, UserEntity>();


                // Domain  <=> Infrastructure
                config.CreateMap<LogEntity, Log<string>>();
                config.CreateMap<UserEntity, User<string>>();
            });
        }
    }
}
