import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AddProductComponent } from './add-product/add-product.component';
import { AdminPanelComponent } from './admin-panel.component';
import { OrderManagmentComponent } from './order-managment/order-managment.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { ProductManagerComponent } from './product-manager/product-manager.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { DeleteProductComponent } from './delete-product/delete-product.component';



@NgModule({
  declarations: [AdminPanelComponent, AddProductComponent, OrderManagmentComponent, ProductManagerComponent, ProductEditComponent, DeleteProductComponent],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    CollapseModule.forRoot(),
  ]
})
export class AdminPanelModule { }
