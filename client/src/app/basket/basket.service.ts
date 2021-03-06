import { HttpClient } from '@angular/common/http';
import { ThrowStmt } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/Basket';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IProduct } from '../shared/models/product';
import { SettingsConstants } from '../shared/models/SettingsConstants';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
baseUrl = environment.apiUrl;
private basketSource = new BehaviorSubject<IBasket>(null);
basket$ = this.basketSource.asObservable();
private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
basketTotal$ = this.basketTotalSource.asObservable();
shipping = 0;

  constructor(private http: HttpClient,private toastr: ToastrService) {}

  createPaymentIntent(){
    return this.http.post(this.baseUrl + 'payments/' + this.getCurrentBasketValue().id , {} )
       .pipe(
        map((basket: IBasket) => {
             this.basketSource.next(basket);
        })
      );
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod){
     this.shipping = deliveryMethod.price;
     const basket = this.getCurrentBasketValue();
     basket.deliveryMethodId = deliveryMethod.id;
     basket.shippingPrice = deliveryMethod.price;
     this.calculateTotals();
     this.setBasket(basket);
  }

  getBasket(id: string){
    return this.http.get(this.baseUrl + "basket?id=" + id)
         .pipe(
           map((basket: IBasket) =>{
               if(basket.items.length === 0){
                 localStorage.removeItem("basket_id");
                   return this.basketSource.next(null);
               }
               this.basketSource.next(basket);
               this.shipping = basket.shippingPrice;

               this.calculateTotals();
           })
         );
  }
  isNotNull<T>(value: T): value is NonNullable<T> {
    return value != null;
  }

  setBasket(basket: IBasket){
    return this.http.post(this.baseUrl + "basket" , basket).subscribe((response: IBasket ) =>{
        this.basketSource.next(response);
        this.calculateTotals();
     }, error => {
       this.toastr.error(error.errors)
     });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1){
            const itemtoAdd: IBasketItem = this.mapProductItemToBasketItem(item,quantity);
            let basket = this.getCurrentBasketValue();
            if (basket === null) {
              basket = this.createBasket();
            }
            if(basket.items.length >= SettingsConstants.MaxItemsPerBasket && basket.items.filter(i => i.id === itemtoAdd.id).length === 0){
              var errorMessage = `You can add maximum ${SettingsConstants.MaxItemsPerBasket} items in basket`;
              this.toastr.error(errorMessage);
              return;
            }
            if(basket.items.filter(i => i.id === itemtoAdd.id).length > 0 && basket.items.find(i => i.id === itemtoAdd.id).quantity >= 20){
              this.toastr.error(`Max item quantity is ${SettingsConstants.MaxQuantityPerItem}.`);
              return;
            }
            basket.items = this.AddOrUpdateBasket(basket.items, itemtoAdd ,quantity );
            this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem) {
    if(item.quantity >= 20){
      this.toastr.error(`Max item quantity is ${SettingsConstants.MaxQuantityPerItem}.`);
      return;
    }
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }
  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }
  }
  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if (basket.items.some(x => x.id === item.id)) {
      basket.items = basket.items.filter(i => i.id !== item.id);
      if (basket.items.length > 0) {
        this.setBasket(basket);
      } else {
        this.deleteBasket(basket);
      }
    }
  }

  deleteLocalBasket(id: string){
       this.basketSource.next(null);
       this.basketTotalSource.next(null);
       localStorage.removeItem('basket_id')
  }

  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }
  private AddOrUpdateBasket(items: IBasketItem[], itemtoAdd: IBasketItem, quantity: number): IBasketItem[] {

     const index = items.findIndex(i => i.id === itemtoAdd.id);
     if(index === -1){
       itemtoAdd.quantity == quantity;
       items.push(itemtoAdd);
     }else{
       items[index].quantity += quantity;
     }

     return items;
  }
  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id' , basket.id);
    return basket;

  }
  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = this.shipping;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({shipping, total, subtotal});
  }
   private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      productGenderBase: item.productGenderBase,
      type: item.productType

    };

  }
}
