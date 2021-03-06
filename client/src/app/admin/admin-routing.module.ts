import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel.component';
import { AddProductComponent } from './add-product/add-product.component';
import { OrderManagmentComponent } from './order-managment/order-managment.component';
import { ProductManagerComponent } from './product-manager/product-manager.component';


const routes: Routes = [
  { path: '', component: AdminPanelComponent },
  { path: 'productmanager', component: ProductManagerComponent },
  { path: 'productmanager/addnewproduct', component: AddProductComponent },
  { path: 'ordersmanager', component: OrderManagmentComponent }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)

  ],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
