using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Admin
{
    public class GetProductsSpecification : BaseSpecification<Product>
    {

        public GetProductsSpecification(ProductSpecParams productParams)
        : base(x =>
           (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
           (!productParams.ProductGenderBaseId.HasValue || x.ProductGenderBaseId == productParams.ProductGenderBaseId) &&
           (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
        )
        {
            AddInclude(x => x.ProductGenderBase);
            AddInclude(x => x.ProductType);
            AddOrderBy(x => x.Name);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x => x.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;

                }
            }
        }
    }
}
