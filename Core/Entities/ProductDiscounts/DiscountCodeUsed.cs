using System;

namespace Core.Entities.ProductDiscounts
{
    public class DiscountCodeUsed : BaseEntity
    {
        public int ProductDiscountCodeId { get; set; }
        
        public ProductDiscountCode ProductDiscountCode { get; set; }

        public string UserId { get; set; }

        public string DiscountTypeName { get; set; }

        public int DiscountUsedCount { get; set; }

        public decimal DiscountInPercent { get; set; }
        
        public decimal DiscountInValue { get; set; }

        public DateTime TimeUsed { get; set; }

        public decimal TotalDiscountValue { get; set; }

        public decimal TotalUserPurchaseValue { get; set; }


    }
}