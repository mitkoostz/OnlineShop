using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; } 
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.10,double.MaxValue, ErrorMessage="Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue , ErrorMessage="Quantity must be greater than 0")]
        public int Quantity { get; set; }
        [Required]
        public string PictureUrl {get;set;}
        [Required]
        public string ProductGenderBase { get; set; }
        [Required]
        public string Type { get; set; }
    }
}