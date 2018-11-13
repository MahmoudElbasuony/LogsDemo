using AspNetCoreRateLimit;
using AutoMapper;
using LogsDemo.API.Models;
using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Enums;
using LogsDemo.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LogsDemo.API.Controllers
{
    [Route("api/users/{userId}/logs")]
    public class LogController : Controller
    {
        private readonly ILogService logService;

        private readonly IUserService userService;

        public LogController(ILogService logService, IUserService userService)
        {
            this.logService = logService;
            this.userService = userService;
        }


        /// <summary>
        /// Get  log by log Id and user id
        /// </summary>
        /// <param name="logId">Log Id</param>
        /// <param name="userId">User id that belongs this log</param>
        [HttpGet("{logId}", Name = "GetLog")]
        public async Task<IActionResult> GetUserLog(string userId, string logId)
        {
            try
            {
                if (!await userService.UserExistsAsync(userId))
                {
                    ModelState.AddModelError(nameof(userId), $"User with Id {userId} Not Found");

                    return BadRequest(ModelState);
                }


                var log = Mapper.Map<LogDto>(await logService.GetUserLogAsync(userId, logId));

                if (log == null)
                    NotFound();

                return Ok(log);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        /// <summary>
        /// Search for user logs
        /// </summary>
        /// <param name="userId">User Id the owns logs [ Required ]</param>
        /// <param name="from">From date in format [yyyy-mm-dd hh:mm:ss.tt] e.x "2018-11-07 20:13:22.428 [Optional]"</param>
        /// <param name="to"> To date in format [yyyy-mm-dd hh:mm:ss.tt] e.x "2018-11-07 20:13:22.428 [Optional]"</param>
        /// <param name="severity">Severity of log {Low = 0, Medium = 1, High = 2, Etc } [Optional]  </param>
        /// <param name="type">Type of log { Debug = 0, Trace = 1, Info = 2, Etc } [Optional]  </param>
        [HttpGet]
        public async Task<IActionResult> GetUserLogs(string userId, DateTime? from, DateTime? to, LogSeverity? severity, LogType? type)
        {
            try
            {

                if (!await userService.UserExistsAsync(userId))
                {
                    ModelState.AddModelError(nameof(userId), $"User with Id {userId} Not Found");

                    return BadRequest(ModelState);
                }


                var logs = Mapper.Map<List<LogDto>>(await logService.GetUserLogsAsync(userId, from, to, severity, type));

                return Ok(logs);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        /// <summary>
        /// Creates new user log 
        /// </summary>
        /// <param name="createDto"> Data transfer object represent log </param>
        /// <param name="userId">User id </param>
        [HttpPost]
        public async Task<IActionResult> CreateLog(string userId, [FromBody] LogCreateDto createDto)
        {
            if (createDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                if (!await userService.UserExistsAsync(userId))
                {
                    ModelState.AddModelError(nameof(userId), $"User with Id {userId} Not Found");

                    return BadRequest(ModelState);
                }

                var log = Mapper.Map<Log>(createDto);

                log.UserId = userId;

                log = await logService.CreateLogAsync(log);

                return CreatedAtRoute("GetLog", new { logId = log.ID,  userId }, log);

            }
            catch (ApplicationException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
