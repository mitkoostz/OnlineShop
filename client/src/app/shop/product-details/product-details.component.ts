import { Component, OnInit } from '@angular/core';
import { IProduct } from '../../shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { IUser } from 'src/app/shared/models/user';
import { AccountService } from 'src/app/account/account.service';
import { ReviewsServiceService } from '../reviews-service.service';
import { IReview } from 'src/app/shared/models/review';
import { IProductReview } from 'src/app/shared/models/productReview';
import { ThrowStmt } from '@angular/compiler';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  currentUser$: Observable<IUser>;
  reviewForm: FormGroup;
  product: IProduct;
  productReviews: IProductReview[] = [];
  quantity = 1;
  userReviewRating: number = 0;
  mouseOveredUserReview: boolean = false;
  userReviewCommentLength: number = 0;
  review = {} as IReview;
  userSubmitedReview = {} as IProductReview;
  hasUserSubmitedReview = false;
  averageProductRating: number = 0;
  totalProductReviews: number = 0;

  constructor(private shopService: ShopService,
           private activateRoute: ActivatedRoute,
           private bcService: BreadcrumbService,
           private basketService: BasketService,
           private toastr: ToastrService,
           private router: Router,
           private reviewService: ReviewsServiceService,
           private accountService: AccountService) {
           this.bcService.set('@productDetails', " ");
    }

  ngOnInit(): void {
    this.loadProduct();
    this.loadProductReviews();
    this.checkIfUserHasReview();
    this.currentUser$ = this.accountService.currentUser$;
    this.createReviewForm();
  }

  onSubmitReview() {
    if (localStorage.getItem('token') === null) {
      this.router.navigateByUrl("/account/login");
      return;
    }
    if (this.userReviewRating === 0 || this.userReviewRating > 5) {
      this.toastr.info("Please pick star rate!");
      return;
    }
    if (this.userReviewCommentLength > 600) {
      this.toastr.error("Max review comment characters are 600!");
      return;
    }
    this.review.comment = this.reviewForm.get("comment").value;
    this.review.starRating = this.userReviewRating;
    this.review.productId = this.product.id;

    this.reviewService.submitReview(this.review).subscribe(response => {
      this.loadProduct();
      this.loadProductReviews();
      this.checkIfUserHasReview();
      this.toastr.success("Thank you for submitting review!")
    }, error => {
      console.log(error);
    });

  }
  loadProductReviews() {
    this.reviewService.getProductReviews(+this.activateRoute.snapshot.paramMap.get('id')).subscribe(response => {
      this.productReviews = response.reviews;
      this.totalProductReviews = response.totalReviews;
    }, error => {
      console.log(error);
    });
  }
  loadMoreReviews(){
    this.reviewService.getProductReviews(+this.activateRoute.snapshot.paramMap.get('id'), this.productReviews.length,3).subscribe(response => {
      this.productReviews.push(...response.reviews);
      this.totalProductReviews = response.totalReviews;
    }, error => {
      console.log(error);
    });
  }
  checkIfUserHasReview()
  {
    if(localStorage.getItem('token') === null)
    {
          return;
    }
    this.reviewService.checkIfUserAlreadyHasReview(+this.activateRoute.snapshot.paramMap.get('id')).subscribe(response => {
       if(response)
       {
          this.userSubmitedReview = response;
          this.hasUserSubmitedReview = true;
       }else{
          this.hasUserSubmitedReview = false;
       }
    }, error => {
      console.log(error);
    });
  }

  createReviewForm(){
    this.reviewForm = new FormGroup({
      rating: new FormControl(''),
      comment: new FormControl('', Validators.maxLength(600))
    });
  }

  addItemToBasket(){
    this.basketService.addItemToBasket(this.product, this.quantity);
  }

  incrementQuantity(){
    if(this.quantity < 10){
      this.quantity++;
    }

  }
  decrementQuantity(){
    if(this.quantity > 1){
      this.quantity--;
    }
  }

  loadProduct() {
    this.shopService.getProduct(+this.activateRoute.snapshot.paramMap.get('id')).subscribe(product => {
      this.product = product;
      this.averageProductRating = product.averageReviewRate;
      this.bcService.set('@productDetails', product.name);
    }, error => {
        console.log(error);
    });

  }

  CalculateCommentLength(event){
    this.userReviewCommentLength = event;
  }

}
