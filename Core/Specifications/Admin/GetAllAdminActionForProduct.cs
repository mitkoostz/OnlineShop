using Core.Entities;
using Core.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications.Admin
{
    public class GetAllAdminActionForProduct : BaseSpecification<Product>
    {
        public GetAllAdminActionForProduct(int id) 
            : base(pr => pr.Id == id)
        {
            AddInclude(o => o.ProductAdminHistory);
        }
    }
}
