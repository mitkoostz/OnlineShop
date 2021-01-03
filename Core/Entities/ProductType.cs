using System.Collections.Generic;
using Core.Entities.ProductSizeAndQuantityNameSpace;

namespace Core.Entities
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }

        public List<Size> Sizes { get; set; }
    }
}