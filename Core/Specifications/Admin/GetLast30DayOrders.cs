using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications.Admin
{
    public class GetLast30DayOrders : BaseSpecification<Order>
    {
        public GetLast30DayOrders() 
            : base(o => o.OrderDate >= DateTime.Now.AddDays(-30))
        {
            AddOrderByDescending(o => o.OrderDate);

        }
    }
}
