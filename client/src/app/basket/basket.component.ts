import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { IBasket, IBasketItem } from '../shared/models/Basket';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
basket$: Observable<IBasket>;

  constructor(private basketService: BasketService, private router:Router) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }

  EmptyBasketRedirectToShop(){
      this.router.navigateByUrl('/shop');
      //this.toastr.info("Your cart is empty ")
  }

  removeBasketItem(item: IBasketItem){
    this.basketService.removeItemFromBasket(item);
  }

  incrementItemQuantity(item: IBasketItem){
    this.basketService.incrementItemQuantity(item);

  }
  decrementItemQuanity(item: IBasketItem){
    this.basketService.decrementItemQuantity(item);
  }
}
