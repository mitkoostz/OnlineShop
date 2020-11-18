import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, RequiredValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SSL_OP_DONT_INSERT_EMPTY_FRAGMENTS } from 'constants';
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
  lastOrder = {} as IOrder;
  address = {} as IAddress;
  hasDefaultAddress = false;
  addressForm: FormGroup;
  editStart = false;
  

  constructor(private userOrderService: UserOrdersService,
     private accountService: AccountService,
     private fb: FormBuilder,
     private toast: ToastrService,
     private router: Router,
     private activateRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.createAddressForm();
    this.addressForm.disable();
    this.currentUser$ = this.accountService.currentUser$;
    this.loadUserOrders();
    this.loadUserAddress();
    this.checkIfSelected();    
  }
  checkIfSelected(){
     let selected = this.activateRoute.snapshot.paramMap.get('selected');
    if( selected !== null &&  !isNaN(Number(selected)))
    {
          this.changeActive(parseInt(selected));
    }else if(selected !== null && isNaN(Number(selected)))
    {
      this.router.navigateByUrl('/not-found');
    }
  }

  loadUserOrders(){
    this.userOrderService.getUserOrders().subscribe(response =>{
    this.orders = response;
    this.loadLastOrder();
    }, error => {
      console.log(error);
    });
  }
  loadLastOrder(){
    if(this.orders.length > 0){
      this.lastOrder = this.orders[0];
    }
  }
  loadUserAddress(){
    this.accountService.getUserAddress().subscribe(address => {
      
      if(address){
        this.addressForm.patchValue(address);
        this.address = address;
        this.hasDefaultAddress = true;
      }else{
        this.editAddress();
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

  RedirectToOrderInfo(id:number){
      this.router.navigateByUrl("/order/"+id.toString());
  }

  editAddress(){
    this.editStart = true;
    this.addressForm.enable();
  }
  createAddressForm(){
    this.addressForm = this.fb.group({
      firstName: [null,Validators.required],
      lastName: [null,Validators.required],
      street: [null,Validators.required],
      city: [null,Validators.required],
      state: [null,Validators.required],
      zipcode: [null,Validators.required]
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
        this.router.navigateByUrl('/not-found');
        break;
    }
  }

}
