using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications
{
    public class GetGenderByName : BaseSpecification<ProductGenderBase>
    {
        public GetGenderByName(string name) 
            : base(g => g.Name.ToLower() == name.ToLower())
        {
        }
    }
}
