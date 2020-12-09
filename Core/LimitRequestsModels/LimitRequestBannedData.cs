using System;
using System.Collections.Generic;
using System.Text;

namespace Core.LimitRequestsModels
{
    public class LimitRequestBannedData
    {
        public bool IsBanned { get; set; }
        public TimeSpan? ExpiryDate { get; set; }
    }
}
