import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserOrdersComponent } from '../user-orders/user-orders.component';
import { RouterModule } from '@angular/router';
import { OrderComponent } from '../order/order.component';

const routes = [
  {path: '', component: UserOrdersComponent },
  { path: ':id', component: OrderComponent , data: { breadcrumb: { alias: 'Order' } } }

];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
    
  ],
  exports: [RouterModule]
})
export class UserOrdersRoutingModule { }
