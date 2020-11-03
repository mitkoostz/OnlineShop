import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route } from '@angular/compiler/src/core';
import { BasketComponent } from './basket.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path:'', component: BasketComponent}
];



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class BasketRoutingModule { }
