using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogsDemo.API.Filters
{
    public class ClientRateGuardAttribute : ActionFilterAttribute
    {

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string ClientIdHeaderName = Startup.settings?.ClientIdHeader;

            string ClientIdHeader = context?.HttpContext?.Request?.Headers?[ClientIdHeaderName];

            if (string.IsNullOrWhiteSpace(ClientIdHeader))
            {
                context.Result = new BadRequestObjectResult($"{ClientIdHeaderName} Header Must Be Provided");

                return Task.CompletedTask;
            }
            else
                return base.OnActionExecutionAsync(context, next);
        }
    }
}
