using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Reviews
{
    public class ProductReview : BaseEntity
    {
        public ProductReview()
        {
            this.ReviewLikes = 0;
            this.ReviewDislikes = 0;
        }
             
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserEmail { get; set; }

        public Product Product { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int StarRate { get; set; }

        [Required]
        public bool HasComment {get;set;}

        public string Comment { get; set; }

        public int ReviewLikes { get; set; }

        public int ReviewDislikes { get; set; }

        [NotMapped]
        public int TotalProductReviews { get; set; }

    }
}