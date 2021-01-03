using System.Collections.Generic;

namespace Core.Entities.ProductDiscounts
{
    public class DiscountType : BaseEntity
    {
        public string Name { get; set; }

        public List<ProductDiscount> ProductDiscounts { get; set; }
    }
}