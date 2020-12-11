import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import { map, delay } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopparams';
import { IProduct } from '../shared/models/product';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = 'https://localhost:5001/api/';
  products: IProduct[] = [];
  barnds: IBrand[] = [];
  types: IType[] = [];
  pagination = new Pagination();
  shopParams = new ShopParams();

  constructor(private http: HttpClient) { }


  getProducts(useCache: boolean) {
    if(useCache === false){
        this.products = [];
    }
    if(this.products.length > 0 && useCache === true){
      const pagesReceived = Math.ceil(this.products.length / this.shopParams.pageSize);
      if(this.shopParams.pageNumber <= pagesReceived){

        this.pagination.data = this.products.slice(
          (this.shopParams.pageNumber - 1 ) * 
           this.shopParams.pageSize , this.shopParams.pageNumber * this.shopParams.pageSize);

           return of(this.pagination);
      }
    }
    let params = new HttpParams();

    if (this.shopParams.ProductGenderBaseId !== 0) {
      params = params.append('ProductGenderBaseId', this.shopParams.ProductGenderBaseId.toString());
    }
    if (this.shopParams.typeId !== 0) {
      params = params.append('typeId', this.shopParams.typeId.toString());
    }

    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber.toString());
    params = params.append('pageSize', this.shopParams.pageSize.toString());


    return this.http.get<IPagination>(this.baseUrl + 'products', { observe: 'response', params })
      .pipe(
        map(response => {
          this.products  = [...this.products, ...response.body.data];
          this.pagination = response.body;
          return this.pagination;
        }
      ));

  }

  setShopParams(params:ShopParams){
    this.shopParams = params;
  }
  getShopParams(){
    return this.shopParams;
  }

  getProduct(id: number) {
    //caching is removed becouse of reviews not updating
    // const product = this.products.find(p => p.id === id);
    // if(product){
    //   return of(product);
    // }
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }
    getBrands() {
      if (this.barnds.length > 0){
        return of(this.barnds);
      }
       return this.http.get<IBrand[]>(this.baseUrl + 'products/brands').pipe(
         map(response => {
            this.barnds = response;
            return response;
         })
       );
    }
    getTypes() {
      if (this.types.length > 0){
        return of(this.types);
      }
    return this.http.get<IType[]>(this.baseUrl + 'products/types').pipe(
      map(response => {
        this.types = response;
        return response;
      })
    );

    }



}
