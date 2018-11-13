using LogsDemo.API.Models;
using LogsDemo.Domain.Entities;
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
                config.CreateMap<LogCreateDto, Log>();
                config.CreateMap<Log, LogDto>();

                config.CreateMap<UserCreateDto, User>();
                config.CreateMap<User, UserDto>();
            });
        }
    }
}
