using System.Collections.Generic;
using Core.Entities.ProductSizeAndQuantity;
using Core.Entities.ProductSizeAndQuantityNameSpace;

namespace Api.Dtos.ProductDetails
{
    public class ProductDetailToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; }
        public string ProductGenderBase { get; set; }
        public decimal AverageReviewRate { get; set; }
        public List<ProductSizeAndQuantityReturnDto> ProductSizeAndQuantity {get; set;}
    }
}