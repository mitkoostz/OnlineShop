import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IOrder } from '../shared/models/Order';

@Injectable({
  providedIn: 'root'
})
export class UserOrdersService {
baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUserOrders(){
    return this.http.get<IOrder[]>(this.baseUrl + "Orders");
  }

  getUserOrder(id: number){
    return this.http.get<IOrder>(this.baseUrl + "Orders/" + id.toString());
  }
}
