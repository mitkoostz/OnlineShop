using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.ProductReviews
{
    public class ProductReviewDto
    {
        
       [Required]
       public int ProductId { get; set; }
       [Required]
       [Range(1,5)]
       public int StarRating { get; set; }
       public string Comment { get; set; }

    }
}