using System;

namespace Api.Dtos.ProductReviews
{
    public class ProductReviewReturnDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime Date { get; set; }

        public int StarRate { get; set; }

        public bool HasComment {get;set;}

        public string Comment { get; set; }

        public int ReviewLikes { get; set; }

        public int ReviewDislikes { get; set; }
        
        public int TotalProductReviews { get; set; }

    }
}