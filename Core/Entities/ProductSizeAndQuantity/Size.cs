namespace Core.Entities.ProductSizeAndQuantityNameSpace
{
    public class Size : BaseEntity
    {
        //M S , 42, ,36 , XL
       public string SizeShortName { get; set; }

       public int ProductTypeId { get; set; }
       public ProductType ProductType { get; set; }
    }
}