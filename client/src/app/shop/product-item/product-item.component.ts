import { Component, OnInit, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from '../../shared/models/product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent implements OnInit {
  @Input() product: IProduct;


  constructor( private basketService: BasketService) { }

  ngOnInit(): void {
  }

  AddItemToBasket(){
    
    this.basketService.addItemToBasket(this.product);
  }

}