using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications.Admin
{
    public class GetAllOrdersWthDeliveryMethod : BaseSpecification<Order>
    {
        public GetAllOrdersWthDeliveryMethod() : base()
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.OrderDate);
        }
    }
}
