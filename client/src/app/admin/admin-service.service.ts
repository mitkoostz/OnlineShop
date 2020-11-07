import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IAdminActionHistory, IOrdersForDayWeekMounth } from '../shared/models/admin';
import { IOrder } from '../shared/models/Order';
import { IProductToAdd } from '../shared/models/productToAdd';

@Injectable({
  providedIn: 'root'
})
export class AdminServiceService {
  baseUrl = environment.apiUrl;
  
  constructor(private http: HttpClient) { }

  AddNewProduct(product: IProductToAdd)
  {   
    console.log(this.baseUrl + "admin/addnewproduct");
    this.http.put<IProductToAdd>(this.baseUrl + "admin/addnewproduct", product).subscribe(data => { 
  });
  }

  loadAdminActionHistory(){
    return this.http.get<IAdminActionHistory[]>(this.baseUrl + "admin/adminhistory").pipe(
      map(response => {        
        return response;
      }));
  }
  loadOrdersForDayWeekMounth(){
    return this.http.get<IOrdersForDayWeekMounth>(this.baseUrl + "admin/getordersfordayweekmounth").pipe(
      map(response => {        
        return response;
      }));
  }
  loadAllOrders(){
    return this.http.get<IOrder[]>(this.baseUrl + "admin/getallorders").pipe(
      map(response => {        
        return response;
      }));
  }
  
}
