import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountOverviewRoutingModule } from './account-overview-routing.module';
import { AccountOverviewComponent } from './account-overview.component';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [AccountOverviewComponent],
  imports: [
    CommonModule,
    AccountOverviewRoutingModule,
    ReactiveFormsModule,
    CollapseModule.forRoot(),
  ]
})
export class AccountOverviewModule { }
