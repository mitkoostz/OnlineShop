import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
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
  
}
