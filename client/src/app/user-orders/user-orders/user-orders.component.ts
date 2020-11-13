import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IOrder } from 'src/app/shared/models/Order';
import { UserOrdersService } from '../user-orders.service';

@Component({
  selector: 'app-user-orders',
  templateUrl: './user-orders.component.html',
  styleUrls: ['./user-orders.component.scss']
})
export class UserOrdersComponent implements OnInit {


  constructor(private userOrderService: UserOrdersService, private router: Router) { }
 orders: IOrder[] = [];

  ngOnInit(): void {
      this.userOrderService.getUserOrders().subscribe(order =>{
          this.orders = order;
    }, error => {
      console.log(error);
    });
  }

  goToViewOrder(id: number){
    this.router.navigateByUrl( 'order/' + id );
  }


}
