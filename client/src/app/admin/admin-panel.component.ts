import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../account/account.service';
import { IAdminActionHistory, IOrdersForDayWeekMounth } from '../shared/models/admin';
import { IUser } from '../shared/models/user';
import { AdminServiceService } from './admin-service.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent implements OnInit {
  currentUser$: Observable<IUser>;
  adminHistory: IAdminActionHistory[] = [];
  ordersForDayWeekMounth = {} as IOrdersForDayWeekMounth;
  activityParse = ["Add", "Delete", "Update" ]
  constructor(private accountSerivce: AccountService, private adminService: AdminServiceService) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountSerivce.currentUser$;
    this.loadAdminHistory();
    this.loadOrdersForDayWeekMounth();    
  }
  
  loadAdminHistory(){
      this.adminService.loadAdminActionHistory().subscribe(response => {
         this.adminHistory = response;
      }, error => {
        console.log(error);
      });   
  }
  loadOrdersForDayWeekMounth(){
    this.adminService.loadOrdersForDayWeekMounth().subscribe(response => {
      this.ordersForDayWeekMounth = response;
    }, error => {
      console.log(error);
    }); 
  }



  


}
