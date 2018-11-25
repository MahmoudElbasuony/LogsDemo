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
        /// <param name="clientRatePolicies">Current Rate Limit Policies To Add New Rule Into It</param>
        /// <param name="userRepository"></param>
        public async static void LoadUsersRateLimits(IUserRepository<string> userRepository, IOptions<ClientRateLimitPolicies> clientRatePolicies)
        {
            var clientRateLimitPolicies = clientRatePolicies.Value;

            var users = await userRepository.ListAsync();

            if (users != null && clientRateLimitPolicies != null)
            {
                clientRateLimitPolicies.ClientRules = new List<ClientRateLimitPolicy>();

                foreach (var user in users)
                    clientRateLimitPolicies.ClientRules.Add(CreateUserRateLimitPolicy(user.ID, user.ThrottlingLimit, user.ThrottlingPeriod));
            }

        }
        /// <summary>
        /// Create New user Rate Limit Policy Rule
        /// </summary>
        public static ClientRateLimitPolicy CreateUserRateLimitPolicy(string UserId, long ThrottlingLimit, string ThrottlingPeriod)
        {
            return new ClientRateLimitPolicy
            {
                ClientId = UserId,
                Rules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Limit = ThrottlingLimit,
                        Period = ThrottlingPeriod,
                        // for [ Post,Put,Get ] 
                        Endpoint = "*"
                    }
                }
            };


        }
    }
}
