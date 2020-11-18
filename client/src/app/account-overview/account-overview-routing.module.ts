import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AccountOverviewComponent } from './account-overview.component';

const routes:Routes = [
  {path: '', component: AccountOverviewComponent},
  {path: ':selected', component: AccountOverviewComponent}

]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)

  ],
  exports: [RouterModule]
})
export class AccountOverviewRoutingModule { }
