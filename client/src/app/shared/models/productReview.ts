export interface IProductReview {
  id: string;
  userName: string;
  date: string;
  starRate: number;
  hasComment: boolean;
  comment: string;
  reviewLikes: number;
  reviewDislikes: number;
  totalProductReviews: number;
}

export interface IProductReviewData {
  reviews: IProductReview[],
  totalReviews: number
}
