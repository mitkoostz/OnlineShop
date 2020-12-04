using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ILimitRequestsService
    {
        Task<int> GetIpRequestCountAsync(string ip);
        Task SetIpRequestsCountAsync(string ip, TimeSpan time);
        Task BanIpRequestsAsync(string ip,TimeSpan bannedTo);
        Task<bool> IsBannedAsync(string ip);
        Task  UpdateValidToTime(string ip,TimeSpan time);
        Task<bool> CheckForBanAndIncreaseRequestsCount( string ip,
                int requestsPerTime,
                TimeSpan time,
                TimeSpan banTime,
                string controllerName,
                string actionName,
                string methodType,
                bool uniqueForEveryActionAndController = true,
                bool useTestMode = false);

    }
}
