using Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification :  BaseSpecification<Product>
    {

        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(x =>
               (string.IsNullOrEmpty(productParams.Search) ||  x.Name.ToLower().Contains(productParams.Search)) &&  
               (!productParams.ProductGenderBaseId.HasValue || x.ProductGenderBaseId == productParams.ProductGenderBaseId) &&
               (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(x => x.ProductGenderBase);
            AddInclude(x => x.ProductType);
            AddOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1),
                productParams.PageSize);

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
        public ProductsWithTypesAndBrandsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductGenderBase);
            AddInclude(x => x.ProductType);
        }
    }
}
