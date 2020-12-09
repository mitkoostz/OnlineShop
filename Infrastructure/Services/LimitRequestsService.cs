using Core.Interfaces;
using Core.LimitRequestsModels;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LimitRequestsService : ILimitRequestsService
    {
        private readonly IDatabase database;

        public LimitRequestsService(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }
        public async Task BanIpRequestsAsync(string ip, TimeSpan banTime)
        {
            await database.StringSetAsync(ip+":banned", "", banTime);
            ClearRequestsCount(ip);
            database.KeyDelete(ip + ":countValidTo");
        }

        public async Task<int> GetIpRequestCountAsync(string ip)
        {
            var requestsCount = await database.StringGetAsync(ip);    
            return int.Parse(requestsCount);
        }

        public async Task<LimitRequestBannedData> IsBannedAsync(string ip)
        { 
            if(database.KeyExists(ip + ":banned"))
            {
                RedisValueWithExpiry data = await database.StringGetWithExpiryAsync(ip + ":banned");
                LimitRequestBannedData returnData = new LimitRequestBannedData()
                {
                    IsBanned = true,
                    ExpiryDate = data.Expiry
                };
                return returnData;
            }
            return new LimitRequestBannedData() { IsBanned = false };
         

            //return await database.KeyExistsAsync(ip + ":banned");
        }

        public async Task SetIpRequestsCountAsync(string ip, TimeSpan time)
        {
            int currentRequests = 0;
            if(database.KeyExists(ip))
            {
                currentRequests = await GetIpRequestCountAsync(ip);
            }
            await database.StringSetAsync(ip, ++currentRequests,time);
            await UpdateValidToTime(ip,time);
        }
        private async void ClearRequestsCount(string ip)
        {
            await database.KeyDeleteAsync(ip);
        }

        public async Task UpdateValidToTime(string ip, TimeSpan time)
        {
            if(!database.KeyExists(ip + ":countValidTo"))
            {
                DateTime countValidTo = DateTime.Now.Add(time);
                 await database.StringSetAsync(ip + ":countValidTo", countValidTo.ToString(), time.Add(time));
                return;
            }
            var validToTime = await database.StringGetAsync(ip + ":countValidTo");
            DateTime validTo = DateTime.Parse(validToTime);
            if(DateTime.Now > validTo)
            {
                ClearRequestsCount(ip);
                DateTime countValidTo = DateTime.Now.Add(time);
                await database.StringSetAsync(ip + ":countValidTo", countValidTo.ToString(), time.Add(time));
            }
        }

        public async Task<LimitRequestBannedData> CheckForBanAndIncreaseRequestsCount(
                string ip,
                int requestsPerTime,
                TimeSpan time,
                TimeSpan banTime,
                string controllerName,
                string actionName,
                string methodType,
                bool uniqueForEveryActionAndController = true,
                bool useTestMode = false)
        {
            if (useTestMode)
            {
                ip = $"101.TEST.MODE.101";
            }
            if (uniqueForEveryActionAndController)
            {
                ip = GenerateUniquePathKeyIp(methodType,ip,controllerName,actionName);
            }else
            {
               ip = "LimitRequestsCounter:" + ip;
            }

            await UpdateValidToTime(ip, time);
            LimitRequestBannedData isBannedData = await IsBannedAsync(ip);
            bool isBanned = isBannedData.IsBanned;

            if (isBanned)
            {
               return isBannedData;
            }
            else
            {
               await SetIpRequestsCountAsync(ip, time);
               if (await GetIpRequestCountAsync(ip) >= requestsPerTime)
               {
                   await BanIpRequestsAsync(ip.ToString(), banTime);         
               }
               return isBannedData;
            }

           
        }

        private string GenerateUniquePathKeyIp(string methodType, string ip,string controller,string action)
        {
            return $"LimitRequests:{methodType}:{controller}:{action}:{ip}";
        }
    }
}
