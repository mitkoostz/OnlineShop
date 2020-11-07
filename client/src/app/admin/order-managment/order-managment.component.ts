import { Component, OnInit } from '@angular/core';
import { IOrder } from 'src/app/shared/models/Order';
import { AdminServiceService } from '../admin-service.service';

@Component({
  selector: 'app-order-managment',
  templateUrl: './order-managment.component.html',
  styleUrls: ['./order-managment.component.scss']
})
export class OrderManagmentComponent implements OnInit {

  constructor(private adminService: AdminServiceService) { }
  orders: IOrder[] = [];
  loadedOrder = {} as IOrder;
  statusParse = ["Pending", "Payment Recevied", "Payment Failed"]
  ngOnInit(): void {
    console.log(this.loadedOrder);
    this.getAllOrders();
  }

  getAllOrders(){
    this.adminService.loadAllOrders().subscribe(response => {
      this.orders = response;
      console.log(this.orders);
   }, error => {
     console.log(error);
   });  
  }
   loadOrCloseClickedOrder(id:number){
     if(this.isEmptyObject(this.loadedOrder)){
      this.loadedOrder = this.orders.find(o => o.id === id);
     }else{
       this.loadedOrder = {} as IOrder;
     }
   }


  isEmptyObject(obj) {
    return (obj && (Object.keys(obj).length === 0));
  }

}
