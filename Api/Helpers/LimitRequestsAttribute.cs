using Api.Errors;
using Core.Interfaces;
using Core.LimitRequestsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                //TODO: Think what to do if we don't have the IP in the request.
                await next();
                return;
            } 
            string controllerName = context.RouteData.Values["controller"].ToString();
            string actionName = context.RouteData.Values["action"].ToString();
            string methodType = context.HttpContext.Request.Method;

            LimitRequestBannedData data = await limitRequestsService.CheckForBanAndIncreaseRequestsCount(ip, requestsPerTime, time, banTime, controllerName, actionName, methodType, uniqueForEveryActionAndController, useTestMode);
            if (data.IsBanned)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                if(data.ExpiryDate?.CompareTo(TimeSpan.FromSeconds(1)) == -1)
                {
                    data.ExpiryDate = TimeSpan.FromSeconds(1);
                }
                string tryAgainAfter = data.ExpiryDate?.ToString(@"dd\.hh\:mm\:ss");
                var contentResult = new ContentResult
                {
                    
                    Content = JsonSerializer.Serialize(
                        new ApiResponse(400,$"You have made too many requests!Try again after {tryAgainAfter}"),options),
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
