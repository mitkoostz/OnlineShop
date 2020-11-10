import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { IOrder } from 'src/app/shared/models/Order';
import { environment } from 'src/environments/environment';
import { AdminServiceService } from '../admin-service.service';

@Component({
  selector: 'app-order-managment',
  templateUrl: './order-managment.component.html',
  styleUrls: ['./order-managment.component.scss']
})
export class OrderManagmentComponent implements OnInit {

  constructor(private adminService: AdminServiceService,private fb: FormBuilder) { }
  baseUrl = environment.apiUrl.replace("api/","");
  orders: IOrder[] = [];
  loadedOrder = {} as IOrder;
  ordersSearchForm: FormGroup;
  statusParse = ["Pending", "Payment Recevied", "Payment Failed"]
  ngOnInit(): void {
    this.getOrders(new HttpParams());
    this.createOrderFilterForm();
  }

  createOrderFilterForm(){
    this.ordersSearchForm = this.fb.group({
      dateSearch: new FormControl(''),
      emailSearch: new FormControl(''),
      nameSearch: new FormControl(''),
      paymentIntentSearch: new FormControl('')
  });
  }
  onSubmit(){
    let params = new HttpParams();
    let nameSearch = this.ordersSearchForm.get('nameSearch').value;
    let emailSearch = this.ordersSearchForm.get('emailSearch').value;
    let dateSearch = this.ordersSearchForm.get('dateSearch').value;
    let paymentIntentSearch = this.ordersSearchForm.get('paymentIntentSearch').value;

    if(nameSearch != "" ){
      params = params.append('nameSearch', nameSearch);

    }
    if(emailSearch != "" ){
      params = params.append('emailSearch', emailSearch);

    }
    if(dateSearch != ""){
      params = params.append('dateSearch', dateSearch);

    }
    if(paymentIntentSearch != ""){
      params = params.append('paymentIntentSearch', paymentIntentSearch);

    }
    this.getOrders(params);
    
  }

  getOrders(params: HttpParams){
    this.adminService.loadOrders(params).subscribe(response => {
      this.orders = response;
   }, error => {
     console.log(error);
   });  
  }
   loadOrCloseClickedOrder(id:number){
     if(id === this.loadedOrder.id){
      this.loadedOrder = {} as IOrder;
      return;
     }
     if(this.isEmptyObject(this.loadedOrder)){
      this.loadedOrder = this.orders.find(o => o.id === id);
     }else{
       this.loadedOrder = this.orders.find(o => o.id === id);
     }
   }
   closeLoadedOreder(){
    this.loadedOrder = {} as IOrder;
   }


  isEmptyObject(obj) {
    return (obj && (Object.keys(obj).length === 0));
  }

}
