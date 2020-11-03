import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserOrdersComponent } from './user-orders/user-orders.component';
import { UserOrdersRoutingModule } from './user-orders-routing/user-orders-routing.module';
import { OrderComponent } from './order/order.component';



@NgModule({
  declarations: [UserOrdersComponent, OrderComponent],
  imports: [
    CommonModule,
    UserOrdersRoutingModule,
    
  ]
})
export class UserOrdersModule { }
