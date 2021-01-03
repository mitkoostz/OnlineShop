using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.ProductDiscounts
{
    public class ProductDiscountCode : BaseEntity
    {

        [Required]
        [MaxLength(6)]
        public string CodeName { get; set; }
        
        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int DiscountTypeId { get; set; }
        
        public DiscountType DiscountType { get; set; }

        [Required]
        public DateTime DiscountStartingDate { get; set; }

        [Required]
        public DateTime DiscountValidToDate { get; set; }

        public decimal DiscountPercent { get; set; }

        public decimal DiscountConstValue { get; set; }

    }
}