import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { error } from 'protractor';
import { IOrder } from 'src/app/shared/models/Order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { UserOrdersService } from '../user-orders.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {

  constructor(private userOrderService: UserOrdersService
    , private activateRoute: ActivatedRoute, 
      private breadcrumbService: BreadcrumbService) {
          this.breadcrumbService.set('@Order', 'Order #');

       }
  order: IOrder;
  isBasket = false;

  ngOnInit(): void {
    this.userOrderService.getUserOrder(+this.activateRoute.snapshot.paramMap.get('id')).subscribe( or => {
      this.order = or;
      this.breadcrumbService.set('@Order', 'Order - '+ or.status );

    }, error => {
      console.log(error);
    });
  }

 
  

}
