import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { IProduct } from 'src/app/shared/models/product';
import { IType } from 'src/app/shared/models/productType';
import { ShopService } from 'src/app/shop/shop.service';
import { AdminServiceService } from '../admin-service.service';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { ProductEditComponent } from '../product-edit/product-edit.component';
import { DeleteProductComponent } from '../delete-product/delete-product.component';


@Component({
  selector: 'app-product-manager',
  templateUrl: './product-manager.component.html',
  styleUrls: ['./product-manager.component.scss']
})
export class ProductManagerComponent implements OnInit {
products: IProduct[] = [];
types: IType[] = [];
productFilterForm: FormGroup;
bsModalRef: BsModalRef;
  constructor(private adminService: AdminServiceService, private fb: FormBuilder, private shopService: ShopService,private modalService: BsModalService) { }

  ngOnInit(): void {
        this.loadProducts(new HttpParams());
        this.loadTypes();
        this.createProductFilterForm();
  }

  createProductFilterForm(){
    this.productFilterForm = this.fb.group({
      nameSearch: new FormControl(''),
      filterSearch: new FormControl(''),
      typeFilter: new FormControl(''),
      genderFilter: new FormControl('')
  });
}
onSubmit(){
  let params = new HttpParams();
  let nameSearch = this.productFilterForm.get('nameSearch').value;
  let filterSearch = this.productFilterForm.get('filterSearch').value;
  let typeFilter = this.productFilterForm.get('typeFilter').value;
  let genderFilter = this.productFilterForm.get('genderFilter').value;

  if(nameSearch != "" ){
    params = params.append('search', nameSearch);

  }
  if(filterSearch != "" ){
    params = params.append('sort', filterSearch);

  }
  if(typeFilter != ""){
    params = params.append('typeId', typeFilter);

  }
  if(genderFilter != ""){
    params = params.append('productGenderBaseId', genderFilter);
  }

   this.loadProducts(params);
}

  loadProducts(par:HttpParams){
    this.adminService.loadProducts(par).subscribe(response => {
      this.products = response;
   }, error => {
     console.log(error);
   });  
  }

  loadTypes(){
    this.shopService.getTypes().subscribe(response => {
      this.types = response;
   }, error => {
     console.log(error);
   });  
  }

  openProductEdit(id: number){
    let initialState  = this.products.find(x => x.id === id);

    this.bsModalRef = this.modalService.show(ProductEditComponent, {initialState});
    this.bsModalRef.content.closeBtnName = 'Close';
  }
  openProductDelete(id: number){
    let initialState  = this.products.find(x => x.id === id);

    this.bsModalRef = this.modalService.show(DeleteProductComponent,{initialState, class:"modal-lg"});
    this.bsModalRef.content.closeBtnName = 'Close';
  }
  refresh(){
    this.loadProducts(new HttpParams());

  }
  

}
