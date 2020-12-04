   using Api.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class LimitRequestsAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int requestsPerTime;
        private readonly bool uniqueForEveryActionAndController;
        private readonly bool useTestMode;
        private readonly TimeSpan time;
        private readonly TimeSpan banTime;

        public LimitRequestsAttribute(int requestsPerTime,
            int timeInSeconds,
            int banTimeInSeconds,
            bool uniqueForEveryActionAndController = true,
            bool useTestMode = false)
        {
            this.requestsPerTime = requestsPerTime;
            this.uniqueForEveryActionAndController = uniqueForEveryActionAndController;
            this.useTestMode = useTestMode;
            this.time = TimeSpan.FromSeconds(timeInSeconds);
            this.banTime = TimeSpan.FromSeconds(banTimeInSeconds);
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var limitRequestsService = context.HttpContext.RequestServices.GetRequiredService<ILimitRequestsService>();
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip == null && useTestMode == false)
            {
                await next();
                return;
            } 
            string controllerName = context.RouteData.Values["controller"].ToString();
            string actionName = context.RouteData.Values["action"].ToString();
            string methodType = context.HttpContext.Request.Method;

            //check if banned , increase request count and check max requests per given time
            if (await limitRequestsService.CheckForBanAndIncreaseRequestsCount(ip,requestsPerTime,time,banTime,controllerName,actionName,methodType,uniqueForEveryActionAndController,useTestMode))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var contentResult = new ContentResult
                {
                    Content = JsonSerializer.Serialize(
                        new ApiResponse(400,$"You have made too many requests!Try again after {banTime.Minutes} minutes"),options),
                    ContentType = "application/json",
                    StatusCode = 400
                };
            
                context.Result = contentResult;
                return;
            }
            //move to controller
            await next();           
        }


    }
}
