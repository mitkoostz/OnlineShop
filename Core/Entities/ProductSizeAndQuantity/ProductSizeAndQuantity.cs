namespace Core.Entities.ProductSizeAndQuantityNameSpace
{
    public class ProductSizeAndQuantity : BaseEntity
    {
        public int SizeId { get; set; }

        public Size Size { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

    }
}