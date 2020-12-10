using System;
using System.Linq.Expressions;
using Core.Entities.Reviews;

namespace Core.Specifications.ProductReviews
{
    public class ProductReviewFindByProductId : BaseSpecification<ProductReview>
    {
        public ProductReviewFindByProductId(int productId) 
        : base(review => review.ProductId == productId)
        {
        }
    }
}