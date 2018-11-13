﻿using AspNetCoreRateLimit;
using AutoMapper;
using LogsDemo.API.Helpers;
using LogsDemo.API.Models;
using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LogsDemo.API.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IClientPolicyStore clientPolicyStore;
        private readonly ClientRateLimitOptions clientRateLimitOptions;
        public UserController(IUserService userService, IClientPolicyStore clientPolicyStore, IOptions<ClientRateLimitOptions> clientRateOptions)
        {

            this.userService = userService;
            this.clientPolicyStore = clientPolicyStore;
            this.clientRateLimitOptions = clientRateOptions?.Value;
        }

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="Id">user id </param>
        /// <returns></returns>
        [HttpGet("{Id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest();

            try
            {
                var user = await userService.GetUserAsync(Id);

                if (user == null)
                    return NotFound();

                return Ok(Mapper.Map<UserDto>(user));

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        /// <summary>
        /// Create new user 
        /// </summary>
        /// <param name="dto">user details </param>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]UserCreateDto dto)
        {
            if (dto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = Mapper.Map<User>(dto);

                user = await userService.CreateUserAsync(user);

                #region User Rate Limit

                // set user rate limit 

                var clientPolicy = RateLimitHelper.CreateUserRateLimitPolicy(user);

                if (clientPolicy != null && clientRateLimitOptions != null && clientPolicyStore != null && !clientPolicyStore.Exists(clientPolicy.ClientId))
                {
                    clientPolicyStore.Set($"{clientRateLimitOptions.ClientPolicyPrefix}_{clientPolicy.ClientId}", clientPolicy);
                }

                #endregion
                

                return CreatedAtRoute("GetUser", new { Id = user.ID }, user);
            }
            catch (ApplicationException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


    }
}