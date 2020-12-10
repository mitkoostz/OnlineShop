import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
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
}
