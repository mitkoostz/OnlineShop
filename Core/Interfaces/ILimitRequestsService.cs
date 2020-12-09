using Core.LimitRequestsModels;
using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ILimitRequestsService
    {
        Task<int> GetIpRequestCountAsync(string ip);
        Task SetIpRequestsCountAsync(string ip, TimeSpan time);
        Task BanIpRequestsAsync(string ip,TimeSpan bannedTo);
        Task<LimitRequestBannedData> IsBannedAsync(string ip);
        Task  UpdateValidToTime(string ip,TimeSpan time);
        Task<LimitRequestBannedData> CheckForBanAndIncreaseRequestsCount( string ip,
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
