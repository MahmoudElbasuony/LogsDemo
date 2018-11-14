using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsDemo.API.Middlewares
{
    public class CustomClientRateLimit : ClientRateLimitMiddleware
    {
        private readonly ILogger<ClientRateLimitMiddleware> logger;
        public CustomClientRateLimit(RequestDelegate next, IOptions<ClientRateLimitOptions> options, IRateLimitCounterStore counterStore, IClientPolicyStore policyStore, ILogger<ClientRateLimitMiddleware> logger) : base(next, options, counterStore, policyStore, logger)
        {
            this.logger = logger;
        }

        public override void LogBlockedRequest(HttpContext httpContext, ClientRequestIdentity identity, RateLimitCounter counter, RateLimitRule rule)
        {
            // custom logging for request blocking 
            this.logger.LogWarning($" {DateTime.Now.ToString()} : User with id {identity.ClientId} was blocked to access {identity.HttpVerb} : {identity.Path}  due to [ Limit :  {rule.Limit} , Period : {rule.Period} , Endpoint : {rule.Endpoint} ]! ");

            base.LogBlockedRequest(httpContext, identity, counter, rule);
        }

    }
}
