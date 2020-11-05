using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications
{
    public class ProductTypeByProductTypeName : BaseSpecification<ProductType>
    {
        public ProductTypeByProductTypeName(string productTypeName) 
            : base(p => p.Name.Contains(productTypeName))
        {
        }
    }
}
