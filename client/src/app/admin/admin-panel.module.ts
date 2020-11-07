import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddProductComponent } from './add-product/add-product.component';
import { AdminPanelComponent } from './admin-panel.component';
import { OrderManagmentComponent } from './order-managment/order-managment.component';



@NgModule({
  declarations: [AdminPanelComponent, AddProductComponent, OrderManagmentComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    AdminRoutingModule,
    ReactiveFormsModule
  ]
})
export class AdminPanelModule { }
