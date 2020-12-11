using System.Collections.Generic;

namespace Api.Dtos.ProductReviews
{
    public class ProductReviewsDataReturn
    {
        public IReadOnlyList<ProductReviewReturnDto> Reviews { get; set; }
        public int TotalReviews { get; set; }
    }
}