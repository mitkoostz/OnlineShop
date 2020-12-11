import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IProductReview, IProductReviewData } from '../shared/models/productReview';
import { IReview } from '../shared/models/review';

@Injectable({
  providedIn: 'root'
})
export class ReviewsServiceService {

  constructor(private http: HttpClient) { }
   baseUrl = environment.apiUrl;

  submitReview(review: IReview)
  {
    return  this.http.post<IReview[]>(this.baseUrl + "productReview", review ).pipe(
      map(response => {
        return response;
      }));
  }

  getProductReviews(productId:number, currentLoaded = 0, reviewsToTake = 3)
  {
    let params = new HttpParams();
    params = params.append('productId', productId.toString());
    params = params.append('currentLoaded', currentLoaded.toString());
    params = params.append('reviewsToTake', reviewsToTake.toString());


    return  this.http.get<IProductReviewData>(this.baseUrl + "productReview", {params } ).pipe(
      map(response => {
        return response;
      }));
  }

  // checkIfThereAreMoreReviews(productId:number, currentLoaded = 0)
  // {
  //   let params = new HttpParams();
  //   params = params.append('productId', productId.toString());
  //   params = params.append('currentLoaded', currentLoaded.toString());

  //   return  this.http.get<boolean>(this.baseUrl + "productReview/checkformoreviews", {params } ).pipe(
  //     map(response => {
  //       return response;
  //     }));
  // }

}
