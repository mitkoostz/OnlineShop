import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AccountService } from '../account/account.service';
import { IAddress } from '../shared/models/address';
import { IOrder } from '../shared/models/Order';
import { IUser } from '../shared/models/user';
import { UserOrdersService } from '../user-orders/user-orders.service';

@Component({
  selector: 'app-account-overview',
  templateUrl: './account-overview.component.html',
  styleUrls: ['./account-overview.component.scss']
})
export class AccountOverviewComponent implements OnInit {
  overviewCollapsed = false;
  addressCollapsed = true;
  ordersCollapsed = true;
  settingsCollapsed = true;
  activeButton = 1;
  currentUser$: Observable<IUser>;
  orders: IOrder[] = [];
  address = {} as IAddress;
  addressForm: FormGroup;
  editStart = false;

  constructor(private userOrderService: UserOrdersService, private accountService: AccountService,private fb: FormBuilder,private toast: ToastrService) { }

  ngOnInit(): void {
    this.createAddressForm();
    this.addressForm.disable();
    this.currentUser$ = this.accountService.currentUser$;
    this.loadUserOrders();
    this.loadUserAddress();
  }

  loadUserOrders(){
    this.userOrderService.getUserOrders().subscribe(order =>{
    this.orders = order;
    }, error => {
      console.log(error);
    });
  }
  loadUserAddress(){
    this.accountService.getUserAddress().subscribe(address => {
      if(address){
        this.addressForm.patchValue(address);
        this.address = address;
      }
    }, error => {
      console.log(error);
    });
  }
  onAddressUpdate(){
    this.accountService.updateUserAddress(this.addressForm.value)
    .subscribe((address: IAddress) => {
      this.addressForm.reset(address);
       this.toast.success("Address saved");
       this.loadUserAddress();
       this.editStart = false;
       this.addressForm.disable();
     }, error => {
       this.toast.error(error.message);
       console.log(error);
   });
  }

  editAddress(){
    this.editStart = true;
    this.addressForm.enable();
  }
  createAddressForm(){
    this.addressForm = this.fb.group({
      firstName: [null],
      lastName: [null],
      street: [null],
      city: [null],
      state: [null],
      zipcode: [null]
    });
  }
    
  changeActive(activeId: number){
    this.activeButton = activeId;
    switch (activeId) {
      case 1:
        this.overviewCollapsed = false;
        this.addressCollapsed = true;
        this.ordersCollapsed = true;
        this.settingsCollapsed = true;
        break;
      case 2:
        this.overviewCollapsed = true;
        this.addressCollapsed = false;
        this.ordersCollapsed = true;
        this.settingsCollapsed = true;
        break;
      case 3:
        this.overviewCollapsed = true;
        this.addressCollapsed = true;
        this.ordersCollapsed = false;
        this.settingsCollapsed = true;
        break;
      case 4:
        this.overviewCollapsed = true;
        this.addressCollapsed = true;
        this.ordersCollapsed = true;
        this.settingsCollapsed = false;
        break;
      case 5:
        
        break;
    
      default:
        break;
    }
  }

}
