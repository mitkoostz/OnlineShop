using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<bool> IsBannedAsync(string ip)
        {
            return await database.KeyExistsAsync(ip + ":banned");
        }

        public async Task SetIpRequestsCountAsync(string ip, TimeSpan time)
        {
            int currentRequests = 0;
            if(database.KeyExists(ip))
            {
                currentRequests = await GetIpRequestCountAsync(ip);
            }
            await database.StringSetAsync(ip, ++currentRequests,time);
            await UpdateValidTo(ip,time);
        }
        private async void ClearRequestsCount(string ip)
        {
            await database.KeyDeleteAsync(ip);
        }

        public async Task UpdateValidTo(string ip, TimeSpan time)
       {
            var validToTry = await database.StringGetAsync(ip + ":countValidTo");
            if(!database.KeyExists(ip + ":countValidTo"))
            {
                DateTime countValidTo = DateTime.Now.Add(time);
                 await database.StringSetAsync(ip + ":countValidTo", countValidTo.ToString(), time.Add(time));
                return;
            }
            DateTime validTo = DateTime.Parse(validToTry);
            if(DateTime.Now > validTo)
            {
                ClearRequestsCount(ip);
                DateTime countValidTo = DateTime.Now.Add(time);
                await database.StringSetAsync(ip + ":countValidTo", countValidTo.ToString(), time.Add(time));
            }

        }

        public async Task<bool> IncreaseRequestCountAndCheckforBan(string ip,int requestsPerTime,  TimeSpan time, TimeSpan banTime)
        {
            await SetIpRequestsCountAsync(ip.ToString(), time);
            if (await GetIpRequestCountAsync(ip.ToString()) >= requestsPerTime)
            {
                await BanIpRequestsAsync(ip.ToString(), banTime);
                return true;
            }
            return false;
        }
    }
}
