using Api.Dtos.ProductDetails;

namespace Core.Entities.ProductSizeAndQuantity
{
    public class ProductSizeAndQuantityReturnDto
    {
        public string SizeShortName { get; set; }
       //public SizeReturnDto Size { get; set; }
       
       public int Quantity { get; set; }

    }
}