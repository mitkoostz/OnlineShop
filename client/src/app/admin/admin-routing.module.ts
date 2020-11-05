import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin-panel.component';
import { AddProductComponent } from './add-product/add-product.component';


const routes: Routes = [
  { path: '', component: AdminPanelComponent },
  { path: 'AddProduct', component: AddProductComponent }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)

  ],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
