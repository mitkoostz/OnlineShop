using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos.AdminPanel
{
    public class OrdersForDayWeekMounth
    {
        public int OrdersToday { get; set; }
        public int OrdersWeek { get; set; }
        public int OrdersMounth { get; set; }


    }
}
