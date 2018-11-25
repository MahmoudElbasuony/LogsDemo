using AspNetCoreRateLimit;
using AutoMapper;
using LogsDemo.API.Helpers;
using LogsDemo.API.Models;
using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using LogsDemo.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
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

                return base.Ok(Mapper.Map<Models.UserDto>(user));

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
                var user = Mapper.Map<UserEntity>(dto);

                user = await userService.CreateUserAsync(user);

                #region User Rate Limit

                // set user rate limit 

                var clientPolicy = RateLimitHelper.CreateUserRateLimitPolicy(user.ID, user.ThrottlingLimit, user.ThrottlingPeriod);

                if (clientPolicy != null && clientRateLimitOptions != null && clientPolicyStore != null && !clientPolicyStore.Exists(clientPolicy.ClientId))
                {
                    clientPolicyStore.Set($"{clientRateLimitOptions.ClientPolicyPrefix}_{clientPolicy.ClientId}", clientPolicy);
                }

                #endregion
                

                return CreatedAtRoute("GetUser", new { Id = user.ID }, user);
            }
            catch (CustomException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


    }
}
