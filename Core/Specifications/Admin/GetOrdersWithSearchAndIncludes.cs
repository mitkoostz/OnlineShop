using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core.Specifications.Admin
{
    public class GetOrdersWithSearchAndIncludes : BaseSpecification<Order>
    {
        public GetOrdersWithSearchAndIncludes(AdminOrdersManagerParams ordersParams) 
            : base(o => (string.IsNullOrEmpty(ordersParams.EmailSearch) || o.BuyerEmail.ToLower().Contains(ordersParams.EmailSearch.ToLower())) &&
            (string.IsNullOrEmpty(ordersParams.NameSearch) || (o.ShipToAddress.FirstName.ToLower().Contains(ordersParams.NameSearch.ToLower()) || o.ShipToAddress.LastName.ToLower().Contains(ordersParams.NameSearch.ToLower())) &&
            (string.IsNullOrEmpty(ordersParams.PaymentIntentSearch) || o.PaymentIntentId.Contains(ordersParams.PaymentIntentSearch)))) 

        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.OrderDate);
        }

       private bool CompareDayMounthYear(DateTimeOffset d1 , int year, int mounth, int day)
        {
            if(d1.Year == year && d1.Day == day && d1.Month == mounth)
            {
                return true;
            }
            return false;
        }


        
    }
}
