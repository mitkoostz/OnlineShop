import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { CantactUsComponent } from './cantact-us/cantact-us.component';

const routes:Routes = [
  {path: '', component: HomeComponent},
  {path: 'Contact', component: CantactUsComponent}
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)

  ]
})
export class HomeRoutingModule { }
