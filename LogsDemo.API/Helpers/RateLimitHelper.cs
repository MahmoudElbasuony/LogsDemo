using AspNetCoreRateLimit;
using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsDemo.API.Helpers
{
    public static class RateLimitHelper
    {
        /// <summary>
        /// Load users rate limits from Database
        /// </summary>
        /// <param name="logSystemUnitOfWork">UOW used to retrieve  users </param>
        /// <param name="clientRatePolicies">Current Rate Limit Policies To Add New Rule Into It</param>
        public async static void LoadUsersRateLimits(ILogSystemUnitOfWork logSystemUnitOfWork, IOptions<ClientRateLimitPolicies> clientRatePolicies)
        {
            var clientRateLimitPolicies = clientRatePolicies.Value;

            var users = await logSystemUnitOfWork.UserRepository.ListAsync();

            if (users != null && clientRateLimitPolicies != null)
            {
                clientRateLimitPolicies.ClientRules = new List<ClientRateLimitPolicy>();
                foreach (var user in users)
                    clientRateLimitPolicies.ClientRules.Add(CreateUserRateLimitPolicy(user));
            }

        }
        /// <summary>
        /// Create New user Rate Limit Policy Rule
        /// </summary>
        /// <param name="user">User to create rule for </param>
        /// <returns></returns>
        public static ClientRateLimitPolicy CreateUserRateLimitPolicy(User user)
        {
            return new ClientRateLimitPolicy
            {
                ClientId = user.ID,
                Rules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Limit = user.ThrottlingLimit,
                        Period = user.ThrottlingPeriod,
                        // for [ Post,Put,Get ] 
                        Endpoint = "*"
                    }
                }
            };


        }
    }
}
