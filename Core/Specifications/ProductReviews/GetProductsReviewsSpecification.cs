using System;
using System.Linq.Expressions;
using Core.Entities.Reviews;

namespace Core.Specifications.ProductReviews
{
    public class GetProductsReviewsSpecification : BaseSpecification<ProductReview>
    {
        public GetProductsReviewsSpecification(int productId, int currentLoaded, int reviewsToTake)
         : base(review => review.ProductId == productId)
        {
            AddOrderByDescending(review => review.StarRate);
            ApplyPaging(currentLoaded,reviewsToTake);
        }
    }
}